﻿using UnityEngine;
using System.Collections;

public class Bandit : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;

    [SerializeField] Animator   m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_Bandit       m_groundSensor;
    private bool                m_grounded = true;
    private bool                m_jumping = false;
    private bool                m_combatIdle = false;
    private bool                m_isDead = false;

    // Use this for initialization
    void Start () {
        m_body2d = GetComponent<Rigidbody2D>();
        /*
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
        */
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            m_grounded = true;
            m_jumping = false;
            m_animator.SetBool("Grounded", m_grounded);
        }
    }
    // Update is called once per frame
    void Update () {

        if (m_grounded)
        {
            Debug.Log("m_ground is true");
        }
        else {
            Debug.Log("m_ground is false");
        }
        /*
        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State()) {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if(m_grounded && !m_groundSensor.State()) {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }
        */


        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");
        moveX();
        //Run
            if (inputX != 0)// Mathf.Abs(inputX) > Mathf.Epsilon
            m_animator.SetInteger("AnimState", 2);
        //Combat Idle
        else if (m_combatIdle)
            m_animator.SetInteger("AnimState", 1);
        //Idle
        else
            m_animator.SetInteger("AnimState", 0);

        // -- Handle Animations --
        //Death
        if (Input.GetKeyDown("e")) {
            if(!m_isDead)
                m_animator.SetTrigger("Death");
            else
                m_animator.SetTrigger("Recover");

            m_isDead = !m_isDead;
        }
            
        //Hurt
        else if (Input.GetKeyDown("q"))
            m_animator.SetTrigger("Hurt");

        //Attack
        else if(Input.GetKeyDown("a")) {
            Attack();
        }

        //Change between idle and combat idle
        else if (Input.GetKeyDown("f"))
            m_combatIdle = !m_combatIdle;

        //Jump
        else if (Input.GetKeyDown("space") && m_grounded && !m_jumping) {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_jumping = true;
            m_animator.SetBool("Grounded",m_grounded);
            /*
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
            */
            m_body2d.AddForce(new Vector2(0f, m_jumpForce), ForceMode2D.Impulse);
        }
        
    }
    

    void Attack()
    {

        //Play an attack animation
        m_animator.SetTrigger("Attack");
        m_animator.SetInteger("AnimState", 1);


        //Detect enemies in range of attack

        //Damage them
    }

    void moveX() 
    {
        float inputX = Input.GetAxis("Horizontal");
        // Move - m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);
        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
            transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);
        else if (inputX < 0)
            transform.localScale = new Vector3(-2.0f, 2.0f, 1.0f);
        transform.position += new Vector3(inputX, 0f, 0f) * Time.deltaTime * m_speed;
        m_animator.SetInteger("AnimState", 2);
        //Set AirSpeed in animator - m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);

    }



}
