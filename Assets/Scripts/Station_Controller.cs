using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Station_Controller : MonoBehaviour
{
    [Header("Station Components")]
    [Space(2)]
    public GameObject station;
    private Wave_System m_waveSystem;
    private Weapon_Switch m_weaponSwitch;
    public Transform cameraLook;
    public Animator animator;
    private bool m_isUpgrading;
    private bool m_weaponUpgraded;
    public Animator rockAnim;
    private bool m_disableUpgrade;
    private bool m_showText;
    public TextMeshProUGUI upgradeOpenText;

    [Header("Weapon Handler")]
    [Space(2)]
    public GameObject weaponHands;
    public GameObject repairHands;

    public GameObject weapon1;
    public GameObject weapon2;

    public GameObject upgradedWeapon1;
    public GameObject upgradedWeapon2;

    private bool isUpgrading1;
    private bool isUpgrading2;

    // Start is called before the first frame update
    void Start()
    {
        upgradeOpenText.enabled = false;
        m_weaponSwitch = FindObjectOfType<Weapon_Switch>();
        m_waveSystem = FindObjectOfType<Wave_System>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && animator.GetCurrentAnimatorStateInfo(0).IsName("upgradeWeapon1") || animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && animator.GetCurrentAnimatorStateInfo(0).IsName("upgradeWeapon2")) //Check if the animation is at its end.
        {
            m_weaponUpgraded = true;
        }

        RaycastHit m_stationHit; //Create a raycast.

        if (Physics.Raycast(cameraLook.transform.position, cameraLook.transform.forward, out m_stationHit, 5f)) //Shoot the raycast from the camera forward.
        {
            if (m_stationHit.collider.gameObject.tag == "Upgrade" && Input.GetKeyDown("f") && m_disableUpgrade == false) //Check for an object hit with the tag "Upgrade" and for user events.
            {
                if (m_weaponUpgraded == false && m_weaponSwitch.currentWeapon == 1 || m_weaponSwitch.currentWeapon == 2) //Checks if the current weapon value is equal to 1 or 2.
                {
                    m_isUpgrading = true; //Sets hands to idle (no weapons).
                    switch (m_weaponSwitch.currentWeapon) //Checks for current weapon.
                    {
                        case 1:
                            isUpgrading1 = true;
                            animator.SetBool("Upgrading1", true); //Set the animation true for that specific weapon.
                            break;
                        case 2:
                            isUpgrading2 = true;
                            animator.SetBool("Upgrading2", true);
                            break;
                    }
                }

                if (m_weaponUpgraded == true) //Checks if the upgrade is complete.
                {
                    m_weaponUpgraded = false;
                    weaponHands.active = true; //Enables weapons again.
                    repairHands.active = false; //Disables repair hands.
                    m_isUpgrading = false;

                    switch (m_weaponSwitch.currentWeapon)
                    {
                        case 1:
                            weapon1.GetComponent<Gun_Generic>().damageMultiplier = 1.6f;
                            upgradedWeapon1.active = true;
                            m_disableUpgrade = true;
                            break;
                        case 2:
                            weapon2.GetComponent<Gun_Generic>().damageMultiplier = 1.6f;
                            upgradedWeapon2.active = true;
                            m_disableUpgrade = true;
                            break;
                    }

                    animator.SetBool("Upgrading1", false); //Set value to false as the upgrade is finished.
                    animator.SetBool("Upgrading2", false);
                }
            }
        }

        if(isUpgrading1 == true && m_waveSystem.currentIntermissionTime <= 0)
        {
            m_isUpgrading = false;
            weaponHands.active = true;
            repairHands.active = false;
            animator.SetBool("Upgrading1", false);
        }
        if(isUpgrading2 == true && m_waveSystem.currentIntermissionTime <= 0)
        {
            m_isUpgrading = false;
            weaponHands.active = true;
            repairHands.active = false;
            animator.SetBool("Upgrading2", false);
        }

        if (m_isUpgrading == true) //If upgrade is in progress >
        {
            weaponHands.active = false; //Disable player weapons.   
            repairHands.active = true; //Enable repair hands.
        }


        switch (m_waveSystem.curRound)
        {
            case 4:
                if (m_showText == false)
                {
                    m_showText = true;
                    Invoke("f_resetText", 5);
                    upgradeOpenText.enabled = true;
                    m_disableUpgrade = false; //Allow the player to upgrade their weapon as the upgrade station has arrived.
                    rockAnim.SetBool("Active", true); //Raise rocks to allow the player to access the upgrade station.
                }
                break;
            case 8:
                if (m_showText == false)
                {
                    m_showText = true;
                    Invoke("f_resetText", 5);
                    upgradeOpenText.enabled = true;
                    m_disableUpgrade = false;
                    rockAnim.SetBool("Active", true);
                }
                break;
            default:
                m_showText = false;
                m_disableUpgrade = true;
                rockAnim.SetBool("Active", false); //Lower the rock to stop the player from accessing the upgrade station area.
                break;
        }
    }

    void f_resetText()
    {
        upgradeOpenText.enabled = false;
    }
}
