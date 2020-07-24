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

    [Header("Percentage Mechanics")]
    [Space(2)]
    private float percent;
    private bool percentChosen;
    private bool valuesSet;
    private int dropDownValue;

    [Header("Small Enemy")]
    [Space(2)]
    public float defaultSmallHealth;
    public float defaultSmallSpeed;
    public float defaultSmallDamage;

    [Header("Medium Enemy")]
    [Space(2)]
    public float defaultMediumHealth;
    public float defaultMediumSpeed;
    public float defaultMediumDamage;

    [Header("Large Enemy")]
    [Space(2)]
    public float defaultLargeHealth;
    public float defaultLargeSpeed;
    public float defaultLargeDamage;

    [Header("Range Enemy")]
    [Space(2)]
    public float defaultRangeHealth;
    public float defaultRangeSpeed;
    public float defaultRangeDamage;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        switch (dropdown.value)
        {
            case 0:
                if (percentChosen == false)
                {
                    percentChosen = true;
                    percent = 0.75f;
                    dropDownValue = dropdown.value;
                    valuesSet = false;
                }
                break;
            case 1:
                if (percentChosen == false)
                {
                    percentChosen = true;
                    percent = 1;
                    dropDownValue = dropdown.value;
                    valuesSet = false;
                }
                break;
            case 2:
                if (percentChosen == false)
                {
                    percentChosen = true;
                    percent = 1.25f;
                    dropDownValue = dropdown.value;
                    valuesSet = false;
                }
                break;
        }

        if (dropDownValue != dropdown.value)
        {
            percentChosen = false;
        }

        if (SceneManager.GetActiveScene().name == "MainMenu" && valuesSet == false)
        {
            valuesSet = true;
            enemySmall.GetComponent<Enemy_Controller>().m_enemyHealth = defaultSmallHealth * percent;
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

            Debug.Log(enemyRange.GetComponent<Enemy_Controller>().m_enemyHealth);
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
