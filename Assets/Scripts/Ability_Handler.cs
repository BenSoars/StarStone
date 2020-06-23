using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Handler : MonoBehaviour
{
    private Player_Controller r_playerController;

    //Ability Game Objects.
    public GameObject m_wall;
    public GameObject m_storm;

    public Transform m_shotPoint;
    private RaycastHit m_hitscanCast;

    public GameObject m_knife;
    public int m_totalKnives;

    private void Start()
    {
        r_playerController = GameObject.FindObjectOfType<Player_Controller>();
    }

    public void f_spawnWall()
    {
        if (Physics.Raycast(m_shotPoint.position, m_shotPoint.forward, out m_hitscanCast, Mathf.Infinity)) //Creates a Raycast.
        {
            GameObject o_wall = Instantiate(m_wall, new Vector3(m_hitscanCast.point.x, m_hitscanCast.point.y - 2, m_hitscanCast.point.z), Quaternion.LookRotation(Vector3.forward));
            o_wall.transform.eulerAngles = new Vector3(o_wall.transform.eulerAngles.x, r_playerController.m_playerRotX, o_wall.transform.eulerAngles.z);
        }
    }

    public void f_spawnStorm()
    {
        if (Physics.Raycast(m_shotPoint.position, m_shotPoint.forward, out m_hitscanCast, Mathf.Infinity)) //Creates a Raycast.
        {
            GameObject o_storm = Instantiate(m_storm, new Vector3(m_hitscanCast.point.x, m_hitscanCast.point.y - 2, m_hitscanCast.point.z), Quaternion.LookRotation(Vector3.forward));
        }
    }

    public void f_spawnKnives()
    {
        Debug.Log("Shot Point: " + m_hitscanCast.point);

        float m_sideDirection = -40;

        for (int i = 0; i < m_totalKnives; i++)
        {
            GameObject knife = Instantiate(m_knife, m_shotPoint.position, Quaternion.identity);
            Rigidbody m_krb = knife.GetComponent<Rigidbody>();

            m_krb.AddForce(m_shotPoint.forward * 100);
            m_krb.AddForce(m_shotPoint.right * m_sideDirection);

            m_sideDirection += 10;

        }
    }
}
