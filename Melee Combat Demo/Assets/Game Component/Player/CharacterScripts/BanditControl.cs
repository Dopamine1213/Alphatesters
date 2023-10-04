using UnityEngine;
using System.Collections;

public class BanditControl : MonoBehaviour
{

    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_Bandit m_groundSensor;
    private bool m_grounded = false;
    private bool m_combatIdle = false;
    private bool m_isDead = false;

    [SerializeField] int maxHealth = 100;
    int currentHealth;
    public Healthbar healthbar;

    [SerializeField] float KBForce = 1.5f;
    float KBCounter;
    [SerializeField] float KBTotalTime = 0.2f;

    bool KnockFromRight;


    // Use this for initialization
    void Start()
    {
        currentHealth = maxHealth;
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
        healthbar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = 0f;

        // Swap direction of sprite depending on walk direction
        if (Input.GetKey(KeyCode.RightArrow)) 
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            inputX = 1.0f;
        }
        else if (Input.GetKey(KeyCode.LeftArrow)) 
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            inputX = -1.0f;
        }

        // Move
        if (KBCounter <= 0)
        {
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);
        }
        // Knockback
        else {
            if (KnockFromRight == true) {
                transform.position += new Vector3(-0.001f, 0.001f ,0f);
                m_body2d.velocity = new Vector2(-KBForce, KBForce * 0.5f);
            }
            if (KnockFromRight == false)
            {
                transform.position += new Vector3(0.010f, 0.001f, 0f);
                m_body2d.velocity = new Vector2(KBForce, KBForce * 0.5f);
            }
            KBCounter -= Time.deltaTime;
        }
        

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);

  
        // Recover
        if (Input.GetKeyDown("r"))
        {
            if (m_isDead)
            {
                m_animator.SetTrigger("Recover");
                currentHealth = 100;
            }

            m_isDead = !m_isDead;
        }

        //Attack
        else if (Input.GetKeyDown("a"))
        {
            m_animator.SetTrigger("Attack");
        }

        //Change between idle and combat idle
        else if (Input.GetKeyDown("f"))
            m_combatIdle = !m_combatIdle;

        //Jump
        else if (Input.GetKeyDown("space") && m_grounded)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        // -- Handle Animations --
        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon) 
            m_animator.SetInteger("AnimState", 2);

        //Combat Idle
        else if (m_combatIdle)
            m_animator.SetInteger("AnimState", 1);

        //Idle
        else
            m_animator.SetInteger("AnimState", 0);
    }


    // Get Damage
    private void OnCollisionEnter2D(Collision2D collision) {
        Vector2 repulsionDirection = (transform.position - collision.transform.position).normalized;


        if (collision.gameObject.CompareTag("Enemy")) {
            TakeDamage(20);
            

            // Knockback
            KBCounter = KBTotalTime;
            if (collision.transform.position.x <= transform.position.x) {
                KnockFromRight = true;
            }
            if (collision.transform.position.x > transform.position.x)
            {
                KnockFromRight = false;
            }

            
            if (repulsionDirection.y < 0.60f)
            {
                if (KnockFromRight == true)
                {
                    transform.position += new Vector3(1.5f, 0.1f, 0f);
                }
                else
                {
                    transform.position += new Vector3(-1.5f, 0.1f, 0f);
                }

            }
        }

        if (collision.gameObject.CompareTag("Obstacle")) {
            TakeDamage(30);
            }


        m_body2d.AddForce(repulsionDirection * KBForce, ForceMode2D.Impulse);
        healthbar.SetHealth(currentHealth);


    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        //Play hurt animation
        m_animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    //Death
    IEnumerator WaitForDeathEffect()
    {
        // Die animation
        yield return new WaitForSeconds(0.1f);
        m_animator.SetBool("Death", true);

    }
    void Die()
    {
        StartCoroutine(WaitForDeathEffect());
    }

    




}
