using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collidedObject = collision.gameObject;

        if (collidedObject.name == "Player")
        {
            collidedObject.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var collidedObject = collision.gameObject;

        if (collidedObject.name == "Player")
        {
            collidedObject.transform.SetParent(null);
        }
    }
}
