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
        startAttack();
    }

    public void startAttack()
    {
        meleeHurtBox.enabled = true;
        Attacking = true;
    }

    public void stopAttack()
    {
        meleeHurtBox.enabled = false;
        Attacking = false;
    }
}
