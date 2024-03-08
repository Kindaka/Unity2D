using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D playersCollider;
    private SpriteRenderer sprite;
    private Animator animator;

    [SerializeField]
    private LayerMask ground;

    private short direction = 1; // right
    private DateTime lastShootTime = DateTime.MinValue;
    private float inputHorizontal = 0f;

    [SerializeField]
    private float speed = 30f;
    private const float fallMultiplier = 10f; // Set your desired fall multiplier


    [SerializeField]
    private float jumpForce = 6f;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private long shootDelay = 200; // ms

    private bool canDoubleJump = false;
    private int jumpCount = 0;

    private enum AnimationState
    { idle, running, jumping, falling, doubleJumping }

    private AnimationState state;

    private float maxHealth = 100f;
    private float currentHealth;

    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private float shootDamage = 10f;
    [SerializeField]
    private float trapDamage = 20f;
    [SerializeField]
    private float hell = 100f;
    [SerializeField]
    private float healing = 10f;
    [SerializeField]
    private GameObject explosionPrefab;

    public delegate void OnDestroyedHandler(GameObject gameObject);
    public event OnDestroyedHandler OnDestroyed;
    // Start is called before the first frame update
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playersCollider = GetComponent<BoxCollider2D>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
        Jump();
        Shoot();
        UpdateAnimationState();
        //CheckFallDamage();
    }

    private void Move()
    {
        string axisName = gameObject.name == "Player1" ? "Horizontal1" : "Horizontal2";
        inputHorizontal = Input.GetAxisRaw(axisName);

        float moveSpeed = inputHorizontal * speed;

        // Check if the character is falling or double jumping
        if (state == AnimationState.falling || state == AnimationState.doubleJumping)
        {
            // Apply additional downward force to make the character fall faster
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - fallMultiplier * Time.deltaTime);
        }

        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

        if (inputHorizontal > 0f)
        {
            sprite.flipX = false;
            direction = 1;
        }
        else if (inputHorizontal < 0f)
        {
            sprite.flipX = true;
            direction = -1;
        }
    }

    private void Jump()
    {
        string buttonName = gameObject.name == "Player1" ? "Jump1" : "Jump2";
        if (Input.GetButtonDown(buttonName) && IsJumpable())
        {
            Vector3 velocity = rb.velocity;
            velocity.y = 0f;
            rb.velocity = velocity;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;
        }
    }

    public void Shoot()
    {
        string buttonName = gameObject.name == "Player1" ? "Fire1" : "Fire2";
        DateTime shootTime = DateTime.Now;
        double deltaTime = (shootTime - lastShootTime).TotalMilliseconds;

        if (Input.GetAxisRaw(buttonName) > 0 && deltaTime >= shootDelay)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.GetComponent<Bullet>().direction = direction;

            // calculate x base on direction
            float x = direction > 0 ? playersCollider.bounds.max.x : playersCollider.bounds.min.x;
            float y = playersCollider.bounds.center.y;
            bullet.transform.position = new Vector3(x, y, 0);
            lastShootTime = shootTime;
        }
    }

    private void UpdateAnimationState()
    {
        if (inputHorizontal != 0f)
        {
            state = AnimationState.running;
        }
        else
        {
            state = AnimationState.idle;
        }
        if (rb.velocity.y > .1f)
        {
            state = AnimationState.jumping;
        }
        else if (rb.velocity.y < -.5f)
        {
            state = AnimationState.falling;
        }
        if (jumpCount == 2)
        {
            state = AnimationState.doubleJumping;
            jumpCount = 0;
        }
        animator.SetInteger("state", (int)state);
    }

    private bool IsJumpable()
    {
        bool isJumpable = false;
        bool isGrounded = Physics2D.BoxCast(playersCollider.bounds.center, playersCollider.bounds.size, 0f, Vector2.down, .1f, ground);
        if (isGrounded || canDoubleJump)
        {
            isJumpable = true;
            canDoubleJump = !canDoubleJump;
        }
        return isJumpable;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            // knock back, follow bullet's direction
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            rb.AddForce(bullet.direction * bullet.speed / 2 * Vector2.right, ForceMode2D.Impulse);

            TakeDamage(shootDamage);
            if(currentHealth<= 0)
            {
                Die();
            }
        }
        if (collision.gameObject.CompareTag("Trap"))
        {
            TakeDamage(trapDamage);
            if (currentHealth <= 0)
            {
                Die();
            }
        }
        if (collision.gameObject.CompareTag("hell"))
        {
            Die();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cherry"))
        {

            Healing(healing);
            if (currentHealth <= 0)
            {
                Die();
            }
        }
        if (collision.gameObject.CompareTag("Banana"))
        {
            if (currentHealth < maxHealth)
            {
                currentHealth = maxHealth;
                healthBar.SetHealth(currentHealth);
            }
            if (currentHealth <= 0)
            {
                Die();
            }

        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
    public void Healing(float healing)
    {

        if(currentHealth < maxHealth)
        {
            currentHealth += healing;
            healthBar.SetHealth(currentHealth);
        }
    }
    //private void CheckFallDamage()
    //{
    //    if (transform.position.y < -10f)
    //    {
    //        TakeDamage(30f); 
    //        if (currentHealth <= 0)
    //        {
    //            Die(); 
    //        }
            
    //    }
    //}
    private void Die()
    {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            if (OnDestroyed != null)
            {
                OnDestroyed(gameObject);
            }
    }
}