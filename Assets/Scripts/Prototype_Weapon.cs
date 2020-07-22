using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using System;

//Kurtis Watson
public class Prototype_Weapon : MonoBehaviour
{
    private Player_Controller r_playerController;
    private Prototype_Classes r_prototypeClasses;
    private Enemy_Controller enemyHit;

    private Animator anim;

    private LineRenderer m_lr;

    private Transform shotPoint;
    public GameObject particles;

    public GameObject m_hitDamageText;
    public GameObject beamParticles;

    public float m_laserDamage;

    public float m_damageCoolDown;
    private float m_currentDamageCoolDown;

    public AudioSource beamNoise;

    // Use this for initialization
    void Start()
    {
        m_damageCoolDown = 0.2f;
        m_currentDamageCoolDown = m_damageCoolDown;

        r_playerController = FindObjectOfType<Player_Controller>();
        r_prototypeClasses = FindObjectOfType<Prototype_Classes>();

        m_lr = GetComponent<LineRenderer>();
        beamNoise.volume = PlayerPrefs.GetFloat("volumeLevel");
        beamNoise.enabled = false;
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        anim = GetComponentInChildren<Animator>();
        f_animation();
        f_prototypeWeapon();

        shotPoint = GameObject.Find("Staff_Whole").transform.FindChild("Orb").transform;
        particles.transform.position = shotPoint.position;

        m_currentDamageCoolDown -= Time.deltaTime;
        m_laserDamage = UnityEngine.Random.Range(10, 15);
    }


    [System.Obsolete]
    void f_prototypeWeapon()
    {
        RaycastHit m_laserHit;
        if (Input.GetKey(KeyCode.Mouse0) && r_prototypeClasses.stonePower[r_prototypeClasses.classState] > 0 && r_playerController.isSprinting == false)
        {
            beamParticles.active = true;
            beamNoise.enabled = true;
            r_prototypeClasses.stonePower[r_prototypeClasses.classState] -= 0.025f;
            m_lr.SetPosition(0, shotPoint.position);
            m_lr.enabled = true;
            if (Physics.SphereCast(shotPoint.position, 0.2f, shotPoint.forward, out m_laserHit)) //SphereCast allows for a thicker Raycast.
            {
                anim.SetBool("Firing", true);
                if (m_laserHit.collider)
                {
                    m_lr.SetPosition(1, m_laserHit.point);
                }
                Debug.DrawRay(shotPoint.position, shotPoint.forward * m_laserHit.distance);
                if (m_laserHit.collider.gameObject.CompareTag("Enemy") && m_currentDamageCoolDown <= 0)
                {
                    enemyHit = m_laserHit.collider.gameObject.GetComponent<Enemy_Controller>();
                    switch (r_prototypeClasses.classState)
                    {
                        case 0: //Yellow
                            m_laserDamage = m_laserDamage * 2;
                            break;
                        case 1: //White
                            break;
                        case 2: //Pink
                            enemyHit.m_isStunned = true;
                            break;
                        case 3: //Blue
                            r_playerController.playerHealth += 1;
                            break;
                    }
                    m_laserHit.collider.gameObject.GetComponent<Enemy_Controller>().m_enemyHealth -= m_laserDamage;      
                    GameObject textObject = Instantiate(m_hitDamageText, m_laserHit.point, Quaternion.identity);
                    textObject.GetComponentInChildren<TextMeshPro>().text = "" + m_laserDamage;
                    m_currentDamageCoolDown = m_damageCoolDown;
                }
            }
        }
        else
        {
            m_lr.enabled = false;
            beamNoise.enabled = false;
            beamParticles.active = false;
            anim.SetBool("Firing", false);
        }

        switch (r_prototypeClasses.classState)
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
        beamParticles.GetComponent<ParticleSystem>().startColor = m_lr.startColor;
    }

    void f_animation()
    {
        anim.SetBool("Run", r_playerController.isSprinting);
    }
}