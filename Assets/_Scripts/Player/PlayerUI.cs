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
        if (playerState.moveDirX == 0f) return;

        spriteRenderer.flipX = playerState.moveDirX < 0f;
    }
}
