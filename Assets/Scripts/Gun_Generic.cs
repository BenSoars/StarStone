using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Gun_Generic : MonoBehaviour
{

    // Ben Soars
    [Header("Weapon Ammo")]
    [Space (2)]
    public int m_maxAmmo = 6; // the maximum ammo
    public int m_currentAmmo = 6; // the current ammo
    public int m_ammoPerShot = 1; // how much ammo shooting will consume

    [Space(2)]
    [Header("Weapon Accuracy")]
    [Space(2)]
    public bool m_isAccurate = true;
    public List<float> m_accuracyMinRange = new List<float>();
    public List<float> m_accuracyMaxRange = new List<float>();
    public List<float> m_accuracyGenerated = new List<float>();
    private Vector3 m_newAccuracy;
    
    [Space(2)]
    [Header("Weapon Other Stats")]
    [Space(2)]
    public float m_shotForce = 100; // how much force the bullet will be shot for, it hitscan leave blank
    public int m_bulletDamage = 1;

    [Space(2)]
    [Header("Shot Type")]
    [Space(2)]
    public Rigidbody m_physicalBullet; // leave this blank if the gun is hitscan
    public Transform m_shotPoint; // the point where the bullet is shot from
    private Rigidbody m_shotBullet;

    private RaycastHit m_hitscanCast; // the hitscan raycast
    public GameObject hitSpark;

    private Text m_ammoCount; // the ui element displaying the current ammo
    private Player_Controller m_player;

    void Start()
    {
        m_ammoCount = GameObject.Find("AmmoCount").GetComponent<Text>();
        m_player = GameObject.FindObjectOfType<Player_Controller>();
        f_updateUI();
    }

    void f_ShootGun()
    {
        for (int i = 0; i < m_ammoPerShot; i++)
        {
            if (!m_isAccurate)
            {
                m_newAccuracy = f_BulletSpread();
            }
            else
            {
                m_newAccuracy = m_shotPoint.forward;
            }

            if (m_physicalBullet) // if a physical bullet is there
            {
                m_shotBullet = Instantiate(m_physicalBullet, m_shotPoint.transform.position, Quaternion.identity) as Rigidbody; // shoot bullet
                m_shotBullet.AddForce(m_newAccuracy * m_shotForce); // push bullet out
            }
            else
            {
                if (Physics.Raycast(m_shotPoint.position, m_newAccuracy, out m_hitscanCast, Mathf.Infinity)) // shoot out a raycast for hitscan
                { 
                    Debug.DrawRay(m_shotPoint.position, m_newAccuracy * m_hitscanCast.distance, Color.yellow);
                    Instantiate(hitSpark, m_hitscanCast.point, Quaternion.identity);

                    if (m_hitscanCast.transform.gameObject.CompareTag("Enemy"))
                    {
                        m_hitscanCast.transform.gameObject.GetComponent<Enemy_Controller>().m_enemyHealth -= m_bulletDamage;
                        
                    }
                }
            }

            m_accuracyGenerated.Clear(); // clear the generated accuracy
            m_currentAmmo -= 1; // decrease total ammo counter by the ammo consuption of the gun
        }
    }

    Vector3 f_BulletSpread()
    {
        for (int j = 0; j < m_accuracyMaxRange.Count; j++)
        {
            m_accuracyGenerated.Add(Random.Range(m_accuracyMinRange[j], m_accuracyMaxRange[j])); // randomly generate accuracy
        }
        Vector3 accuracy = new Vector3(m_shotPoint.forward.x + m_accuracyGenerated[0], m_shotPoint.forward.y + m_accuracyGenerated[1], m_shotPoint.forward.z + m_accuracyGenerated[2]); // send back accuracy
        return accuracy;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && m_player.m_isSprinting == false && m_player.m_isPlayerActive == true)
        {
            if (m_currentAmmo > 0)
            {
                f_ShootGun();
            }
            f_updateUI();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ammo"))
        {
            m_currentAmmo += other.GetComponent<AmmoReplenish>().AmmoWorth;
            if (m_currentAmmo > m_maxAmmo) { m_currentAmmo = m_maxAmmo; }
            Destroy(other.gameObject);
            f_updateUI();
        }
    }
    
    void f_updateUI()
    {
        m_ammoCount.text = (m_currentAmmo.ToString() + "/" + m_maxAmmo.ToString());
    }
}
