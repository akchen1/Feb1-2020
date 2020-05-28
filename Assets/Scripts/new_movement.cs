using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class new_movement : MonoBehaviour
{
    Rigidbody2D rb;
    private Animator anim;

    public float speed;
    public float jumpForce;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    bool isGrounded = false;
    public Transform isGroundedChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;

    public float rememberGroundedFor;
    float lastTimeGrounded;

    public int defaultAdditionalJumps = 1;
    int additionalJumps;

    private bool isFacingRight = true;
    private float x;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();

        additionalJumps = defaultAdditionalJumps;
    }

    void Update()
    {
        Move();
        checkCharacterDirection();
        Jump();
        BetterJump();
        CheckIfGrounded();
        CheckAnimationStates();
    }


    void Move()
    {
        x = Input.GetAxisRaw("Horizontal");
        float moveBy = x * speed;
        if (Input.GetKey(KeyCode.LeftShift))
        { 
            moveBy = moveBy * 1.5f;
        }

        

        rb.velocity = new Vector2(moveBy, rb.velocity.y);

    }

    private void checkCharacterDirection()
    {
        if (isFacingRight && x < 0)
        {
            flip();
        }
        else if (!isFacingRight && x > 0)
        {
            flip();
        }
    }

    private void flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || Time.time - lastTimeGrounded <= rememberGroundedFor || additionalJumps > 0))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            additionalJumps--;
            
        }      

    }

    void BetterJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void CheckIfGrounded()
    {
        Collider2D colliders = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);

        if (colliders != null)
        {
            isGrounded = true;
            additionalJumps = defaultAdditionalJumps;
        }
        else
        {
            if (isGrounded)
            {
                lastTimeGrounded = Time.time;
            }
            isGrounded = false;
        }

        
    }

    void CheckAnimationStates()
    {
        if (isGrounded)
        {
            anim.SetBool("isGrounded", true);
            anim.SetBool("isFalling", false);
            anim.SetBool("isJumping", false);
            if (rb.velocity.x != 0)
            {
                print(Mathf.Abs(rb.velocity.x) + "   " + speed);
                if (Mathf.Abs(rb.velocity.x) > speed)
                {
                    anim.SetBool("isWalking", false);
                    anim.SetBool("isRunning", true);
                } else
                {
                    anim.SetBool("isRunning", false);
                    anim.SetBool("isWalking", true);
                }
                

                anim.SetBool("isIdle", false);
            }
            else
            {
                anim.SetBool("isIdle", true);
                anim.SetBool("isRunning", false);
                anim.SetBool("isWalking", false);

            }
        } else
        {
            anim.SetBool("isGrounded", false);
            if (rb.velocity.y > 0)
            {
                anim.SetBool("isJumping", true);
                anim.SetBool("isFalling", false);
                anim.SetBool("isIdle", false);
            } else
            {
                anim.SetBool("isJumping", false);
                anim.SetBool("isFalling", true);
                anim.SetBool("isIdle", false);
            }
        }
    }
}
