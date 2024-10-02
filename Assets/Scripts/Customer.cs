using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [SerializeField] List<CustomerStatusSO> statuses;

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
        Transform waypoint = waypoints[waypointIndex];

        Vector3 targetPosition = waypoint.position;

        // moves the object until the target has been reached
        while (Vector2.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
            yield return null;
        }

        if (waypoint.gameObject.tag == "Checkpoint")
        {
            // trigger buying of item
            BuyItem();
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

    // reach destination, buy item
    // in love emojie on head
    // move to cashier

    // TO DO mood system that drains the longer the player takes to help them

    void BuyItem()
    {
        StartCoroutine(DisplayStatus("Positive"));

        // TO DO grabbing sound

        // TO DO grabbing animation

        // TO DO move to cashier

        // TO DO pay status

        // TO DO
    }

    IEnumerator DisplayStatus(string sentiment)
    {
        // get random positive sprite for status
        Sprite sprite = GetRandomStatusSpriteBasedOnSentiment(sentiment);

        // get the status holder
        GameObject statusHolder = transform.Find("Status Holder").gameObject;

        // change the sprite of the sprite rendered to the random sprite
        GameObject statusPiece = statusHolder.transform.Find("Status Piece").gameObject;
        SpriteRenderer sr = statusPiece.GetComponent<SpriteRenderer>();
        sr.sprite = sprite;

        // set the status holder to active
        statusHolder.SetActive(true);

        yield return new WaitForSecondsRealtime(5);

        statusHolder.SetActive(false);

        yield return null;
    }

    Sprite GetRandomStatusSpriteBasedOnSentiment(string sentiment)
    {
        CustomerStatusSO status = statuses.FirstOrDefault((x) => x.GetSentiment() == sentiment);

        return status.GetRandomSprite();
    }
}
