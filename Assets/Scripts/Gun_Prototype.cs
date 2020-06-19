using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class Gun_Prototype : MonoBehaviour
{
    private LineRenderer m_lr;

    public Transform m_shotPoint;

    public GameObject m_hitDamageText;

    public float m_laserDamage;

    public int m_classState;

    public float m_coolDown;
    private float m_currentCooldown;


    // Use this for initialization
    void Start()
    {
        m_currentCooldown = m_coolDown;

        m_classState = 0;
        m_lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        f_startstoneSelect();
        f_prototypeClasses();

        m_currentCooldown -= Time.deltaTime;
        m_laserDamage = Random.Range(5, 10);
    }

    void f_startstoneSelect()
    {
        RaycastHit m_stoneSelect;

        if (Physics.Raycast(m_shotPoint.position, m_shotPoint.forward, out m_stoneSelect, 2f, 1 << 11) && Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            switch (m_stoneSelect.collider.gameObject.name)
            {
                case ("Starstone 1"):
                    m_classState = 0;
                    break;
                case ("Starstone 2"):
                    m_classState = 1;
                    break;
                case ("Starstone 3"):
                    m_classState = 2;
                    break;
                case ("Starstone 4"):
                    m_classState = 3;
                    break;
            }
        }
    }

    [System.Obsolete]
    void f_prototypeClasses()
    {
        RaycastHit m_laserHit;
        if (Input.GetKey(KeyCode.Mouse0))
        {
            m_lr.SetPosition(0, transform.position);
            m_lr.enabled = true;
            if (Physics.Raycast(m_shotPoint.position, m_shotPoint.forward, out m_laserHit))
            {
                if (m_laserHit.collider)
                {
                    m_lr.SetPosition(1, m_laserHit.point);
                }

                if (m_laserHit.collider.gameObject.CompareTag("Enemy") && m_currentCooldown <= 0)
                {
                    switch (m_classState)
                    {
                        case 0:
                            m_laserDamage = m_laserDamage * 2;
                            break;
                        case 1:
                            m_laserHit.collider.gameObject.GetComponent<Enemy_Controller>().m_runSpeed = 0;
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                    }
                    m_laserHit.collider.gameObject.GetComponent<Enemy_Controller>().m_enemyHealth -= m_laserDamage;      
                    GameObject textObject = Instantiate(m_hitDamageText, m_laserHit.point, Quaternion.identity);
                    textObject.GetComponentInChildren<TextMeshPro>().text = "" + m_laserDamage;
                    m_currentCooldown = m_coolDown;
                }
            }
        }
        else
        {
            m_lr.enabled = false;
        }

        switch (m_classState)
        {
            case 0:
                m_lr.SetColors(Color.red, Color.red);
                break;
            case 1:
                m_lr.SetColors(Color.yellow, Color.yellow);
                break;
            case 2:
                m_lr.SetColors(Color.blue, Color.blue);
                break;
            case 3:
                m_lr.SetColors(Color.green, Color.green);
                break;
        }
    }
}