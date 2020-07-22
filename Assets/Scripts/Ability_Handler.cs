﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kurtis Watson
public class Ability_Handler : MonoBehaviour
{
    private Player_Controller playerController;
    private Prototype_Classes prototypeClasses;

    public int m_totalKnives;

    public Transform m_shotPoint; //Where bullets are shot from.

    private RaycastHit m_hitscanCast; //The raycast that determines the direction of the bullet.

    public GameObject pushBack;
    public GameObject invisibility;
    public GameObject m_wall; //Ability Game Objects.
    public GameObject m_storm;
    public GameObject m_knife;
    public GameObject m_Tornado;
    public GameObject m_Infector;

    public GameObject weapons;
    public GameObject hands;

    private Animator m_handsAnim;
    public string[] animationType;

    private void Start()
    {
        playerController = GameObject.FindObjectOfType<Player_Controller>();
        prototypeClasses = FindObjectOfType<Prototype_Classes>();

        m_handsAnim = hands.GetComponent<Animator>();
    }

    public void Update()
    {
        if (prototypeClasses.abilityState == true)
        {
            hands.active = true;
            weapons.active = false;
        }
        else
        {
            hands.active = false;
            weapons.active = true;
            prototypeClasses.stateQ = false;
            prototypeClasses.stateV = false;
        }
    }

    public void f_spawnPush()
    {
        Instantiate(pushBack, m_shotPoint.transform.position, m_shotPoint.rotation);
    }

    public void f_spawnInvisibilty()
    {
        m_handsAnim.SetBool(animationType[0], true);
        playerController.isPlayerInvisible = true;
        invisibility.active = true;
        Invoke("reset", 10);
    }

    public void f_spawnWall()
    {
        if (Physics.Raycast(m_shotPoint.position, m_shotPoint.forward, out m_hitscanCast, Mathf.Infinity)) //Creates a Raycast in direction player is looking.
        {
            GameObject o_wall = Instantiate(m_wall, new Vector3(m_hitscanCast.point.x, m_hitscanCast.point.y - 2, m_hitscanCast.point.z), Quaternion.LookRotation(Vector3.forward)); //Instantiate a wall that summons at the position of the players crosshair location.
            o_wall.transform.eulerAngles = new Vector3(o_wall.transform.eulerAngles.x, playerController.playerRotX, o_wall.transform.eulerAngles.z); //Rotate the wall based on angle of player.
        }
    }

    public void f_spawnStorm()
    {
        if (Physics.Raycast(m_shotPoint.position, m_shotPoint.forward, out m_hitscanCast, Mathf.Infinity)) //Creates a Raycast.
        {
            Instantiate(m_storm, new Vector3(m_hitscanCast.point.x, m_hitscanCast.point.y - 2, m_hitscanCast.point.z), Quaternion.LookRotation(Vector3.forward));
        }
    }

    public void f_spawnKnives()
    {
        float m_sideDirection = -40; //Angle of first knife.

        for (int i = 0; i < m_totalKnives; i++) //Instantiate a set amount of knives.
        {
            GameObject knife = Instantiate(m_knife, m_shotPoint.position, Quaternion.identity); 
            Rigidbody m_krb = knife.GetComponent<Rigidbody>(); //Access that specific knife RigidBody and >

            m_krb.AddForce(m_shotPoint.forward * 100); 
            m_krb.AddForce(m_shotPoint.right * m_sideDirection); 

            m_sideDirection += 10; //> change the RigidBody force from the right direction so it 'spreads' correctly.

        }
    }

    public void f_spawnTornado()
    {
        if (Physics.Raycast(m_shotPoint.position, m_shotPoint.forward, out m_hitscanCast, Mathf.Infinity)) //Creates a Raycast.
        {
            Instantiate(m_Tornado, new Vector3(m_hitscanCast.point.x, m_hitscanCast.point.y - 2, m_hitscanCast.point.z), Quaternion.LookRotation(Vector3.forward)); //Spawns a tornade of position of player crosshair.
        }
    }

    public void f_spawnInfector()
    {
        if (Physics.Raycast(m_shotPoint.position, m_shotPoint.forward, out m_hitscanCast, Mathf.Infinity)) //Creates a Raycast.
        {
            Instantiate(m_Infector, new Vector3(m_hitscanCast.point.x, m_hitscanCast.point.y - 2, m_hitscanCast.point.z), Quaternion.LookRotation(Vector3.forward)); //Spawns a tornade of position of player crosshair.
        }
    }

    private void reset()
    {
        playerController.isPlayerInvisible = false;
        invisibility.active = false;
        StartCoroutine(f_resetAnimations());
    }


    IEnumerator f_resetAnimations()
    {
        for (int i = 0; i < animationType.Length; i++)
        {
            m_handsAnim.SetBool(animationType[i], false);
        }
        yield return new WaitForSeconds(0.1f);
        prototypeClasses.stateV = false;
        prototypeClasses.stateQ = false;
        prototypeClasses.abilityState = !prototypeClasses.abilityState;
    }
}
