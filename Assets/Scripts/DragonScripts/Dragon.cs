using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

public class Dragon : MonoBehaviour
{
    float speed = 50f, maxSpeed = 3, jumpPow = 400f, maxJump = 6, dashPow = 400f, strikePow = 400f;

    public bool grounded = true, faceRight = true, doubleJump = true, couch = false;

    public Rigidbody2D r2;
    public Animator anim;

    public Transform firePoint;
    public GameObject bulletPrefab;
    public Animator animator;

    public PlayerInput controls;

    float maxHealth = 100;
    float currentHealth;
    float regenHealth = 1f;
    float maxMana = 100;
    float currentMana;
    float regenMana = 1f;

    float attackMana = 5f;
    float strikeMana = 10f;
    float jumpAttackMana = 10f;
    float crouchAttackMana = 10f;
    float flyKickMana = 10f;

    public Collider2D idleCollider;
    public Collider2D couchCollider;
    public Collider2D flyKickCollider;

    public MultipleCamera Mcam;

    public LayerMask enemyLayers;

    public Transform FlyKickPoint;
    float flyKickRange = 0.3f;
    int flyKickDamage = 40;

    public Transform StrikePoint;
    public float strikeRange = 0.3f;
    int strikeDamage = 40;

    public GameObject Status;
    public HealthBarD healthBar;
    public ManaBarD manaBar;

    private void Awake()
    {
        firePoint = transform.Find("FirePoint");
        controls.Gameplay.AttackD.started += ctx => AttackD();
        controls.Gameplay.SkillD.started += ctx => SkillD();
        controls.Gameplay.JumpD.performed += ctx => JumpD();
        controls.Gameplay.CrouchD.performed += ctx => CrouchD();
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void AttackD()
    {
        if (grounded && !couch && currentMana >= attackMana)
        {
            anim.SetTrigger("Attack");
        }
        if (couch && currentMana >= crouchAttackMana)
        {
            anim.SetTrigger("CouchAttack");
        }
        if (!grounded && currentMana >= jumpAttackMana)
        {
            anim.SetTrigger("JumpAttack");
        }
    }
    void SkillD()
    {
        if (grounded && (Mathf.Abs(r2.velocity.x) > 3f) && currentMana >= strikeMana)
        {
            anim.SetTrigger("Strike");
        }
        if (!grounded && currentMana >= flyKickMana)
        {
            anim.SetTrigger("FlyKick");
        }
    }
    void JumpD()
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
    void CrouchD()
    {
        if (!couch && grounded)
        {
            couch = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        r2 = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();

        currentHealth = maxHealth;
        currentMana = maxMana;

        healthBar.SetMaxHealth(maxHealth);
        manaBar.SetMaxMana(maxMana);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        anim.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            anim.SetBool("isDead", true);
        }
    }
    public void TakeHP(int hp)
    {
        currentHealth = currentHealth + hp;

    }
    public void TakeMP(int mp)
    {
        currentMana = currentMana + mp;

    }
    void Hurt()
    {
        OnDisable();
    }
    void Die()
    {
        Debug.Log("enemy died");
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        this.enabled = false;

        Mcam.targets.RemoveAt(0);

        Destroy(gameObject);
    }

    // Update is called once per frame
    private void Update()
    {
        anim.SetBool("Grounded", grounded);
        anim.SetBool("Couch", couch);
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
        if (Input.GetKey(KeyCode.L))
        {
            couch = false;
            h = -1f;
        }
        if (Input.GetKey(KeyCode.Quote))
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
    void Run()
    {
    }
    void Idle()
    {
        OnEnable();
        idleCollider.enabled = true;
        couchCollider.enabled = false;
        flyKickCollider.enabled = false;
        regenMana = 1f;
        regenHealth = 1f;
    }
    void Couch()
    {
        couchCollider.enabled = true;
        idleCollider.enabled = false;
        flyKickCollider.enabled = false;
        regenMana = 3f;
        regenHealth = 3f;
    }
    void FlyKick()
    {
        currentMana -= flyKickMana;
        couchCollider.enabled = false;
        idleCollider.enabled = false;
        flyKickCollider.enabled = true;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(FlyKickPoint.position, flyKickRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.SendMessageUpwards("TakeDamage", flyKickDamage);
        }
    }
    //void OnDrawGizmosSelected()
    //{
    //    if (StrikePoint == null)
    //    {
    //        return;
    //    }
    //    Gizmos.DrawWireSphere(StrikePoint.position, strikeRange);
    //}
    void Attack()
    {
        currentMana -= attackMana;
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
    void JumpAttack()
    {
        currentMana -= jumpAttackMana;
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
    void CrouchAttack()
    {
        currentMana -= crouchAttackMana;
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
    void Jump()
    {
        couchCollider.enabled = false;
        idleCollider.enabled = true;
        flyKickCollider.enabled = false;
    }
    void Strike()
    {
        currentMana -= strikeMana;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(StrikePoint.position, strikeRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.SendMessageUpwards("TakeDamage", strikeDamage);
        }
    }

}
