using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D playersCollider;
    private SpriteRenderer sprite;
    private Animator anim;
    private float dirX = 0f;
    [SerializeField]
    private LayerMask ground;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 12f;


    private bool canDoubleJump = false;
    private int jumpCount = 0;
    private DateTime lastShootTime = DateTime.MinValue;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private long shootDelay = 200; // ms
    private short direction = 1; // right
    private float maxHealth = 100f;
    private float currentHealth;

    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private float shootDamage = 10f;
    private enum MovementState
    {
        idle, running, jumping, falling, doubleJump
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        playersCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && IsJumpable()) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;
        }

        UpdateAnimationState();
        
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
        MovementState state;
        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }
        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }else if(rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }
        if (jumpCount == 2)
        {
            state = MovementState.doubleJump;
            jumpCount = 0;
        }

        anim.SetInteger("state", (int)state);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            // knock back, follow bullet's direction
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            rb.AddForce(bullet.direction * bullet.speed / 2 * Vector2.right, ForceMode2D.Impulse);

            TakeDamage();
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void TakeDamage()
    {
        currentHealth -= shootDamage;
        healthBar.SetHealth(currentHealth);
    }
    private void Die()
    {
        anim.SetTrigger("death");
        rb.bodyType = RigidbodyType2D.Static;
    }
}

