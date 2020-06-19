using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Text_HitDamage : MonoBehaviour
{
    private Player_Controller r_playerController;
    private Gun_Generic r_gunGeneric;
    private Gun_Prototype r_gunPrototype;

    private float m_bulletDamage;

    private bool m_textUpdated;
    // Start is called before the first frame update
    void Start()
    {
        r_playerController = FindObjectOfType<Player_Controller>();
        r_gunGeneric = FindObjectOfType<Gun_Generic>();
        r_gunPrototype = FindObjectOfType<Gun_Prototype>();       
    }

    // Update is called once per frame
    void Update()
    {     
        switch (r_gunPrototype.m_classState)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                m_bulletDamage = r_gunPrototype.m_laserDamage;
                break;
            default:
                m_bulletDamage = r_gunGeneric.m_bulletDamage;
                break;
        }  

        if (m_textUpdated == false)
        { 
            GetComponentInChildren<TextMeshPro>().text = "" + m_bulletDamage;
            m_textUpdated = true;
        }

        float m_distance = Vector3.Distance(transform.position, r_playerController.transform.position);

        if(m_distance < 1)
        {
            Destroy(gameObject);
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, r_playerController.m_playerRotX, transform.eulerAngles.z);
        transform.Translate(Vector3.up * (Time.deltaTime * 1.5f));
        transform.position = Vector3.MoveTowards(transform.position, r_playerController.transform.position, Time.deltaTime * 2);
    }
}
