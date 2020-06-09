using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Pushback : MonoBehaviour
{
    public float m_abilitySpeed;
    public float m_upliftForce;
    public float m_knockbackForce;

    private SphereCollider m_sphereCollider;
    public float m_damageRadius;

    public float m_abilityDuration;

    public bool m_isFired;

    private Rigidbody m_rb;

    private Animator m_animator;
    public void Start()
    {
        m_sphereCollider = gameObject.GetComponent<SphereCollider>();

        m_isFired = true;

        m_animator = gameObject.GetComponent<Animator>();

        m_rb = GetComponent<Rigidbody>();

        m_rb.AddForce(transform.forward * m_abilitySpeed);
    }

    public void Update()
    {
        m_damageRadius = m_sphereCollider.radius;

        m_animator.SetBool("isFired", m_isFired);

        Collider[] o_colliders = Physics.OverlapSphere(transform.position, m_damageRadius); //o for objects.
        foreach (Collider o_hit in o_colliders)
        {
            Rigidbody o_rb = o_hit.GetComponent<Rigidbody>();

            if(o_rb != null && o_hit.gameObject.layer != 9 && o_hit.gameObject != this.gameObject)
            {
                o_rb.AddExplosionForce(m_knockbackForce, transform.position, m_damageRadius, m_upliftForce);
            }
        }

        Invoke("f_removeObject", 1f);
    }

    void f_removeObject()
    {
        GameObject.Destroy(this.gameObject);
    }
}
