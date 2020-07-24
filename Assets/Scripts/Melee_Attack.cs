using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_Attack : MonoBehaviour
{
    // Ben Soars

    public Collider meleeHurtBox; // melee attack hurtbox
    public bool Attacking; // if the player is attacking
    
    // Start is called before the first frame update
    void Start()
    {
        startAttack(); // start the start attack code, to check
    }

    public void startAttack()
    {
        meleeHurtBox.enabled = true; // enable the hurtbox
        Attacking = true; // set the attacking bool to true
    }

    public void stopAttack()
    {
        meleeHurtBox.enabled = false; // disable the hurtbox
        Attacking = false; // set the attacking bool to false
    }
}
