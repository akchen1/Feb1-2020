using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce = 16f;
    public float accelerateDownForce = 10f;
    public float airMoveForce = 135f;
    public float groundCheckRadius;
    public float airDrag = 0.95f;

    public int amountOfJumps = 2;

    public Transform groundCheck;
    public LayerMask whatIsGround;


    private Rigidbody2D rb;
    private float moveDirectionX;
    private float variableJumpHeightMultiplier = 0.5f;
    private bool isFacingRight = true;
    private bool isGrounded;
    private bool canJump;
    private int jumpsLeft;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpsLeft = amountOfJumps;
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();
        checkCharacterDirection();
        checkCanJump();
    }
 
    void FixedUpdate()
    {
        moveCharacter();
        checkGround();
    }

    private void checkInput()
    {
        // use GetAxis or GetAxixRaw
        moveDirectionX = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.W))
        {
            jump();
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (!isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, -accelerateDownForce);
            }
        }

    }

    private void moveCharacter()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(moveDirectionX * speed, rb.velocity.y);
        } else if (!isGrounded && moveDirectionX != 0)
        {
            Vector2 force = new Vector2(airMoveForce * moveDirectionX, 0);
            rb.AddForce(force);

            if (Mathf.Abs(rb.velocity.x) > speed)
            {
                rb.velocity = new Vector2(speed * moveDirectionX, rb.velocity.y);
            }
        } else if (!isGrounded && moveDirectionX == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDrag, rb.velocity.y);
        }
    }

    private void jump()
    {
        if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpsLeft--;
        }
        
    }

    private void checkCharacterDirection()
    {
        if (isFacingRight && moveDirectionX < 0)
        {
            flip();
        } else if (!isFacingRight && moveDirectionX > 0)
        {
            flip();
        }
    }

    private void flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void checkGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    private void checkCanJump()
    {
        if (isGrounded && rb.velocity.y <= 0)
        {
            jumpsLeft = amountOfJumps;
        }
        canJump = jumpsLeft <= 0 ? false : true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
