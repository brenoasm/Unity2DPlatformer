using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;
    private PlayerSoundEffect soundEffect;

    [SerializeField] private float wallSlideSpeed = 2f;
    [SerializeField] private float wallJumpXForce = 8f;
    [SerializeField] private float wallJumpYForce = 16f;
    [SerializeField] private float cancelWallJumpTime = 0.4f;
    
    [SerializeField] private LayerMask terrainLayerMask;

    [SerializeField] private PlayerStateSO playerState;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        soundEffect = GetComponent<PlayerSoundEffect>();
    }

    private void FixedUpdate()
    {
        if (playerState.isDying)
        {
            rb.bodyType = RigidbodyType2D.Static;

            return;
        }

        playerState.isOnTheGround = IsCollidingWithTerrain();
        playerState.isFacingWall = IsCollidingWithWall();
        playerState.isFalling = !playerState.isOnTheGround && rb.velocity.y < 0.1f;

        if (!playerState.isWallJumping)
        {
            Move(new Vector2(playerState.moveDirX * playerState.moveSpeed, rb.velocity.y));
        }
    }

    private void Update()
    {
        if (playerState.isDying) return;

        playerState.moveDirX = Input.GetAxisRaw("Horizontal");

        if (playerState.isOnTheGround && rb.velocity.y < 0.8f)
        {
            playerState.currentJumpCount = 0;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (playerState.isWallSliding)
            {
                WallJump();
            }
            else
            {
                OnJumpPress(new Vector2(rb.velocity.x, playerState.jumpForce));
            }
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            OnJumpRelease(new Vector2(rb.velocity.x, rb.velocity.y * 0.5f));
        }

        WallSlide();
    }

    private void WallJump()
    {
        playerState.isWallJumping = true;

        OnJumpPress(new Vector2(playerState.wallJumpDirection * wallJumpXForce, wallJumpYForce));

        Invoke(nameof(CancelWallJump), cancelWallJumpTime);
    }

    private void CancelWallJump()
    {
        playerState.isWallJumping = false;
    }

    private void WallSlide()
    {
        if (!playerState.isWallJumping && playerState.isFacingWall && !playerState.isOnTheGround && playerState.moveDirX != 0f)
        {
            playerState.wallJumpDirection = playerState.moveDirX * -1;

            Move(new Vector2(rb.velocity.x,
                Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue)));

            playerState.isWallSliding = true;
            playerState.currentJumpCount = 0;

            Invoke(nameof(CancelWallJump), 0f);
        }
        else
        {
            playerState.isWallSliding = false;
        }
    }

    private void Move(Vector2 movePower)
    {
        rb.velocity = movePower;
    }

    private void OnJumpPress(Vector2 jumpPower)
    {
        if (playerState.currentJumpCount >= playerState.maxJumpCount) return;

        rb.velocity = jumpPower;

        soundEffect.PlayJumpingEffect();

        playerState.currentJumpCount++;
    }

    private void OnJumpRelease(Vector2 jumpPower)
    {
        rb.velocity = jumpPower;
    }

    private bool IsCollidingWithTerrain()
    {
        float extraHeight = .02f;

        RaycastHit2D hit = Physics2D.Raycast(
            boxCollider2D.bounds.center,
            Vector2.down,
            boxCollider2D.bounds.extents.y + extraHeight,
            terrainLayerMask
            );

        Color rayColor = hit.collider == null ? Color.red : Color.green;

        Debug.DrawRay(
             boxCollider2D.bounds.center,
             Vector2.down * (boxCollider2D.bounds.extents.y + extraHeight),
             rayColor
            );

        return hit.collider != null;
    }

    private bool IsCollidingWithWall()
    {
        float extraWidth = .02f;

        RaycastHit2D hit = Physics2D.Raycast(
            boxCollider2D.bounds.center,
            Vector2.right * playerState.moveDirX,
            boxCollider2D.bounds.extents.x + extraWidth,
            terrainLayerMask);

        Color rayColor = hit.collider == null ? Color.red : Color.green;

        Debug.DrawRay(
             boxCollider2D.bounds.center,
             Vector2.right * playerState.moveDirX * (boxCollider2D.bounds.extents.x + extraWidth),
             rayColor
            );

        return hit.collider != null;
    }
}
