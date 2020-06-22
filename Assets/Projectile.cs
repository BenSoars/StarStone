using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{

    public float m_damage; // damage to deal on contact
    public Collider m_hurtBox; // the hurtbox
    public Rigidbody m_rb; // the rigidbodys
    public TrailRenderer m_trail;

    public bool m_enemy; // if it's an enemy projectile

    public bool m_sticky; // will stick to objects it hits
    public bool m_faceDirectionOfTravel;

    public GameObject m_hitDamageText;

    void Update()
    {
        if (m_faceDirectionOfTravel == true)
        {
            transform.rotation = Quaternion.LookRotation(m_rb.velocity);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy_Controller>().m_enemyHealth -= m_damage;
            GameObject m_textObject = Instantiate(m_hitDamageText, other.transform.position, Quaternion.identity);
            m_textObject.GetComponentInChildren<TextMeshPro>().text = "" + m_damage;
            if (m_sticky)
            {
                Destroy(m_hurtBox);
                Destroy(m_rb);
                Destroy(m_trail);
                transform.SetParent(other.gameObject.transform);
            }
        }
    }
}
