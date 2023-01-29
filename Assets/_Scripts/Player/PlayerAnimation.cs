using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private PlayerStateSO playerState;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (playerState.isDying)
        {
            PlayAnimation(PlayerState.PLAYER_DYING);
        }
        else if (playerState.isWallSliding)
        {
            PlayAnimation(PlayerState.PLAYER_WALL_SLIDING);
        }
        else if (playerState.isFalling)
        {
            PlayAnimation(PlayerState.PLAYER_FALLING);
        }
        else if (playerState.IsJumping)
        {
            if (playerState.currentJumpCount == 1)
            {
                PlayAnimation(PlayerState.PLAYER_JUMPING);
            }
            else
            {
                PlayAnimation(PlayerState.PLAYER_DOUBLE_JUMPING);
            }
        }
        else
        {
            if (playerState.IsRunning)
            {
                PlayAnimation(PlayerState.PLAYER_RUNNING);
            }
            else
            {
                PlayAnimation(PlayerState.PLAYER_IDLE);
            }
        }
    }

    private void PlayAnimation(string animation)
    {
        if (animation == playerState.currentAnimation) return;

        playerState.currentAnimation = animation;

        animator.Play(playerState.currentAnimation);
    }
}

