using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Infector : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {            
            other.gameObject.GetComponent<Enemy_Controller>().m_isEnemyInfected = true;
            Destroy(gameObject);
        }
    }
}
