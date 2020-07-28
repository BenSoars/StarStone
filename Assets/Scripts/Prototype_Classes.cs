using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public Gun_Generic m_gunGeneric;

    [Header("Weapon Mechanics")]
    [Space(2)]
    [Tooltip("Position where the laser is shot from.")]
    public Transform shotPoint;
    public bool canSwitch;

    [Header("Default Stats Before Buffs")]
    [Space(2)]
    private float m_defaultDamage;
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
    public GameObject defaultStaff;

    [Header("Environment")]
    [Space(2)]

    [Tooltip("Set the starting intensity of the fog at round start.")]
    public float fogStrength;
    public float currentFog;

    void Start()
    { 
        for (int i = 0; i < activeStone.Length; i++) //Set Starstone powers at first launch.
        {
            stonePower[i] = Random.Range(30, 80);
        }

        m_runeAnim = FindObjectsOfType<Rune_Controller>(); //Reference Rune_Controller.
        m_playerController = FindObjectOfType<Player_Controller>();
        m_waveSystem = FindObjectOfType<Wave_System>();

        currentFog = fogStrength; //Set current fog.
        canSelect = true; //Allow the player to select a starstone.
        canSwitch = true; //Allow the player to switch weapons.

        m_defaultDamage = m_gunGeneric.m_bulletDamage;
        m_defaultDefence = m_playerController.defenceValue; //Set default values.
        m_defaultHealth = m_playerController.playerHealth;
        m_defaultDamageCooldown = prototypeWeapon.damageCoolDown;
        defaultStaff.active = true; //Enable the default staff.
    }

    void Update()
    {
        f_startstoneSelect();
        f_enemyBuff();
        f_chargeStones();
        f_ability();
        f_setStoneColor();

        if(m_waveSystem.curRound > 0)
        {
            defaultStaff.active = false; //Disable staff.
        }
    }

    void f_defaultSettings() //Reset to 0
    {
        for (int i = 0; i < activeStone.Length; i++)
        {
            activeStone[classState] = false;
            staffs[classState].active = false;
        }

        m_gunGeneric.m_bulletDamage = m_defaultDamage;
        m_waveSystem.notChosen = false; //Set players weapon state to not chosen to indicate they need to select a weapon.
        currentFog = fogStrength; //Reset fog.
        RenderSettings.fog = false;
        m_playerController.defenceValue = m_defaultDefence;
        prototypeWeapon.damageCoolDown = m_defaultDamageCooldown;
        m_waveSystem.isIntermission = false;
        m_waveSystem.newWave = true; //Set new wave.
        canSelect = false;
    }

    void f_startstoneSelect()
    {
        RaycastHit m_stoneSelect;

        if (Physics.Raycast(shotPoint.position, shotPoint.forward, out m_stoneSelect, 3f, 1 << 11) && Input.GetKeyDown("f") && canSelect == true) //Create a raycast that checks for layer 11.
        {
            buffChosen = true; //If the player select their prototype buff, this is set true.
            f_animateRunes(); //Start rune shake.

            f_defaultSettings();
            switch (m_stoneSelect.collider.gameObject.name) //Set values/buffs of the prototype weapons.
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
                    prototypeWeapon.damageCoolDown = m_defaultDamageCooldown / 2;
                    break;
            }

            staffs[classState].active = true;
            activeStone[classState] = true;
        }

        if (m_waveSystem.notChosen == true) //This will run if the player doesn't select a new starstone in time.
        {
            f_animateRunes(); //Begin rune animation.
            buffChosen = true;
            f_defaultSettings();

            classState = Random.Range(0, 3); //Select a random number for their prototype choice.
            staffs[classState].active = true;
            activeStone[classState] = true;
        }

        if (buffChosen == true)
        {
            GameObject.Find("Canvas").GetComponent<User_Interface>().runtimeUI.active = true; //Enable in game UI.
            buffChosen = false;
            float max = int.MinValue; 
            for (int i = 0; i < activeStone.Length; i++) //This will determine the highest valued starstone after the enemy has chosen.
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
                m_gunGeneric.m_bulletDamage = m_defaultDamage * 0.80f;
                break;
            case 1:
                fogStrength = Mathf.Lerp(fogStrength, currentFog, Time.deltaTime * 2); //Smooth fog adjustment.
                RenderSettings.fogDensity = fogStrength;
                RenderSettings.fog = true;
                break;
            case 2:
                m_gunGeneric.m_bulletDamage = m_defaultDamage * 0.75f;
                break;
            case 3:
                m_gunGeneric.m_bulletDamage = m_defaultDamage * 0.70f;
                break;
        }
    }

    void f_ability()
    {
        if (canSwitch == true)
        {
            if (Input.GetKeyDown("q"))
            {
                abilityState = !abilityState; //Set the state true or false based on current bool value.
                stateQ = true;
            }
            if (Input.GetKeyDown("v"))
            {
                abilityState = !abilityState;
                stateV = true;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetAxis("Mouse ScrollWheel") < 0f) //If the player user the scroll wheel when they have abilities active it will switch back to weapon state.
            {
                abilityState = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && stateQ == true)
        {
            stateQ = false;
            switch (classState) //Check for Class State.
            {
                case 0:
                    if (stonePower[0] >= 15) //Check if the player has enough power in their current starstone.
                    {
                        FindObjectOfType<Ability_Handler>().StartCoroutine("f_spawnInvisibility"); //Begin Coroutine to execute ability.
                        stonePower[0] -= 15; //Decrease starstone power as it has been 'drained'.
                        canSwitch = false; //Don't allow the player to switch weapons.
                    }
                    break;
                case 1:
                    if (stonePower[1] >= 15)
                    {
                        FindObjectOfType<Ability_Handler>().StartCoroutine("f_spawnTornado");
                        stonePower[1] -= 15;
                        canSwitch = false;
                    }
                    break;
                case 2:
                    if (stonePower[2] >= 15)
                    {
                        FindObjectOfType<Ability_Handler>().StartCoroutine("f_spawnPushback");
                        stonePower[2] -= 15;
                        canSwitch = false;
                    }
                    break;
                case 3:
                    if (stonePower[3] >= 15)
                    {
                        FindObjectOfType<Ability_Handler>().StartCoroutine("f_spawnHealthPad");
                        stonePower[3] -= 15;
                        canSwitch = false;
                    }
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && stateV == true)
        {
            stateV = false;
            switch (classState)
            {
                case 0:
                    if (stonePower[0] >= 20)
                    {
                        FindObjectOfType<Ability_Handler>().StartCoroutine("f_spawnWall"); //Begin Coroutine to execute ability.
                        stonePower[0] -= 20; //Decrease starstone power as it has been 'drained'.
                        canSwitch = false; //Don't allow the player to switch weapons.
                    }
                    break;
                case 1:
                    if (stonePower[1] >= 25)
                    {
                        FindObjectOfType<Ability_Handler>().StartCoroutine("f_spawnStorm");
                        stonePower[1] -= 25;
                        canSwitch = false;
                    }
                    break;
                case 2:
                    if (stonePower[2] >= 15)
                    {
                        FindObjectOfType<Ability_Handler>().StartCoroutine("f_spawnKnives");
                        stonePower[2] -= 15;
                        canSwitch = false;
                    }
                    //Knives.
                    break;
                case 3:
                    if (stonePower[3] >= 25)
                    {
                        FindObjectOfType<Ability_Handler>().StartCoroutine("f_spawnInfector");
                        stonePower[3] -= 25;
                        canSwitch = false;
                    }
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
                if (activeStone[i] != true && stonePower[i] < 100) //Charge any stone that isn't current active in the players staff.
                {
                    stonePower[i] += 0.003f;
                }
            }
        }
    }

    void f_setStoneColor() //Set the colours of the stones based on class state.
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
            m_runeAnim[i].animated = true; //Gather all runes in the scene and set their animations to true.
        }
    }
}
