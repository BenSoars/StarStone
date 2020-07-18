using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station_Controller : MonoBehaviour
{
    public Transform cameraLook;

    public Animator animator;

    private bool isUpgrading;
    private bool weaponUpgraded;

    private Weapon_Switch weaponSwitch;

    public GameObject weaponHands;
    public GameObject repairHands;

    // Start is called before the first frame update
    void Start()
    {
        weaponSwitch = FindObjectOfType<Weapon_Switch>();
    }

    // Update is called once per frame
    void Update()
    {

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && animator.GetCurrentAnimatorStateInfo(0).IsName("upgradeWeapon1") || animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && animator.GetCurrentAnimatorStateInfo(0).IsName("upgradeWeapon2"))
        {
            weaponUpgraded = true;

            
        }

        RaycastHit m_stationHit;

        if (Physics.Raycast(cameraLook.transform.position, cameraLook.transform.forward, out m_stationHit, 5f))
        {
            if (m_stationHit.collider.gameObject.tag == "Upgrade" && Input.GetKeyDown("f"))
            {
                if (weaponUpgraded == false && weaponSwitch.currentWeapon == 1 || weaponSwitch.currentWeapon == 2)
                {
                    Debug.Log(animator);
                    isUpgrading = true;
                    switch (weaponSwitch.currentWeapon)
                    {
                        case 1:
                            animator.SetBool("Upgrading1", true);
                            break;
                        case 2:
                            animator.SetBool("Upgrading2", true);
                            break;
                    }
                }

                if (weaponUpgraded == true)
                {
                    weaponUpgraded = false;
                    Debug.Log("Yes");
                    weaponHands.active = true;
                    repairHands.active = false;
                    isUpgrading = false;
                    
                    animator.SetBool("Upgrading1", false);
                    animator.SetBool("Upgrading2", false);

                }
            }
        }

        if(isUpgrading == true)
        {
            weaponHands.active = false;
            repairHands.active = true;
        }
    }
}
