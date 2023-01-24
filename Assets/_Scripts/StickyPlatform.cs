using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    private static readonly string PLAYER_NAME = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collidedObject = collision.gameObject;

        if (collidedObject.name == PLAYER_NAME)
        {
            collidedObject.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var collidedObject = collision.gameObject;

        if (collidedObject.name == PLAYER_NAME)
        {
            collidedObject.transform.SetParent(null);
        }
    }
}
