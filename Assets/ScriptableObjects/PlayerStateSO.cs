using UnityEngine;

[CreateAssetMenu(fileName = "PlayerState", menuName = "PlayerState")]
public class PlayerStateSO : ScriptableObject
{
    [SerializeField] public int maxJumpCount = 2;
    [SerializeField] public float moveSpeed = 8f;
    [SerializeField] public float jumpForce = 16f;

    [HideInInspector] public float moveDirX = 0f;
    [HideInInspector] public float wallJumpDirection = 0f;
    [HideInInspector] public int currentJumpCount = 0;

    public bool isOnTheGround = true;
    public bool isFalling = false;
    public bool isFacingWall = false;
    public bool isWallSliding = false;
    public bool isWallJumping = false;
    public bool isDying = false;
    public bool wasOnTheGround = true;

    public string currentAnimation = PlayerState.PLAYER_IDLE;

    public bool IsJumping => currentJumpCount != 0;
    public bool IsRunning => moveDirX != 0;
}
