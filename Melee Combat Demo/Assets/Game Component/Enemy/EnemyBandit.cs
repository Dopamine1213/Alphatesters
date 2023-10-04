using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBandit : MonoBehaviour
{

    public Animator animator;
    private Rigidbody2D rb;

    public int maxHealth = 100;
    int currentHealth;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerLayers;
    public int attackDamage = 30;
    public float attackRate = 0.5f;
    float nextAttackTime = 0f;

    [HideInInspector]
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = 0.5f;
        currentHealth = maxHealth;
    }

    void FixedUpdate() {

        // follow player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 chaseDirection = (player.transform.position - transform.position).normalized;

        // move
        if (chaseDirection.x > 0) {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        rb.velocity = new Vector2(chaseDirection.x * speed, rb.velocity.y);

        // attack
        if (Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }

        // -- Handle Animation --
        animator.SetInteger("AnimState", 0);

    }

    public void TakeDamage(int damage) 
    {
        currentHealth -= damage;
        //Play hurt animation
        StartCoroutine(WaitForHurtAnimation());
        if (currentHealth <= 0) 
        {
            Die();
        }
    }

    IEnumerator WaitForHurtAnimation() {
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("Hurt");
    }

    IEnumerator WaitForDeathEffect() {
        // Die animation
        yield return new WaitForSeconds(0.6f);
        animator.SetBool("Death", true);

        // Disable
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }


    void Die() {
        speed = 0;
        StartCoroutine(WaitForDeathEffect());
    }

    void Attack() {

        // Animation
        animator.SetTrigger("Attack");

        //Delay
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack() {
        yield return new WaitForSeconds(0.5f);
        // Detect
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);

        // Damage
        foreach (Collider2D collision in hitPlayer)
        {
            if (collision.CompareTag("Player"))
            {
                BanditControl playerScript = collision.gameObject.GetComponent<BanditControl>();
                playerScript.TakeDamage(attackDamage);
            }
        }
    }


}
