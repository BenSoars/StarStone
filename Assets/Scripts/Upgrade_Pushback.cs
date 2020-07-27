using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade_Pushback : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            Rigidbody player = other.gameObject.GetComponent<Rigidbody>();
            player.AddExplosionForce(1000, transform.position, 200);
            player.AddForce(transform.up * 350);
            player.velocity = Vector3.zero;
        }
    }
}
