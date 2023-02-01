using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private PlayerStateSO playerState;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        Flip();
    }

    private void Flip()
    {
        if (playerState.isWallJumping)
        {
            switch (playerState.wallJumpDirection)
            {
                case < 0f:
                    spriteRenderer.flipX = true;
                    break;
                case > 0f:
                    spriteRenderer.flipX = false;
                    break;
            }
        }
        else
        {
            switch (playerState.moveDirX)
            {
                case < 0f:
                    spriteRenderer.flipX = true;
                    break;
                case > 0f:
                    spriteRenderer.flipX = false;
                    break;
            }
        }
    }
}
