using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const string CHARACTER_STATE = "characterState";

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    private float dirX = 0f;

    private bool hasDoubleJumped = false;
    private bool hasJumped = false;

    private bool isJumping;
    private bool isFalling = false;
    private bool isRunning = false;
    private bool isRunningRight = false;
    private bool isRunningLeft = false;
    private bool isGrounded = true;
    private bool isIdle = true;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private LayerMask jumpableGround;

    private CharacterState characterState = CharacterState.idle;

    [SerializeField] private AudioSource jumpSoundEffect;

    private enum CharacterState
    {
        idle = 0,
        running = 1,
        jumping = 2,
        falling = 3,
        doubleJumping = 4,
        wallJumping = 5,
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (rb.bodyType == RigidbodyType2D.Static)
        {
            return;
        }

        HandleInputs();
    }

    private void HandleInputs()
    {
        dirX = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        isGrounded = CheckGroundCollision();

        if (rb.velocity.y < 0.8f && isGrounded)
        { 
            hasJumped = false;
            hasDoubleJumped = false;
        }

        if (Input.GetButtonDown("Jump") && !hasDoubleJumped)
        {
            if (hasJumped)
            {
                hasDoubleJumped = true;
            }
            else
            { 
                hasJumped = true;
            }

            jumpSoundEffect.Play();

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        isFalling = rb.velocity.y < -.9f;
        isRunning = dirX != 0f;
        isRunningRight = dirX > 0f;
        isRunningLeft = dirX < 0f;
        isJumping = rb.velocity.y > .1f;

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        Flip();

        if (isRunning)
        {
            characterState = CharacterState.running;
        }
        else
        {
            characterState = CharacterState.idle;
        }

        if (isJumping)
        {
            if (hasDoubleJumped)
            {
                characterState = CharacterState.doubleJumping;
            }
            else
            {
                characterState = CharacterState.jumping;
            }
        }
        else if (isFalling)
        {
            characterState = CharacterState.falling;
        }

        animator.SetInteger(CHARACTER_STATE, (int)characterState);
    }

    private void Flip()
    {
        if (isRunningRight)
        {
            spriteRenderer.flipX = false;
        }
        else if (isRunningLeft)
        {
            spriteRenderer.flipX = true;
        }
    }

    private bool CheckGroundCollision()
    {
        return Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size,
            0f, Vector2.down, .1f, jumpableGround);
    }
}
