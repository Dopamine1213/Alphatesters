using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.A))
        {
            Attack();
        }
        */
    }

    void Attack() 
    {

        //Play an attack animation
        animator.SetTrigger("Attack");


        //Detect enemies in range of attack

        //Damage them
    }
}
