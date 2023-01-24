using UnityEngine;

public class Player : MonoBehaviour
{
    private static readonly string TRAP_TAG = "Traps";

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 frontColliderPosition;

    private float moveDirX = 0f;
    
    private bool hasDoubleJumped = false;
    private bool hasJumped = false;

    private bool isJumping = false;
    private bool isFalling = false;
    private bool isRunning = false;
    private bool isRunningLeft = false;
    private bool isRunningRight = false;
    private bool isWallSliding = false;
    private bool isWallJumping = false;

    private bool isGrounded = true;
    private bool isOnTheWall = false;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float wallSlideSpeed = 5f;
    [SerializeField] private float wallJumpXForce = 15f;
    [SerializeField] private float wallJumpYForce = 15f;
    [SerializeField] private float cancelWallJumpTime = 0.1f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource dyingSoundEffect;
    [SerializeField] private float fontColliderLenght = .6f;
    [SerializeField] private GameStateSO gameState;

    private string playerState = PlayerState.PLAYER_IDLE;  

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (rb.bodyType == RigidbodyType2D.Static) return;
            
        HandleInput();
    }

    private void FixedUpdate()
    {
        frontColliderPosition = transform.position;

        isGrounded = CheckGroundCollision();
        isOnTheWall = CheckFrontCollision();

        isFalling = rb.velocity.y < -.9f;
        isRunning = moveDirX != 0f;
        isRunningLeft = moveDirX < 0f;
        isRunningRight = moveDirX > 0f;
        isJumping = rb.velocity.y > .1f;
        isWallSliding = isOnTheWall && !isGrounded && isRunning;
    }

    private void HandleInput()
    {
        moveDirX = Input.GetAxisRaw("Horizontal");

        if (rb.velocity.y < 0.8f && isGrounded)
        {
            hasJumped = false;
            hasDoubleJumped = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isWallSliding)
            {
                hasJumped = true;
                hasDoubleJumped = false;
                isWallJumping = true;

                rb.velocity = new Vector2(wallJumpXForce * -moveDirX, wallJumpYForce);

                jumpSoundEffect.Play();

                Invoke(nameof(CancelWallJumping), cancelWallJumpTime);
            }
            else if (!hasDoubleJumped)
            {
                if (hasJumped)
                {
                    hasDoubleJumped = true;
                }
                else
                { 
                    hasJumped = true;
                }

                rb.velocity = new Vector2(rb.velocity.x, jumpForce);

                jumpSoundEffect.Play();
            }
        }

        if (isWallSliding)
        {
            rb.velocity = new Vector2(
                rb.velocity.x,
                Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue)
                );
        }
        else if (!isWallJumping)
        {
            rb.velocity = new Vector2(moveDirX * moveSpeed, rb.velocity.y);
        }

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        Flip();

        if (isWallSliding)
        {
            PlayAnimation(PlayerState.PLAYER_WALL_SLIDING);
        }
        else if (isJumping || isWallJumping)
        {
            if (hasDoubleJumped)
            {
                PlayAnimation(PlayerState.PLAYER_DOUBLE_JUMPING);
            }
            else
            {
                PlayAnimation(PlayerState.PLAYER_JUMPING);
            }
        }
        else if (isFalling)
        {
            PlayAnimation(PlayerState.PLAYER_FALLING);
        }
        else if (isRunning)
        {
            PlayAnimation(PlayerState.PLAYER_RUNNING);
        }
        else
        {
            PlayAnimation(PlayerState.PLAYER_IDLE);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TRAP_TAG))
        {
            Die();
        }
    }

    private void Die()
    {
        dyingSoundEffect.Play();

        PlayAnimation(PlayerState.PLAYER_DYING);

        rb.bodyType = RigidbodyType2D.Static;

        Invoke(nameof(RestartLevel), dyingSoundEffect.clip.length);
    }

    private void RestartLevel()
    {
        gameState.RaiseEvent(GameState.RESTART_LEVEL);
    }

    private void Flip()
    {
        if (!isRunning) return;

        spriteRenderer.flipX = isRunningLeft;
    }

    private void CancelWallJumping()
    {
        isWallJumping = false;
    }

    private bool CheckGroundCollision()
    {
        return Physics2D.BoxCast(boxCollider2D.bounds.center,
            boxCollider2D.bounds.size,
            0f, Vector2.down, .1f, jumpableGround
            );
    }

    private bool CheckFrontCollision()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            frontColliderPosition,
            Vector2.right * moveDirX,
            fontColliderLenght,
            jumpableGround,
            0);

        if (hit.collider != null)
        {
            Debug.Log("Facing: " + hit.collider.name);
        }

        return hit.collider != null;
    }

    private void PlayAnimation(string animation)
    {
        if (animation == playerState) return;

        playerState = animation;

        animator.Play(playerState);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(
            frontColliderPosition,
            (Vector2.right * fontColliderLenght) * moveDirX,
            Color.red
            );
    }
}

static class PlayerState
{
    public static string PLAYER_IDLE = "Player_idle";
    public static string PLAYER_JUMPING = "Player_jumping";
    public static string PLAYER_DOUBLE_JUMPING = "Player_double_jumping";
    public static string PLAYER_FALLING = "Player_falling";
    public static string PLAYER_RUNNING = "Player_running";
    public static string PLAYER_WALL_SLIDING = "Player_wall_sliding";
    public static string PLAYER_DYING = "Player_dying";
}