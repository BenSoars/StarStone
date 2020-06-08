using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Generic : MonoBehaviour
{

    // Ben Soars
    [Header("Weapon Ammo")]
    [Space (2)]
    public int m_maxAmmo = 6; // the maximum ammo
    public int m_currentAmmo = 6; // the current ammo
    public int m_ammoPerShot = 1; // how much ammo shooting will consume

    [Space(2)]
    [Header("Weapon Accuracy")]
    [Space(2)]
    public bool m_isAccurate = true;
    public Vector3 m_accuracyMinRange;
    public Vector3 m_accuracyMaxRange;
    
    [Space(2)]
    [Header("Weapon Other Stats")]
    [Space(2)]
    public float m_shotForce = 100; // how much force the bullet will be shot for, it hitscan leave blank
    public int m_bulletDamage = 1;

    [Space(2)]
    [Header("Shot Type")]
    [Space(2)]
    public Rigidbody m_physicalBullet; // leave this blank if the gun is hitscan
    public Transform m_shotPoint; // the point where the bullet is shot from
    private Rigidbody m_shotBullet;

    private RaycastHit m_hitscanCast; // the hitscan raycast
    public GameObject hitSpark;

    void f_ShootGun()
    {
        for (int i = 0; i < m_ammoPerShot; i++)
        {
            if (m_physicalBullet) // if a physical bullet is there
            {
                m_shotBullet = Instantiate(m_physicalBullet, m_shotPoint.transform.position, Quaternion.identity) as Rigidbody; // shoot bullet
                m_shotBullet.AddForce(m_shotPoint.transform.forward * m_shotForce); // push bullet out

            }
            else
            {

                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out m_hitscanCast, Mathf.Infinity)) // shoot out a raycast for hitscan
                {
                    // if hit something
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * m_hitscanCast.distance, Color.yellow);
                    Debug.Log("Did Hit");
                    Instantiate(hitSpark, m_hitscanCast.point, Quaternion.identity);

                }
            }
        }

        m_currentAmmo -= m_ammoPerShot; // decrease total ammo counter by the ammo consuption of the gun
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (m_currentAmmo > 0)
            {
                f_ShootGun();
            }
        }
    }
}
