using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] private CollectedItems collectedItems;

    [SerializeField] private AudioSource collectSoundEffect;

    private void OnDestroy()
    {
        collectedItems.count = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectable"))
        {
            collectSoundEffect.Play();

            collectedItems.count++;
            Destroy(collision.gameObject);
        }
    }
}
