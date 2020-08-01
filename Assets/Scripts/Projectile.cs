using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    // Ben Soars

    [Tooltip("The damage the projectile deals, is passed from either the weapon script or enemy script")]
    public float m_damage; // damage to deal on contact
    [Tooltip("The hurtbox for the weapon, is disabled on hit to resolve issues where it may hit multiple times.")]
    public Collider m_hurtBox; // the hurtbox
    [Tooltip("The rigidbody")]
    public Rigidbody m_rb; // the rigidbodys
    [Tooltip("Trail effect for the projectile, if it has one")]
    public TrailRenderer m_trail;

    [Tooltip("Enable for a projectile that damages the player")]
    public bool m_enemy; // if it's an enemy projectile

    [Tooltip("Enable this if you want the projectile to stick to enemies and walls when it collides with them")]
    public bool m_sticky; // will stick to objects it hits
    [Tooltip("Enable this if you want the projectile to rotate in the direction it's traveling in")]
    public bool m_faceDirectionOfTravel; // 

    public GameObject m_hitDamageText; 
    private AchievementSpecialConditions m_SpecialTracker;

    void Start()
    {
        m_SpecialTracker = GameObject.FindObjectOfType<AchievementSpecialConditions>();
    }

    void Update()
    {
        if (m_faceDirectionOfTravel == true && m_rb) // if is set to rotate to face the direction it's facing and has a rigidbody
        {
            transform.rotation = Quaternion.LookRotation(m_rb.velocity); // rotate to velocity direction
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && m_enemy == false) // if it's not an enemy projectile and collides with an enemy
        {
            other.GetComponent<Enemy_Controller>().m_enemyHealth -= m_damage; // deal damage to the enemy
            GameObject m_textObject = Instantiate(m_hitDamageText, other.transform.position, Quaternion.identity); // create a damage text number
            m_textObject.GetComponentInChildren<TextMeshPro>().text = "" + m_damage; // display the damage dealt to the text
            stickyProjectile(other); // run the sticky projectile test
           
        } else if (other.gameObject.CompareTag ("Player") && m_enemy == true) // if it's an enemy projectile
        {
            Player_Controller player = other.GetComponent<Player_Controller>(); // get the player component
            m_SpecialTracker.imperfectRun();
            player.playerHealth -= m_damage; // take damage from projecile
            player.audio.playPlayerHurt(); // play player hurt sound effect
            stickyProjectile(other); // run the sticky projectile test
        }

        //else if (m_sticky && !other.gameObject.CompareTag("Player")) // if it's anything else, other than the player
        //{
        //    stickyProjectile(other); // run the sticky projectile test
        //}
    }

    void stickyProjectile(Collider col)
    {
        if (m_sticky) // if the projectile is set to be sticky
        {
            Destroy(m_hurtBox); // destroy the hurtbox so it can't damage any more enemies
            Destroy(m_rb); // destroy the rigidbody so it won't fall while stick
            Destroy(m_trail); // destroy the trail to prevent visual glitches
            transform.SetParent(col.gameObject.transform); // set the prijectile to be parented to the passed object, in this case it's always the object it hits
        } else
        {
            Destroy(gameObject); // else just destroy the object on hit
        }
    }
}
