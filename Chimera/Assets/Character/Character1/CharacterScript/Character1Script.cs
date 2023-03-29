using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character1Script : MonoBehaviour
{
    [SerializeField] private float moveForce = 4.5f;
    [SerializeField] private float jumpForce = 7.5f;
    [SerializeField] private Rigidbody2D mybody;


    private float movementX;
    private SpriteRenderer sr;
    private Animator anim;
    private string RUN_ANIMATION = "IdleToRun";
    private string JUMP_ANIMATION = "RunToJump";
    private string GROUND_TAG = "Ground";
    private bool isGrounded = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PlayerMoveKeyboard();
        AnimatePlayer();
        if (Input.GetKeyDown(KeyCode.A)) 
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        PlayerJump();
    }

    void PlayerMoveKeyboard()
    {
        movementX = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(movementX, 0f, 0f) * Time.deltaTime * moveForce;
        
     }

    void AnimatePlayer() 
    {
        if (movementX > 0)
        {
            anim.SetBool(RUN_ANIMATION, true);
            sr.flipX = false;
        }
        else if (movementX < 0)
        {
            anim.SetBool(RUN_ANIMATION, true);
            sr.flipX = true;
        }
        else {
            anim.SetBool(RUN_ANIMATION, false);
        }
            
    }

    void PlayerJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded){
            isGrounded = false;
            mybody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            anim.SetBool(JUMP_ANIMATION, true);
        }
        else {
            anim.SetBool(JUMP_ANIMATION, false);
        }
    }

    void Attack() 
    {
        // Play an attack animation
        anim.SetTrigger("Attack");
        // Detect enemies in range of attack
        // Damage them


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GROUND_TAG)) { 
            isGrounded=true;
        }
    }
}
