using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackPoint : MonoBehaviour
{
    public Transform enemy;

    // Start is called before the first frame update
    void Start()
    {
        /* Can do this way, but prefer Transform
        GameObject player = GameObject.Find("LightBandit");
        */

        Vector3 enemyPosition = enemy.transform.position;
        transform.position = enemyPosition;
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 offset = new Vector3(1, 0, 0);

        // Swap direction of sprite depending on walk direction

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 chaseDirection = (player.transform.position - transform.position).normalized;

        // move
        if (chaseDirection.x > 0)
        {
            transform.position = player.transform.position - offset;
        }
        else 
        {
            transform.position = player.transform.position + offset;
        }

    }


}
