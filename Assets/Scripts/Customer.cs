using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Customer : MonoBehaviour
{
    [Header("Mood and Payment")]
    [SerializeField] private float tipAmount = 0.20f;
    [SerializeField] private float moodScore = 100f;
    [SerializeField] private float moodIncreasePerAction = 20f;
    [SerializeField] private float moodDecreasePerSecond = 0.05f;
    [SerializeField] private float baseProductCost = 10f;
    [SerializeField] private float baseServiceCost = 20f;
    [SerializeField] private SpriteRenderer moodSprite;
    [SerializeField] private MoodSO[] moods;
    [SerializeField] private float bonusMultiplier = 25f;
    private float bonusTip = 0f;

    [SerializeField] private Animator animator;

    [SerializeField] public GameObject canvas;

    [SerializeField] public SpriteLibraryAsset[] skins = new SpriteLibraryAsset[2];
    [SerializeField] private SpriteLibrary spriteLibrary;

    // movement and pathfinding
    public float moveSpeed;
    public PathsSO path;
    private List<Transform> waypoints;
    public bool isWaiting = false;
    public bool isAtRegister = false;
    private bool isMoving = false;
    public int waypointIndex = 0;
    public bool isAtAnvil = false;

    // dependencies
    public QueueManager queueManager;
    public CustomerSpawner customerSpawner;
    private Anvil minigame;
    private GameManager gameManager;
    private AnimationManager animationManager;

    [Header("Statuses")]
    [SerializeField] private Speech speech;
    [SerializeField] private Thought thought;
    private bool shouldShowStatus = false;

    private bool isRageQuitting = false;
    private bool hasLeftStore = false;

    public bool reachedFront = false;

    private Vector2 currentPosition;

    private void Start()
    {
        waypoints = path.GetWaypoints();
        queueManager = QueueManager.instance;
        customerSpawner = CustomerSpawner.instance;
        minigame = FindObjectOfType<Anvil>();
        gameManager = FindObjectOfType<GameManager>();
        animationManager = GetComponent<AnimationManager>();

        // change to random skin
        ChangeSkin();
    }

    private void FixedUpdate()
    {
         if (!gameManager.isDayPassed && !isRageQuitting)
        //if (!gameManager.isDayPassed && !minigame.isGameInProgress && !isRageQuitting)
        {
            // move the object if it is not moving
            // and if the object is not at the final waypoint
            if (!isMoving && !isWaiting)
            {
                StartCoroutine(MoveAlongPath());
            }

            if (moodScore > 0 && isWaiting)
            {
                DecreaseMood();
            }

            // only update if mood has changed by a lot
            UpdateMoodDisplayed();
        }
    }

    private void ChangeSkin()
    {
        // get random asset for skin
        SpriteLibraryAsset newSkin = skins[UnityEngine.Random.Range(0, skins.Length)];

        // assign new skin
        spriteLibrary.spriteLibraryAsset = newSkin;
    }

    // moves a customer along their assigned path until destination has been reached
    // utilised Vector2.MoveTowards()
    private IEnumerator MoveAlongPath()
    {
        isMoving = true;
        int customersInQueue = queueManager.GetTotalCustomersInQueue();

        Transform waypoint = waypoints[waypointIndex];
        string tag = waypoint.gameObject.tag;
        isAtRegister = tag == "Cash Register";

        Vector2 targetPosition = waypoint.position;
        currentPosition = new Vector2(transform.position.x, transform.position.y);

        customersInQueue++;

        // if on register waypoint
        if (isAtRegister)
        {
            // calculate dist if customers in queue
            // position * num customers in queue
            targetPosition = new Vector2(waypoint.position.x, waypoint.position.y * customersInQueue);
        }

        if (!isWaiting)
        {
            Vector2 movement = targetPosition - currentPosition;
            animationManager.TriggerMovementAnimation(movement, animator, transform, currentPosition);

            while (currentPosition != targetPosition)
            {
                currentPosition = Vector2.MoveTowards(currentPosition, targetPosition, Time.deltaTime * moveSpeed);
                transform.position = currentPosition;

                yield return null;
            }

            waypointIndex++;
        }

        animationManager.StopMovementAnimation(animator);

        // check if customer is at a product, or the anvil for service
        if (tag == "Checkpoint" || tag == "Anvil")
        {
            Buy(tag);

            // wait for wait status to change
            // this will change in response to player interaction
            yield return new WaitUntil(() => !isWaiting);
        }

        if (isAtRegister)
        {
            isWaiting = true;
            queueManager.AddCustomerToQueue(this);

            reachedFront = true;
        }

        if (waypoint == waypoints.Last())
        {
            // clear customer in game manager
            customerSpawner.RemoveCustomerFromStore(this);

            DestroySelf();
        }

        isMoving = false;
    }

    private void Buy(string tag)
    {
        // set status to waiting
        isWaiting = true;
        shouldShowStatus = true;
        bool hasDisplayedThought = false;

        if (tag == "Checkpoint")
        {
            // show thoughts of liking product
            StartCoroutine(DisplayStatus(hasDisplayedThought));

            // TO DO grabbing sound

            // TO DO grabbing animation

            // TO DO pay status
        }
        else
        {
            hasDisplayedThought = true;

            minigame.SetCustomerToServe(this);
            isAtAnvil = true;
        }

        // trigger speech bubble
        StartCoroutine(DisplayStatus(hasDisplayedThought));
    }

    private IEnumerator DisplayStatus(bool hasDisplayedThought)
    {
        if (shouldShowStatus)
        {
                // if thought display first
            if (!hasDisplayedThought)
            {
                // display thought
                thought.gameObject.SetActive(true);

                // wait for has thought displayed
                yield return new WaitForSecondsRealtime(2);

                // hide thought
                thought.gameObject.SetActive(false);
                hasDisplayedThought = true;
            }

            yield return new WaitUntil(() => hasDisplayedThought);

            if (shouldShowStatus)
            {
                // display speech
                speech.gameObject.SetActive(true);
            }
        }
    }

    public IEnumerator MoveForwardInQueue(Vector3 targetPosition)
    {
        reachedFront = false;

        while (Vector2.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
            yield return null;
        }

        reachedFront = true;
    }

    private void DecreaseMood()
    {
        moodScore -= moodDecreasePerSecond;

        if (moodScore < 0)
        {
            moodScore = 0;
        }

        if (moodScore == 0)
        {
            StartCoroutine(RageQuit());
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

        // add bonus tip
        payment += bonusTip;

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

    public void CalculateBonusTip(float timeLeft)
    {
        // TO DO REWORK THIS DUMPSTER FIRE CODE
        // 3 tiers
        // gold, silver, bronze

        // tip amount * 200

        // calculate base tip for service
        bonusTip = tipAmount * bonusMultiplier;

        // calculate extra bonus based on performance
        if (timeLeft < 0.1)
        {
            bonusTip *= 3;
        }
        else if (timeLeft < 0.66)
        {
            bonusTip *= 2;
        }
        else if (timeLeft < 0.33)
        {
            bonusTip *= 1;
        }

        // turn off at anvil status
        isAtAnvil = false;

        // clear customer to serve
        minigame.SetCustomerToServe(null);

        FinishHelp();
    }

    public void FinishHelp()
    {
        // cancel if anvil game not done
        if (isAtAnvil)
        {
            return;
        }

        shouldShowStatus = false;

        // hide statuses
        thought.gameObject.SetActive(false);
        speech.gameObject.SetActive(false);

        // set waiting false
        isWaiting = false;

        // increase mood
        IncreaseMood();
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public IEnumerator RageQuit()
    {
        if (!gameManager.isDayPassed && !isRageQuitting)
        {
            // set rage quit status
            isRageQuitting = true;

            // hide status
            HideStatus();

            StartCoroutine(WalkOutOfStore());
            yield return new WaitUntil(() => hasLeftStore);

            // remove path
            RemovePath();

            // remove from queue
            queueManager.RemoveCustomerFromQueue(this);

            // remove from store
            customerSpawner.RemoveCustomerFromStore(this);

            // take life
            gameManager.TakeLife();

            DestroySelf();
        }
    }

    private IEnumerator WalkOutOfStore()
    {
        // set previous waypoint as the target position
        for (int i = waypointIndex - 1; i >= 0 ; i--)
        {
            Transform waypoint = waypoints[i];
            // target is last one
            Vector2 targetPosition = new Vector2(waypoint.position.x, waypoint.position.y);

            Vector2 movement = targetPosition - currentPosition;
            animationManager.TriggerMovementAnimation(movement, animator, transform, currentPosition);

            // follow path back
            while (currentPosition != targetPosition)
            {
                currentPosition = Vector2.MoveTowards(currentPosition, targetPosition, Time.deltaTime * moveSpeed);
                transform.position = currentPosition;

                yield return null;
            }
        }

        animationManager.StopMovementAnimation(animator);
        hasLeftStore = true;
    }

    private void HideStatus()
    {
        thought.gameObject.SetActive(false);
        speech.gameObject.SetActive(false);
    }
}
