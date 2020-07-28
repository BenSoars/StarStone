using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//Kurtis Watson
public class Difficulty : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    [Header("Enemies")]
    [Space(2)]
    public GameObject enemySmall;
    public GameObject enemyMedium;
    public GameObject enemyLarge;
    public GameObject enemyRange;

    [Header("Small Enemy")]
    [Space(2)]
    [Tooltip("Set default health for small enemy.")]
    public float defaultSmallHealth;
    [Tooltip("Set default speed for small enemy.")]
    public float defaultSmallSpeed;
    [Tooltip("Set default damage for small enemy.")]
    public float defaultSmallDamage;

    [Header("Medium Enemy")]
    [Space(2)]
    [Tooltip("Set default health for medium enemy.")]
    public float defaultMediumHealth;
    [Tooltip("Set default speed for medium enemy.")]
    public float defaultMediumSpeed;
    [Tooltip("Set default damage for medium enemy.")]
    public float defaultMediumDamage;

    [Header("Large Enemy")]
    [Space(2)]
    [Tooltip("Set default health for large enemy.")]
    public float defaultLargeHealth;
    [Tooltip("Set default speed for large enemy.")]
    public float defaultLargeSpeed;
    [Tooltip("Set default damage for large enemy.")]
    public float defaultLargeDamage;

    [Header("Range Enemy")]
    [Space(2)]
    [Tooltip("Set default health for ranged enemy.")]
    public float defaultRangeHealth;
    [Tooltip("Set default speed for ranged enemy.")]
    public float defaultRangeSpeed;
    [Tooltip("Set default damage for ranged enemy.")]
    public float defaultRangeDamage;

    [Header("Percentage Mechanics")]
    [Space(2)]
    private float percent;
    private bool percentChosen;
    private bool valuesSet;
    private int dropDownValue;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu") //Only update in the Main Menu.
        {
            switch (dropdown.value) //Check current selected dropdown menu value.
            {
                case 0:
                    percent = 0.75f; //Set the percentage based on dropdown selection.
                    break;
                case 1:
                    percent = 1;
                    break;
                case 2:
                    percent = 1.25f;
                    break;
            }
        }

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            enemySmall.GetComponent<Enemy_Controller>().m_enemyHealth = defaultSmallHealth * percent; //Set all of the enemy values based on the percentage.
            enemySmall.GetComponent<Enemy_Controller>().m_enemyDamage = defaultSmallDamage * percent;
            enemySmall.GetComponent<Enemy_Controller>().m_runSpeed = defaultSmallSpeed * percent;

            enemyMedium.GetComponent<Enemy_Controller>().m_enemyHealth = defaultMediumHealth * percent;
            enemyMedium.GetComponent<Enemy_Controller>().m_enemyDamage = defaultMediumDamage * percent;
            enemyMedium.GetComponent<Enemy_Controller>().m_runSpeed = defaultMediumSpeed * percent;

            enemyLarge.GetComponent<Enemy_Controller>().m_enemyHealth = defaultLargeHealth * percent;
            enemyLarge.GetComponent<Enemy_Controller>().m_enemyDamage = defaultLargeDamage * percent;
            enemyLarge.GetComponent<Enemy_Controller>().m_runSpeed = defaultLargeSpeed * percent;

            enemyRange.GetComponent<Enemy_Controller>().m_enemyHealth = defaultRangeHealth * percent;
            enemyRange.GetComponent<Enemy_Controller>().m_enemyDamage = defaultRangeDamage * percent;
            enemyRange.GetComponent<Enemy_Controller>().m_runSpeed = defaultRangeSpeed * percent;
        }
    }
}

    // NOTE \\
    //This is another way to handle difficulty, rather than having the code do it automatically, this will allow for it to be altered manually based on optimisation.

    //switch (dropdown.value)
    //{
    //    case 0:
    //        enemySmall.GetComponent<Enemy_Controller>().m_enemyHealth = 50;
    //        enemySmall.GetComponent<Enemy_Controller>().m_enemyDamage = 3;
    //        enemySmall.GetComponent<Enemy_Controller>().m_runSpeed = 5;

    //        enemyMedium.GetComponent<Enemy_Controller>().m_enemyHealth = 100;
    //        enemyMedium.GetComponent<Enemy_Controller>().m_enemyDamage = 7;
    //        enemyMedium.GetComponent<Enemy_Controller>().m_runSpeed = 4;

    //        enemyLarge.GetComponent<Enemy_Controller>().m_enemyHealth = 200;
    //        enemyLarge.GetComponent<Enemy_Controller>().m_enemyDamage = 15;
    //        enemyLarge.GetComponent<Enemy_Controller>().m_runSpeed = 3;

    //        enemyRange.GetComponent<Enemy_Controller>().m_enemyHealth = 75;
    //        enemyRange.GetComponent<Enemy_Controller>().m_enemyDamage = 5;
    //        enemyRange.GetComponent<Enemy_Controller>().m_runSpeed = 1;
    //        break;
    //    case 1:
    //        enemySmall.GetComponent<Enemy_Controller>().m_enemyHealth = 60;
    //        enemySmall.GetComponent<Enemy_Controller>().m_enemyDamage = 4;
    //        enemySmall.GetComponent<Enemy_Controller>().m_runSpeed = 5;

    //        enemyMedium.GetComponent<Enemy_Controller>().m_enemyHealth = 120;
    //        enemyMedium.GetComponent<Enemy_Controller>().m_enemyDamage = 8;
    //        enemyMedium.GetComponent<Enemy_Controller>().m_runSpeed = 4;

    //        enemyLarge.GetComponent<Enemy_Controller>().m_enemyHealth = 220;
    //        enemyLarge.GetComponent<Enemy_Controller>().m_enemyDamage = 15;
    //        enemyLarge.GetComponent<Enemy_Controller>().m_runSpeed = 4;

    //        enemyRange.GetComponent<Enemy_Controller>().m_enemyHealth = 85;
    //        enemyRange.GetComponent<Enemy_Controller>().m_enemyDamage = 6;
    //        enemyRange.GetComponent<Enemy_Controller>().m_runSpeed = 2;
    //        break;
    //    case 2:
    //        enemySmall.GetComponent<Enemy_Controller>().m_enemyHealth = 70;
    //        enemySmall.GetComponent<Enemy_Controller>().m_enemyDamage = 6;
    //        enemySmall.GetComponent<Enemy_Controller>().m_runSpeed = 5;

    //        enemyMedium.GetComponent<Enemy_Controller>().m_enemyHealth = 140;
    //        enemyMedium.GetComponent<Enemy_Controller>().m_enemyDamage = 9;
    //        enemyMedium.GetComponent<Enemy_Controller>().m_runSpeed = 5;

    //        enemyLarge.GetComponent<Enemy_Controller>().m_enemyHealth = 250;
    //        enemyLarge.GetComponent<Enemy_Controller>().m_enemyDamage = 15;
    //        enemyLarge.GetComponent<Enemy_Controller>().m_runSpeed = 6;

    //        enemyRange.GetComponent<Enemy_Controller>().m_enemyHealth = 100;
    //        enemyRange.GetComponent<Enemy_Controller>().m_enemyDamage = 7;
    //        enemyRange.GetComponent<Enemy_Controller>().m_runSpeed = 2;
    //        break;
    //}
