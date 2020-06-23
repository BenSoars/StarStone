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
        if (Physics.Raycast(m_shotPoint.position, m_shotPoint.forward, out m_hitscanCast, Mathf.Infinity)) //Creates a Raycast.
        {
            Debug.Log("Shot Point: " + m_hitscanCast.point);
            for (int i = 0; i < m_totalKnives; i++)
            {
                GameObject knife = Instantiate(m_knife, m_shotPoint.position, Quaternion.identity);

                float xValue = Random.Range(-0.3f, 0.3f);
                Vector3 accuracy = new Vector3(m_shotPoint.forward.x - xValue, m_shotPoint.forward.y, m_shotPoint.forward.z);

                knife.GetComponent<Rigidbody>().AddForce(accuracy * 100);
            }
        }
    }
}
