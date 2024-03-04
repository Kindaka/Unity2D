using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertivalWayPointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;

    [SerializeField] private float speed = 5f;

    private void Update()
    {
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, waypoints[currentWaypointIndex].transform.position.y), Time.deltaTime * speed);

    }
}
