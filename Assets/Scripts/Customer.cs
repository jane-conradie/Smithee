using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [SerializeField] private List<CustomerStatusSO> statuses;

    [SerializeField] private float purchaseActionDuration = 3f;
    [SerializeField] private float statusDuration = 3f;

    public float moveSpeed;

    // pathfinding
    public PathsSO path;
    private List<Transform> waypoints;
    public bool isWaiting = false;
    private bool isAtRegister = false;
    private bool isMoving = false;
    public int waypointIndex = 0;

    public QueueManager queueManager;
    public CustomerSpawner customerSpawner;

    private void Start()
    {
        waypoints = path.GetWaypoints();
        queueManager = QueueManager.instance;
        customerSpawner = CustomerSpawner.instance;
    }

    private void Update()
    {
        // move the object if it is not moving
        // and if the object is not at the final waypoint
        if (!isMoving && !isWaiting)
        {
            StartCoroutine(MoveAlongPath());
        }
    }

    // moves a customer along their assigned path until destination has been reached
    // utilised Vector2.MoveTowards()
    private IEnumerator MoveAlongPath()
    {
        isMoving = true;
        int customersInQueue = queueManager.GetTotalCustomersInQueue();

        Transform waypoint = waypoints[waypointIndex];
        isAtRegister = waypoint.gameObject.tag == "Cash Register";

        Vector3 targetPosition = waypoint.position;

        customersInQueue++;

        // if on register waypoint
        if (isAtRegister)
        {
            // calculate dist if customers in queue
            // position * num customers in queue
            targetPosition = new Vector3(waypoint.position.x, waypoint.position.y * customersInQueue, waypoint.position.z);
        }

        if (!isWaiting)
        {
            // moves the object until the target has been reached
            while (Vector2.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
                yield return null;
            }
        }

        if (waypoint.gameObject.tag == "Checkpoint")
        {
            // trigger buying of item
            BuyItem();
            yield return new WaitForSecondsRealtime(purchaseActionDuration);
        }

        if (isAtRegister)
        {
            isWaiting = true;
            queueManager.AddCustomerToQueue(this);
        }

        if (waypoint == waypoints.Last())
        {
            customerSpawner.DespawnCustomer();
            Destroy(gameObject);
        }

        if (!isWaiting)
        {
            waypointIndex++;
        }

        isMoving = false;
    }

    private void BuyItem()
    {
        StartCoroutine(DisplayStatus("Positive"));

        // TO DO grabbing sound

        // TO DO grabbing animation

        // TO DO move to cashier

        // TO DO pay status

        // TO DO
    }

    private IEnumerator DisplayStatus(string sentiment)
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

        yield return new WaitForSecondsRealtime(statusDuration);

        statusHolder.SetActive(false);
    }

    private Sprite GetRandomStatusSpriteBasedOnSentiment(string sentiment)
    {
        CustomerStatusSO status = statuses.FirstOrDefault((x) => x.GetSentiment() == sentiment);

        return status.GetRandomSprite();
    }

    public IEnumerator MoveForwardInQueue(Vector3 targetPosition)
    {
        while (Vector2.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
            yield return null;
        }
    }
}
