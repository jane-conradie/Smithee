using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [SerializeField] private List<CustomerStatusSO> statuses;
    [SerializeField] private float purchaseActionDuration = 3f;
    [SerializeField] private float statusDuration = 3f;

    // customer mood and tipping
    [Header("Customer Mood and Payment")]
    [SerializeField] private float tipAmount = 0.20f;
    [SerializeField] private float moodScore = 100f;
    [SerializeField] private float moodIncreasePerAction = 20f;
    [SerializeField] private float moodDecreasePerSecond = 0.05f;
    [SerializeField] private float baseProductCost = 10f;
    [SerializeField] private float baseServiceCost = 20f;
    [SerializeField] private SpriteRenderer moodSprite;
    [SerializeField] private MoodSO[] moods;
    private float bonusTip = 0f;

    public float moveSpeed;

    // pathfinding
    public PathsSO path;
    private List<Transform> waypoints;
    public bool isWaiting = false;
    public bool isAtRegister = false;
    private bool isMoving = false;
    public int waypointIndex = 0;

    public bool isAtAnvil = false;


    public QueueManager queueManager;
    public CustomerSpawner customerSpawner;

    private Minigame miniGame;

    private void Start()
    {
        waypoints = path.GetWaypoints();
        queueManager = QueueManager.instance;
        customerSpawner = CustomerSpawner.instance;
        miniGame = FindObjectOfType<Minigame>();
    }

    private void Update()
    {
        // move the object if it is not moving
        // and if the object is not at the final waypoint
        if (!isMoving && !isWaiting)
        {
            StartCoroutine(MoveAlongPath());
        }

        if (moodScore > 0)
        {
            DecreaseMood();
        }


        // only update if mood has changed by a lot
        UpdateMoodDisplayed();
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

        if (waypoint.gameObject.tag == "Checkpoint" || waypoint.gameObject.tag == "Anvil")
        {
            // trigger buying of item
            BuyItem();

            // wait for waiting to change
            // this will change in response to player interaction
            yield return new WaitUntil(() => !isWaiting);
        }

        if (isAtRegister)
        {
            isWaiting = true;
            queueManager.AddCustomerToQueue(this);
        }

        if (waypoint == waypoints.Last())
        {
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

        // set status to waiting
        isWaiting = true;

        // TO DO grabbing sound

        // TO DO grabbing animation

        // TO DO pay status


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

    private void DecreaseMood()
    {
        if (isWaiting)
        {
            moodScore -= moodDecreasePerSecond;

            if (moodScore < 0)
            {
                moodScore = 0;
            }
        }
    }

    private void IncreaseMood()
    {
        moodScore += moodIncreasePerAction;

        if (moodScore > 100)
        {
            moodScore = 100;
        }
    }

    private void UpdateMoodDisplayed()
    {
        // get sprite for customer mood
        Sprite sprite = GetSpriteForMoodLevel();

        // update the sprite
        moodSprite.sprite = sprite;
    }

    private Sprite GetSpriteForMoodLevel()
    {
        MoodSO mood = moods.FirstOrDefault((x) => moodScore <= x.GetScore());
        Sprite sprite = mood.GetSprite();

        return sprite;
    }

    public float CalculateCustomerPayment()
    {
        // takes customer mood, base product amount (paymentOwed), and customer tip amount
        // to calculate a final payment at checkout
        double payment = Math.Floor(tipAmount * moodScore);

        return (float)payment;
    }

    public float GetBasePayment()
    {
        return baseProductCost;
    }

    public void RemovePath()
    {
        path.SetIsInUse(false);
    }

    public void HelpCustomer()
    {
        // if at anvil, trigger minigame
        if (isAtAnvil)
        {
            miniGame.StartGame();
        }

        // TO DO add tip based on how fast finished minigame


        // increase mood
        IncreaseMood();

        // set waiting false
        isWaiting = false;
    }
}
