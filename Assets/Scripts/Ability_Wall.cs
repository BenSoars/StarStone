using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Wall : MonoBehaviour
{
    public GameObject m_wall;

    public Transform m_shotPoint;
    private RaycastHit m_hitscanCast;
    public Player_Controller r_playerController;

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
}
