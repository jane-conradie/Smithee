using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public float moveSpeed;

    // pathfinding
    public PathsSO path;
    List<Transform> waypoints;
    bool isMoving = false;
    bool destinationReached = false;
    int waypointIndex = 0;

    void Start()
    {
        waypoints = path.GetWaypoints();
    }

    void Update()
    {
        // move the object if it is not moving
        // and if the object is not at the final waypoint
        if (!isMoving && !destinationReached)
        {
            StartCoroutine(MoveAlongPath());
        }
    }

    // moves a customer along their assigned path until destination has been reached
    // utilised Vector2.MoveTowards()
    IEnumerator MoveAlongPath()
    {
        isMoving = true;

        Vector3 targetPosition = waypoints[waypointIndex].position;

        // moves the object until the target has been reached
        while (Vector2.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
            yield return null;
        }

        // check if at last waypoint
        if (waypoints.Count - 1 == waypointIndex)
        {
            destinationReached = true;
        }
        else
        {
            waypointIndex++;
        }

        isMoving = false;
    }
}
