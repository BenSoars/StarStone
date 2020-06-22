using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prototype_Classes : MonoBehaviour
{
    //Kurtis Watson
    private Player_Controller r_playerController;
    public Prototype_Weapon m_prototypeWeapon;

    public Transform m_shotPoint;

    public GameObject m_pushBack;

    public int m_classState;

    private float m_defaultDefence;
    private float m_defaultHealth;
    private float m_defaultDamageCooldown;

    public int[] m_stonePower;

    void Start()
    {
        m_classState = 0;

        r_playerController = FindObjectOfType<Player_Controller>();

        m_defaultDefence = r_playerController.m_defenceValue;
        m_defaultHealth = r_playerController.m_playerHealth;
        m_defaultDamageCooldown = m_prototypeWeapon.m_damageCoolDown;
    }

    void Update()
    {
        f_startstoneSelect();
        f_ability();

        if (Input.GetKeyDown("p"))
        {
            m_classState = 0;
        }

        if (m_classState == 0)
        {
            f_defaultSettings();
        }    
    }

    void f_defaultSettings() //Reset to 0
    {
        r_playerController.m_defenceValue = m_defaultDefence;
        m_prototypeWeapon.m_damageCoolDown = m_defaultDamageCooldown;
    }

    void f_startstoneSelect()
    {
        RaycastHit m_stoneSelect;

        if (Physics.Raycast(m_shotPoint.position, m_shotPoint.forward, out m_stoneSelect, 3f, 1 << 11) && Input.GetKeyDown("f"))
        {
            switch (m_stoneSelect.collider.gameObject.name)
            {
                case ("Starstone 1"): //Yellow
                    f_defaultSettings();
                    m_classState = 0;
                    r_playerController.m_defenceValue = 0.75f;
                    break;
                case ("Starstone 2"): //White
                    f_defaultSettings();
                    m_classState = 1;
                    r_playerController.m_playerHealth = m_defaultHealth * 1.3f;
                    break;
                case ("Starstone 3"): //Pink
                    f_defaultSettings();
                    m_classState = 2;
                    break;
                case ("Starstone 4"): //Blue
                    f_defaultSettings();
                    m_classState = 3;
                    m_prototypeWeapon.m_damageCoolDown = m_defaultDamageCooldown / 2;
                    break;
            }
        }
    }

    void f_ability()
    {
        if (Input.GetKeyDown("q"))
        {
            switch (m_classState)
            {
                case 0:
                    r_playerController.m_isPlayerInvisible = true;
                    Invoke("f_resetInvisible", 10);
                    break;
                case 1:
                    //Cloud of bullets. -15% power.
                    break;
                case 2:
                    Instantiate(m_pushBack, m_shotPoint.transform.position, m_shotPoint.rotation); // 'm_shotPoint.rotation' makes the position of firing relative to where the player is looking based on camera rotation.
                    break;
                case 3:
                    //Ring of blue fire that enemies can't come into. -10% power.
                    break;
            }
        }

        if (Input.GetKeyDown("v"))
        {
            switch (m_classState)
            {
                case 0:
                    f_wallAbility();
                    break;
                case 1:
                    f_stormAbility();
                    break;
                case 2:
                    f_knifeAbility();
                    //Knives.
                    break;
                case 3:

                    break;
            }           
        }
    }

    void f_wallAbility() {
        if (m_stonePower[0] >= 20)
        {
            FindObjectOfType<Ability_Handler>().f_spawnWall();
            m_stonePower[0] -= 20;
        }
    }

    void f_stormAbility()
    {
        if (m_stonePower[1] >= 25)
        {
            FindObjectOfType<Ability_Handler>().f_spawnStorm();
            m_stonePower[1] -= 25;
        }
    }

    void f_knifeAbility()
    {
        if (m_stonePower[2] >= 25)
        {
            FindObjectOfType<Ability_Handler>().f_spawnKnives();
            //m_stonePower[2] -= 25;
        }
    }







    void f_resetInvisible()
    {
        r_playerController.m_isPlayerInvisible = false;
    }
}
