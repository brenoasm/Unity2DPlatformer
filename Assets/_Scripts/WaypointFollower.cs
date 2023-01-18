using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float speed = 2f;

    private int currentWaypointIndex = 0;

    private GameObject currentWaypoint { get { return waypoints[currentWaypointIndex]; } }

    private void Update()
    {
        if (Vector2.Distance(
            currentWaypoint.transform.position,
            transform.position
            ) < .1f)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }

        transform.position = Vector2.MoveTowards(
            transform.position,
            currentWaypoint.transform.position,
            Time.deltaTime * speed
            );
    }
}
