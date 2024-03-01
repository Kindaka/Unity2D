using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dichuyen : MonoBehaviour
{
    private int moveDirection;
    private float moveSpeed = 10f;
    private int jumpForce = 10;
    private bool isFacingRight = true;
    private bool isGrounded;
    private bool canDoubleJump = true;
    private Rigidbody2D rb;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovementInput();
        HandleJumpInput();
        FlipCharacter();
        UpdateAnimation();
    }

    void HandleMovementInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        moveDirection = (int)Mathf.Sign(horizontalInput);
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
    }

    void HandleJumpInput()
    {
        isGrounded = IsGrounded();

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (isGrounded || canDoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                if (!isGrounded)
                {
                    canDoubleJump = false;
                }
            }
        }
    }

    void FlipCharacter()
    {
        if ((isFacingRight && moveDirection < 0) || (!isFacingRight && moveDirection > 0))
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    void UpdateAnimation()
    {
        anim.SetFloat("run", Mathf.Abs(moveDirection));
        anim.SetBool("isGrounded", isGrounded);
    }

    bool IsGrounded()
    {
        // Use RaycastHit2D for more accurate ground detection
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
        return hit.collider != null;
    }

}
