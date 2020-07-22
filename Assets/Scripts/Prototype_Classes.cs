using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

//Kurtis Watson
public class Prototype_Classes : MonoBehaviour
{
    [Header("Script References")]
    [Space(2)]
    private Player_Controller m_playerController;
    public Prototype_Weapon prototypeWeapon;
    private Wave_System m_waveSystem;

    [Header("Weapon Mechanics")]
    [Space(2)] 
    public Transform shotPoint;
    public bool canSwitch;

    [Header("Default Stats Before Buffs")]
    [Space(2)]
    private float m_defaultDefence;
    private float m_defaultHealth;
    private float m_defaultDamageCooldown;

    [Header("Staff Mechanics")]
    [Space(2)]
    public GameObject[] staffs;
    public bool abilityState;
    public bool stateQ;
    public bool stateV;

    [Header("Rune Mechanics")]
    [Space(2)]
    private Rune_Controller[] m_runeAnim;

    [Header("Stone Mechanics")]
    [Space(2)]
    public int classState;
    public float[] stonePower;
    public bool[] activeStone;
    public Color stoneColor;
    public bool canSelect;
    public int chosenBuff;
    public bool buffChosen;

    [Header("Environment")]
    [Space(2)]
    public float m_fogStrength;
    public float m_currentFog;

    void Start()
    { 
        for (int i = 0; i < activeStone.Length; i++) //Set Starstone powers at first launch.
        {
            stonePower[i] = Random.Range(30, 80);
        }

        m_runeAnim = FindObjectsOfType<Rune_Controller>(); //Reference Rune_Controller.
        m_playerController = FindObjectOfType<Player_Controller>();
        m_waveSystem = FindObjectOfType<Wave_System>();

        m_currentFog = m_fogStrength; //Set current fog.
        canSelect = true; //Allow the player to select a starstone.
        canSwitch = true;

        m_defaultDefence = m_playerController.defenceValue;
        m_defaultHealth = m_playerController.playerHealth;
        m_defaultDamageCooldown = prototypeWeapon.m_damageCoolDown;
        //m_defaultBulletDamage = r_gunGeneric.m_bulletDamage;
    }

    void Update()
    {
        f_startstoneSelect();
        f_enemyBuff();
        f_chargeStones();
        f_ability();
        f_setStoneColor();
    }

    void f_defaultSettings() //Reset to 0
    {
        for (int i = 0; i < activeStone.Length; i++)
        {
            activeStone[classState] = false;
            staffs[classState].active = false;
        }
        m_waveSystem.notChosen = false;
        m_currentFog = m_fogStrength;
        RenderSettings.fog = false;
        m_playerController.defenceValue = m_defaultDefence;
        prototypeWeapon.m_damageCoolDown = m_defaultDamageCooldown;
        m_waveSystem.m_isIntermission = false;
        m_waveSystem.m_newWave = true;
        canSelect = false;
    }

    void f_startstoneSelect()
    {
        RaycastHit m_stoneSelect;

        if (Physics.Raycast(shotPoint.position, shotPoint.forward, out m_stoneSelect, 3f, 1 << 11) && Input.GetKeyDown("f") && canSelect == true)
        {
            buffChosen = true;
            f_animateRunes();

            f_defaultSettings();
            switch (m_stoneSelect.collider.gameObject.name)
            {
                case ("Yellow"): //Yellow
                    classState = 0;
                    m_playerController.defenceValue = 0.75f;
                    break;
                case ("White"): //White
                    classState = 1;
                    m_playerController.playerHealth = m_defaultHealth * 1.3f;
                    break;
                case ("Pink"): //Pink
                    classState = 2;
                    break;
                case ("Blue"): //Blue
                    classState = 3;
                    prototypeWeapon.m_damageCoolDown = m_defaultDamageCooldown / 2;
                    break;
            }

            staffs[classState].active = true;
            activeStone[classState] = true;
        }

        if (m_waveSystem.notChosen == true)
        {
            f_animateRunes();
            buffChosen = true;
            f_defaultSettings();


            classState = Random.Range(0, 3);
            staffs[classState].active = true;
            activeStone[classState] = true;
        }

        if (buffChosen == true)
        {
            GameObject.Find("Canvas").GetComponent<User_Interface>().runtimeUI.active = true;
            buffChosen = false;
            float max = int.MinValue;
            for (int i = 0; i < activeStone.Length; i++)
            {
                if (activeStone[i] == false && stonePower[i] > max)
                {
                    max = stonePower[i];
                    chosenBuff = i;
                }
            }
        }
    }

    void f_enemyBuff()
    {
        switch (chosenBuff)
        {
            case 0:

                break;
            case 1:
                //r_gunGeneric.m_bulletDamage = r_gunGeneric.m_bulletDamage * 0.75f;
                m_fogStrength = Mathf.Lerp(m_fogStrength, m_currentFog, Time.deltaTime * 2); //Smooth fog adjustment.
                RenderSettings.fogDensity = m_fogStrength;
                RenderSettings.fog = true;
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }

    void f_ability()
    {
        if (canSwitch == true)
        {
            if (Input.GetKeyDown("q"))
            {
                abilityState = !abilityState;
                stateQ = true;
            }
            if (Input.GetKeyDown("v"))
            {
                abilityState = !abilityState;
                stateV = true;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                abilityState = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && stateQ == true)
        {
            stateQ = false;
            canSwitch = false;
            switch (classState)
            {
                case 0:
                    if (stonePower[0] >= 15)
                    {
                        FindObjectOfType<Ability_Handler>().StartCoroutine("f_spawnInvisibility");
                        stonePower[0] -= 15;
                    }
                    else stateQ = false;
                    break;
                case 1:
                    if (stonePower[1] >= 15)
                    {
                        FindObjectOfType<Ability_Handler>().StartCoroutine("f_spawnTornado");
                        stonePower[1] -= 15;
                    }
                    else stateQ = false;
                    break;
                case 2:
                    if (stonePower[2] >= 15)
                    {
                        FindObjectOfType<Ability_Handler>().StartCoroutine("f_spawnPushback");
                        stonePower[2] -= 15;
                    }
                    else stateQ = false;
                    break;
                case 3:
                    //Ring of blue fire that enemies can't come into. -10% power.
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && stateV == true)
        {
            stateV = false;
            canSwitch = false;
            switch (classState)
            {
                case 0:
                    if (stonePower[0] >= 20)
                    {
                        FindObjectOfType<Ability_Handler>().StartCoroutine("f_spawnWall");
                        stonePower[0] -= 20;
                    }
                    else stateV = false;
                    break;
                case 1:
                    if (stonePower[1] >= 25)
                    {
                        FindObjectOfType<Ability_Handler>().StartCoroutine("f_spawnStorm");
                        stonePower[1] -= 25;
                    }
                    else stateV = false;
                    break;
                case 2:
                    if (stonePower[2] >= 15)
                    {
                        FindObjectOfType<Ability_Handler>().StartCoroutine("f_spawnKnives");
                        stonePower[2] -= 15;
                    }
                    else stateV = false;
                    //Knives.
                    break;
                case 3:
                    if (stonePower[3] >= 25)
                    {
                        FindObjectOfType<Ability_Handler>().f_spawnInfector();
                        stonePower[3] -= 25;
                    }
                    else stateV = false;
                    break;
            }
        }
    }

    void f_chargeStones()
    {
        if (canSelect == false)
        {
            for (int i = 0; i < activeStone.Length; i++)
            {
                if (activeStone[i] != true && stonePower[i] < 100)
                {
                    stonePower[i] += 0.002f;
                }
                //Debug.Log("Star stone: " + i + "    Power: " + stonePower[i]);
            }
        }
    }

    void f_setStoneColor()
    {
        switch (chosenBuff)
        {
            case 0:
                stoneColor = Color.yellow;
                break;
            case 1:
                stoneColor = Color.white;
                break;
            case 2:
                stoneColor = Color.magenta;
                break;
            case 3:
                stoneColor = Color.cyan;
                break;
        }
    }

    void f_animateRunes()
    {
        for (int i = 0; i < m_runeAnim.Length; i++)
        {
            m_runeAnim[i].animated = true;
        }
    }

}
