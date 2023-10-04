using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        /* Can do this way, but prefer Transform
        GameObject player = GameObject.Find("LightBandit");
        */

        Vector3 playerPosition = player.transform.position;
        transform.position = playerPosition;
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 offset = new Vector3(1, 0, 0);

        // Swap direction of sprite depending on walk direction
        if (Input.GetKey(KeyCode.RightArrow)) 
        {
            transform.position = player.position + offset;
        }
        else if (Input.GetKey(KeyCode.LeftArrow)) 
        {
            transform.position = player.position - offset;
        }

    }


}
