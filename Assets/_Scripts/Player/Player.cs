using UnityEngine;

public class Player : MonoBehaviour
{
    private static readonly string TRAP_TAG = "Traps";
    private static readonly string COLLECTABLE_TAG = "Collectable";

    private PlayerSoundEffect soundEffect;

    [SerializeField] private GameStateSO gameState;
    [SerializeField] private PlayerStateSO playerState;
    [SerializeField] private CollectedItemsSO collectedItems;

    private void Awake()
    {
        soundEffect = GetComponent<PlayerSoundEffect>();
    }

    private void OnDestroy()
    {
        collectedItems.count = 0;
    }

    private void Die()
    {
        playerState.isDying = true;

        soundEffect.PlayDyingEffect();

        Invoke(nameof(RestartLevel), soundEffect.currentClipLength);
    }

    private void Collect(GameObject collectableGameObject)
    {
        soundEffect.PlayCollectEffect();

        collectedItems.count++;

        Destroy(collectableGameObject);
    }

    private void RestartLevel()
    {
        gameState.RaiseEvent(GameState.RESTART_LEVEL);

        playerState.isDying = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(TRAP_TAG))
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(COLLECTABLE_TAG))
        {
            Collect(collision.gameObject);
        }
    }
}

