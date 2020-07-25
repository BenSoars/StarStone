using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using System;

//Kurtis Watson
public class Prototype_Weapon : MonoBehaviour
{
    [Header("Referenced Scripts")]
    [Space(2)]
    private Player_Controller m_playerController;
    private Prototype_Classes m_prototypeClasses;
    public Clock_Controller clockController;
    private Enemy_Controller m_enemyHit;

    [Header("Weapon Mechanics")]
    [Space(2)]
    private Animator m_anim;
    private LineRenderer m_lr;
    private Transform m_shotPoint;
    public GameObject particles;
    public GameObject hitDamageText;
    public GameObject beamParticles;
    public float laserDamage;
    public float damageCoolDown;
    private float m_currentDamageCoolDown;
    public AudioSource beamNoise;

    // Use this for initialization
    void Start()
    {
        damageCoolDown = 0.2f;
        m_currentDamageCoolDown = damageCoolDown;

        m_playerController = FindObjectOfType<Player_Controller>();
        m_prototypeClasses = FindObjectOfType<Prototype_Classes>();

        m_lr = GetComponent<LineRenderer>();
        beamNoise.volume = PlayerPrefs.GetFloat("volumeLevel");
        beamNoise.enabled = false;
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        m_anim = GetComponentInChildren<Animator>();
        f_animation();
        f_prototypeWeapon();

        m_shotPoint = GameObject.Find("Staff_Whole").transform.FindChild("Orb").transform; //Constantly update shotpoint as the orb changes position.
        particles.transform.position = m_shotPoint.position;

        m_currentDamageCoolDown -= Time.deltaTime; //Have a cooldown for each hit of the laser.
        laserDamage = UnityEngine.Random.Range(10, 15); //Make the damage of the staff random.
    }


    [System.Obsolete]
    void f_prototypeWeapon()
    {
        RaycastHit m_laserHit; //Create a raycast.

        if (Input.GetKey(KeyCode.Mouse0) && m_prototypeClasses.stonePower[m_prototypeClasses.classState] > 0 && m_playerController.isSprinting == false && clockController.canShoot == true && m_prototypeClasses.defaultStaff.active == false) //Checks for default staff because sometimes it would shoot a laser.
        {
            beamParticles.active = true; //Enable particle effect.
            beamNoise.enabled = true; //Enable audio.
            m_prototypeClasses.stonePower[m_prototypeClasses.classState] -= 0.025f; //Decrease current starstone power (drain).
            m_lr.SetPosition(0, m_shotPoint.position); //Set position of laser.
            m_lr.enabled = true; //Enable laser.
            if (Physics.SphereCast(m_shotPoint.position, 0.2f, m_shotPoint.forward, out m_laserHit)) //SphereCast allows for a thicker Raycast.
            {
                m_anim.SetBool("Firing", true); //Start shooting animation.
                if (m_laserHit.collider)
                {
                    m_lr.SetPosition(1, m_laserHit.point); //Shoot the laser towards where the laser hits.
                }

                if (m_laserHit.collider.gameObject.CompareTag("Enemy") && m_currentDamageCoolDown <= 0) //Check if the laser hits the enemy.
                {
                    m_enemyHit = m_laserHit.collider.gameObject.GetComponent<Enemy_Controller>(); //Grab the enemy hits controller script.
                    switch (m_prototypeClasses.classState) //Each stone has a different buff.
                    {
                        case 0: //Yellow
                            laserDamage = laserDamage * 2; //Double damage.
                            break;
                        case 1: //White
                            damageCoolDown = 0.125f;
                            break;
                        case 2: //Pink
                            m_enemyHit.m_isStunned = true; //Stun enemy.
                            break;
                        case 3: //Blue
                            m_playerController.playerHealth += 0.15f; //Increase player health.
                            break;
                    }
                    m_enemyHit.m_enemyHealth -= laserDamage; //Damage the enemy.   
                    GameObject textObject = Instantiate(hitDamageText, m_laserHit.point, Quaternion.identity); //Spawn hit text on player.
                    textObject.GetComponentInChildren<TextMeshPro>().text = "" + laserDamage; //Change the value of the instantiated text to the value the enemy is hit for.
                    m_currentDamageCoolDown = damageCoolDown;
                }
            }
        }
        else
        {
            damageCoolDown = 0.2f; //Reset damage cooldown.
            m_lr.enabled = false; //Disable line renderer.
            beamNoise.enabled = false; //Stop audio.
            beamParticles.active = false; //Disable particles.
            m_anim.SetBool("Firing", false); //Stop animation.
        }

        switch (m_prototypeClasses.classState) //Set line render colours.
        {
            case 0:
                m_lr.SetColors(Color.yellow, Color.yellow);             
                break;
            case 1:
                m_lr.SetColors(Color.white, Color.white);
                break;
            case 2:
                m_lr.SetColors(Color.magenta, Color.magenta);
                break;
            case 3:
                m_lr.SetColors(Color.blue, Color.blue);
                break;
        }
        beamParticles.GetComponent<ParticleSystem>().startColor = m_lr.startColor; //Set colour of particles equal to the colours set above based on class state.
    }

    void f_animation()
    {
        m_anim.SetBool("Run", m_playerController.isSprinting); //Animate player sprinting based on 'isSprinting' value in referenced script.
    }
}