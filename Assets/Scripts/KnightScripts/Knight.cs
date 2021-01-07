using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

public class Knight : MonoBehaviour
{
    float speed = 50f, maxSpeed = 3, jumpPow = 400f, maxJump = 6, dashPow = 400f, strikePow = 400f;

    int shield = 10;

    public bool grounded = true, faceRight = true, doubleJump = true, couch = false, cast = false, block = false;

    public Rigidbody2D r2;
    public Animator anim;

    public LayerMask enemyLayers;

    public Transform attackPoint;
    float attackRange = 0.5f;
    int attackDamage = 20;


    public Transform strikePoint;
    float strikeRange = 0.5f;
    int strikeDamage = 20;

    float maxHealth = 100;
    float currentHealth;
    float maxMana = 100;
    float currentMana;
    float regenMana = 1f;
    float regenHealth = 1f;


    public MultipleCamera Mcam;

    public Collider2D idleCollider;
    public Collider2D couchCollider;

    public PlayerInput controls;

    float normalGravity;

    public GameObject Status;
    public HealthBar healthBar;
    public ManaBar manaBar;

    float attackMana = 5f;
    float strikeMana = 10f;
    float blockMana = 5f;
    float dashMana = 10f;
    float castMana = 10f;
    private void Awake()
    {
        controls.Gameplay.Attack.started += ctx => Attack2();
        controls.Gameplay.Skill.started += ctx => Skill2();
        controls.Gameplay.Jump.performed += ctx => Jump();
        controls.Gameplay.Crouch.performed += ctx => Crouch();
        controls.Gameplay.Dodge.performed += ctx => Dodge();
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    void Attack2()
    {
        if (currentMana >= attackMana)
        {
            anim.SetTrigger("Attack");
        }

    }
    void Skill2()
    {
        if ((Mathf.Abs(r2.velocity.x) > 3f) && currentMana >= strikeMana && grounded)
        {
            anim.SetTrigger("Strike");
        }
        if (!grounded && cast && currentMana >= castMana)
        {
            anim.SetTrigger("Cast");
        }

    }
    void Jump()
    {
        couch = false;
        if (grounded)
        {
            grounded = false;
            doubleJump = true;
            r2.AddForce(Vector2.up * jumpPow);
        }
        else
        {
            if (doubleJump)
            {
                doubleJump = false;
                r2.velocity = new Vector2(r2.velocity.x, 0);
                r2.AddForce(Vector2.up * jumpPow * 0.7f);

            }
        }
    }
    void Crouch()
    {
        if (!couch && grounded)
        {
            couch = true;
        }
    }
    void Dodge()
    {
        if ((Mathf.Abs(r2.velocity.x) > 3f) && currentMana >= dashMana && grounded)
        {
            anim.SetTrigger("Dash");
        }
        if ((Mathf.Abs(r2.velocity.x) < 3f) && grounded && !couch && currentMana >= blockMana)
        {
            if (!block)
            {
                block = true;
                shield = 39;
                maxSpeed = 0f;
            }
            else
            {
                block = false;
                shield = 10;
                maxSpeed = 3f;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        r2 = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();

        currentHealth = maxHealth;
        currentMana = maxMana;

        normalGravity = r2.gravityScale;

        healthBar.SetMaxHealth(maxHealth);
        manaBar.SetMaxMana(maxMana);
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("Grounded", grounded);
        anim.SetBool("Couch", couch);
        anim.SetBool("Block", block);
        anim.SetFloat("Speed", Mathf.Abs(r2.velocity.x));
        if (currentMana < maxMana)
        {
            currentMana += regenMana * Time.deltaTime;
        }
        if (currentHealth < maxHealth)
        {
            currentHealth += regenHealth * Time.deltaTime;
        }
        manaBar.SetMana(currentMana);
        healthBar.SetHealth(currentHealth);
    }
    private void FixedUpdate()
    {

        float h = 0;
        if (Input.GetKey(KeyCode.F))
        {
            couch = false;
            h = -1f;
        }
        if (Input.GetKey(KeyCode.H))
        {
            couch = false;
            h = 1f;
        }
        r2.AddForce((Vector2.right) * speed * h);

        if (r2.velocity.x > maxSpeed)
        {
            r2.velocity = new Vector2(maxSpeed, r2.velocity.y);
        }
        if (r2.velocity.x < -maxSpeed)
        {
            r2.velocity = new Vector2(-maxSpeed, r2.velocity.y);
        }

        if (r2.velocity.y > maxJump)
        {
            r2.velocity = new Vector2(r2.velocity.x, maxJump);
        }
        if (r2.velocity.y < -maxJump)
        {
            r2.velocity = new Vector2(r2.velocity.x, -maxJump);
        }

        if (h > 0 && !faceRight)
        {
            Flip();
        }
        if (h < 0 && faceRight)
        {
            Flip();
        }
    }
    public void Flip()
    {
        faceRight = !faceRight;
        transform.Rotate(0f, 180f, 0f);
    }

    void Idle()
    {
        OnEnable();
        shield = 10;
        idleCollider.enabled = true;
        couchCollider.enabled = false;
        cast = true;
        regenMana = 1f;
        regenHealth = 1f;
    }
    void Couch()
    {
        couchCollider.enabled = true;
        idleCollider.enabled = false;
        regenMana = 3f;
        regenHealth = 3f;
    }
    void Attack()
    {
        currentMana -= attackMana;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.SendMessageUpwards("TakeDamage", attackDamage);
        }
    }

    //void OnDrawGizmosSelected()
    //{
    //    if (blockPoint == null)
    //    {
    //        return;
    //    }
    //    Gizmos.DrawWireSphere(blockPoint.position, blockRange);
    //}

    void Cast()
    {
        currentMana -= castMana;
        r2.AddForce((Vector2.up) * jumpPow);
        cast = false;
    }
    IEnumerator Strike()
    {
        currentMana -= strikeMana;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(strikePoint.position, strikeRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.SendMessageUpwards("TakeDamage", strikeDamage);
        }

        int h = 0;
        if (faceRight)
        {
            h = 1;
        }
        else h = -1;
        r2.AddForce((Vector2.right) * h * strikePow);
        r2.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.1f);
        r2.velocity = Vector2.zero;

    }
    void Block()
    {
        currentMana -= blockMana;
        if (currentMana < blockMana)
        {
            block = false;
        }
    }
    IEnumerator Dash()
    {
        currentMana -= dashMana;
        Vector2 originalVelocity = r2.velocity;
        idleCollider.enabled = false;
        couchCollider.enabled = false;
        couch = false;

        int h = 0;
        if (faceRight)
        {
            h = 1;
        }
        else h = -1;
        r2.AddForce((Vector2.right) * h * dashPow);
        r2.gravityScale = 0;
        r2.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        r2.velocity = originalVelocity;
        r2.gravityScale = normalGravity;
    }
    void Hurt()
    {
        OnDisable();
    }

    public void TakeDamage(int damage)
    {
        int damageReceived = damage - shield;
        currentHealth = currentHealth - damageReceived;
        if (damageReceived >= 5)
        {
            anim.SetTrigger("Hurt");
        }
        if (currentHealth <= 0)
        {
            anim.SetBool("isDead", true);
        }
    }
    public void TakeHP(int hp)
    {
        currentHealth = currentHealth + hp;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    public void TakeMP(int mp)
    {
        currentMana = currentMana + mp;
        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
    }
    void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        this.enabled = false;

        Mcam.targets.RemoveAt(1);
        Status.SetActive(false);
        Destroy(gameObject);
    }
}
