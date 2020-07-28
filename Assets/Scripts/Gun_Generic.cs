using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Gun_Generic : Melee_Attack
{

    // Ben Soars
    [Header("Weapon Ammo")]
    [Space(2)]

    [Tooltip("Name of the Weapon, used for Specific Ammo Types.")]
    public string m_name = "GUN"; // name of the weapon, used for different ammo types
    [Tooltip("The Maximum Ammo the weapon can hold.")]
    public int m_maxAmmo = 6; // the maximum ammo 
    private int m_savedMaxAmmo; // the saved maximum so it can't go over it
    [Tooltip("The Current Amount of Ammo, will depleat as the gun is fired.")]
    public int m_currentAmmo = 6; // the current ammo
    [Tooltip("The amount of ammo per clip")]
    public int m_clipSize = 6; // the current ammo
    [Tooltip("The scale used to devide the amount of visible ammo on the UI")]
    public int ammoScale = 1; // used for deviding the displayed ammo count for more accurate depictions
    [Tooltip("The ammount of Ammo consumed per each shot, also allows for multiple projectiles per shot.")]
    public int m_ammoPerShot = 1; // how much ammo shooting will consume
    [Tooltip("The time inbetween shots.")]
    public float m_coolDown = 0.5f; // the amount of time until the next shot can happen
    [Tooltip("The time to reload")]
    public float m_reloadTime = 0.5f; // the amount of time until the next shot can happen

    [Tooltip("Damage multiplier")]
    public float damageMultiplier = 1;

    public float m_bulletDamage; // the damage of the bullet
    
    public AudioClip shotSound; // the shot sound

    [Space(2)]
    [Header("Weapon Accuracy")]
    [Space(2)]
    [Tooltip("Used for making the gun accurate, and bypassing the random accuracy.")]
    public bool isAccurate = true; // if the gun is accurate or not
    [Tooltip("The minimum ranges for gun accuracy.")]
    public Vector3 accuracyMinRange; // min range for accuracy
    [Tooltip("The maximum ranges for gun accuracy.")]
    public Vector3 accuracyMaxRange; // max range for accuracy
    private Vector3 m_accuracyGenerated; // the generated accuracy
    private Vector3 m_newAccuracy;

    [Space(2)]
    [Header("Weapon Aim Sights")]
    [Space(2)]
    [Tooltip("Used to enable the ability to aim down sights, doing so automatically makes the gun inaccurate until aimed down sights.")]
    public bool canAim = false; // allows the weapon to aim down sights
    private bool m_isAiming = false; // if the weapon is currently being aimed

    [Tooltip("Access to the first person camera.")]
    public Camera m_playerCam; // the player camera, allows for the changing of FOV for aiming

    //Kurtis Watson and Ben Soars
    [Space(2)]
    [Header("Weapon Other Stats")]
    [Space(2)]
    [Tooltip("The force that will be applied to the physical projectile (if there is a physical projectile).")]
    public float m_shotForce = 100; // how much force the bullet will be shot for, it hitscan leave blank

    [Tooltip("The minimum damage the bullet can deal.")]
    public int m_minBulletDamage; // the minimum damage for the bullet
    [Tooltip("The maximum damage the bullet can deal.")]
    public int m_maxBulletDamage; // the macimum damage for the bullet

    [Space(2)]
    [Header("Shot Type")]
    [Space(2)]
    [Tooltip("The physical projectile, if you want the gun to shoot a hitscan leave this blank.")]
    public Rigidbody m_physicalBullet; // leave this blank if the gun is hitscan
    [Tooltip("The point at which the bullet will shoot from.")]
    public Transform m_shotPoint; // the point where the bullet is shot from
    private Rigidbody m_shotBullet; // the bullet that's physic

    private RaycastHit m_hitscanCast; // the hitscan raycast
    [Tooltip("The hitspark effect that is created when the hitscan hits a wall.")]
    public GameObject hitSpark;

    private Text m_ammoCount; // the ui element displaying the current ammo
    private Text m_ammoTotal; // ui element for displaying total ammo
    private Player_Controller m_player;  // get the player component
    private Audio_System m_audio; // get the audio system component to play sounds

    [Space(2)]
    [Header("Other")]
    [Space(2)]
   
    private float coolDownTimer; // the timer for the cooldown
    [Tooltip("The animator used for gun animator.")]
    public Animator Anim; // the animations for gun
    public LayerMask layerMask;

    //Kurtis Watson
    [Tooltip("The gameobject that is used for the damage text")]
    public GameObject m_hitDamageText; // the hit text
    public Clock_Controller clockController;
  

    void Start()
    {
       
        m_player = GameObject.FindObjectOfType<Player_Controller>(); // get player component
        m_audio = GameObject.FindObjectOfType<Audio_System>(); // get audio system

        m_savedMaxAmmo = m_maxAmmo; // save the max ammo of the weapon
    }

    void OnEnable()
    {
        stopAttack();
        f_updateUI(); // update the UI
    }

    void f_ShootGun() // gun shoot script
    {
        m_bulletDamage = Random.Range(m_minBulletDamage, m_maxBulletDamage) * damageMultiplier; // generate random damage
        if (Anim) // if the gun has an animator
        {
            Anim.SetTrigger("Shot"); // playr shot animations
        }

        coolDownTimer = m_coolDown; // set the cooldown timer
        m_audio.playGun(shotSound); // play shot sound, passed through from this script

        for (int i = 0; i < m_ammoPerShot; i++) // for loop of ammo per shot
        {
            if (!isAccurate) { m_newAccuracy = f_BulletSpread(); } // generate a new accuracy if the gun isn't set to be accurate
            else { m_newAccuracy = m_shotPoint.forward; } // is accurate, uses default accuracy
            
            if (m_physicalBullet) // if a physical bullet is there
            {
                m_shotBullet = Instantiate(m_physicalBullet, m_shotPoint.transform.position, Quaternion.identity) as Rigidbody; // shoot bullet
                m_shotBullet.AddForce(m_newAccuracy * m_shotForce); // push bullet out
                m_shotBullet.GetComponent<Projectile>().m_damage = m_bulletDamage;
            }
            else
            {
                if (Physics.Raycast(m_shotPoint.position, m_newAccuracy, out m_hitscanCast, Mathf.Infinity, ~layerMask)) // shoot out a raycast for hitscan
                {
                    Debug.DrawRay(m_shotPoint.position, m_newAccuracy * m_hitscanCast.distance, Color.yellow); // draw line only viewable ineditor
                    Instantiate(hitSpark, m_hitscanCast.point, Quaternion.identity); // create hitspark at hit point

                    if (m_hitscanCast.transform.gameObject.CompareTag("Enemy")) // if an enemy was hit take away their health based on damage
                    {
                        m_hitscanCast.transform.gameObject.GetComponent<Enemy_Controller>().m_enemyHealth -= m_bulletDamage; // damage the player
                        //Kurtis Watson
                        GameObject m_textObject = Instantiate(m_hitDamageText, m_hitscanCast.point, Quaternion.identity); // spawn a damage 
                        m_textObject.GetComponentInChildren<TextMeshPro>().text = "" + m_bulletDamage; // assign damage dealt to the text
                    }
                }
            }

            m_accuracyGenerated = Vector3.zero; // clear the generated accuracy
            m_currentAmmo -= 1; // decrease total ammo counter by the ammo consuption of the gun
        }
    }

    void onReload()
    {
        Anim.SetTrigger("Reload"); // play reload animation
        m_maxAmmo -= (m_clipSize - m_currentAmmo);
        coolDownTimer = m_reloadTime;
        m_currentAmmo = m_clipSize;
        if (m_maxAmmo <= 0)
        {
            m_maxAmmo = 0;
        }
        
        f_updateUI();
    }

    Vector3 f_BulletSpread()
    {
        for (int j = 0; j < 3; j++)
        {
            switch (j) // switch statement
            {
                case 0: // generate x value
                    m_accuracyGenerated.x = Random.Range(accuracyMinRange.x, accuracyMaxRange.x);
                    break;
                case 1: // generate y value
                    m_accuracyGenerated.y = Random.Range(accuracyMinRange.y, accuracyMaxRange.y);
                    break;

                case 2: // generate z value
                    m_accuracyGenerated.z = Random.Range(accuracyMinRange.z, accuracyMaxRange.z);
                    break;

            }
        }
        Vector3 accuracy = new Vector3(m_shotPoint.forward.x + m_accuracyGenerated.x, m_shotPoint.forward.y + m_accuracyGenerated.y, m_shotPoint.forward.z + m_accuracyGenerated.z); // send back accuracy
        return accuracy; // return the accuracy
    }

    public void f_updateUI() // update the UI to reflect the current ammo count
    {
        if (GameObject.Find("Canvas").GetComponent<User_Interface>().runtimeUI.activeInHierarchy == true)
        {
            if (!m_ammoCount)
            {
                m_ammoCount = GameObject.Find("AmmoCount").GetComponent<Text>(); // get the text for displaying ammo
                m_ammoTotal = GameObject.Find("AmmoTotal").GetComponent<Text>(); // get the text for displaying ammo
            }
            m_ammoCount.text = ((m_currentAmmo / ammoScale).ToString() + "/" + (m_clipSize / ammoScale).ToString());
            m_ammoTotal.text = (m_maxAmmo / ammoScale).ToString();
        }

        
        
    }

    void Update()
    {
        if (Attacking == false)
        {
            if (coolDownTimer >= 0) // if the cooldown is larger than 0
            {
                coolDownTimer -= Time.deltaTime; // decrease the cooldown, acts as a timer
            } else { 
                if ((m_currentAmmo <= 0 || Input.GetKeyDown(KeyCode.R) && m_currentAmmo != m_clipSize) && m_maxAmmo > 0)
                {
                    onReload(); // reload weapon
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse2))
            {
                Anim.SetTrigger("Melee");
            }

            if (m_maxAmmo > m_savedMaxAmmo) { m_maxAmmo = m_savedMaxAmmo; }

            if (m_player.isPlayerActive == true && m_player.isUsingLadder == false) // if the player is active and not on a ladder
            {
                if (m_player.isSprinting == false && coolDownTimer <= 0) // if the player isn't sprinting and the cooldown isn't bigger than 0
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0) && clockController.canShoot == true) // and the player has fired their weapon
                    {
                        if (m_currentAmmo > 0) // if there is ammo
                        {
                            f_ShootGun(); // shoot the gun, run the shoot function
                        }
                        f_updateUI(); // update the ui
                    }
                }

                if (canAim == true) // if the player is able to aim down sights with this weapon
                {
                    if (Input.GetKey(KeyCode.Mouse1) && m_player.isSprinting == false) // if they're holding down right mouse and aren't sprinting
                    {
                        m_isAiming = true; // they are aiming
                        isAccurate = true; // gun becomes accurate
                        m_playerCam.fieldOfView = 40; // change FOV to be smaller
                    }
                    else
                    {
                        m_isAiming = false; // no longer aiming
                        isAccurate = false; // gun becomes inaccurate
                        m_playerCam.fieldOfView = 60; // FOV gets reset
                    }
                }
                else
                {
                    // else reset everything
                    m_isAiming = false;
                    isAccurate = false;
                    m_playerCam.fieldOfView = 60;
                }
            }
        }

        if (Anim) // if there is a gun animatior
        {
            // update gun animations
            Anim.SetBool("Aim", m_isAiming); 
            Anim.SetBool("Run", m_player.isSprinting);
        }
    }

   void playSound(AudioClip soundEffect) // play custom sound effects for aniamtions
    {
        m_audio.playGun(soundEffect);
    }
}
