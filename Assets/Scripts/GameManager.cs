using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // TO DO 180 to start*
    [Header("Time")]
    [SerializeField] private float dayLengthInSeconds = 60f;
    [SerializeField] private float timePassRate = 1f;
    [SerializeField] private TextMeshProUGUI timeText;

    [Header("End Of Day")]
    [SerializeField] private GameObject endOfDayScreen;
    [SerializeField] private TextMeshProUGUI servedText;
    [SerializeField] private TextMeshProUGUI lostText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private float daysToPlay = 3f;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI daysPlayedText;

    private bool isDayPassed = false;

    ObjectManager objectManager;

    public static GameManager instance;

    public float customersServed = 0f;
    public float customersLost = 0f;

    private float timeLeft = 0f;
    private float daysPlayed = 0f;

    ScoreKeeper scoreKeeper;
    CustomerSpawner customerSpawner;
    Minigame minigame;

    private void Awake() 
    {
        if (instance != null)
        {
            Destroy(gameObject);
        } 
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start() 
    {
        objectManager = FindObjectOfType<ObjectManager>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        customerSpawner = FindObjectOfType<CustomerSpawner>();
        minigame = FindObjectOfType<Minigame>();

        // set time left 
        timeLeft = dayLengthInSeconds;
    }

    private void Update() 
    {
        // if day still going and not in a minigame
        // pass time
        if (!isDayPassed && !minigame.isGameInProgress)
        {
            // pass time
            PassTime();

            // format time into readable string
            TimeSpan time = TimeSpan.FromSeconds(timeLeft);

            string formattedTime = string.Format("{0:D2}:{1:D2}", 
            time.Minutes, time.Seconds);

            timeText.SetText(formattedTime);
        }
    }

    // trigger end of day state
    private void PassTime()
    {
        timeLeft -= timePassRate * Time.deltaTime;

        if (timeLeft <= 0 && !isDayPassed)
        {
            isDayPassed = true;
            timeText.SetText("00 : 00");

            ShowEndOfDay();
        }
    }

    private void ShowEndOfDay()
    {
        // increase days played
        daysPlayed++;

        // increase final score
        scoreKeeper.UpdateFinalScore();

        // if in minigame - close and hide
        if (minigame.isGameInProgress)
        {
            // destroy it
            minigame.CancelGame();
        }

        if (daysPlayed >= daysToPlay)
        {
            // show end game summary
            objectManager.ToggleVisibility(gameOverScreen);
            // populate fields
            PopulateEndGame();
            return;
        }

        // show day summary
        objectManager.ToggleVisibility(endOfDayScreen);

        // populate the required fields
        PopulateEndOfDay();
    }

    public void StartNextDay()
    {
        // hide summary
        objectManager.ToggleVisibility(endOfDayScreen);

        // reset time
        timeLeft = dayLengthInSeconds;

        // reset score
        scoreKeeper.ResetScore();

        // reset customer stats
        customersLost = 0;
        customersServed = 0;

        // clear all objects
        ClearScene();

        // start day up again
        isDayPassed = false;
    }

    private void ClearScene()
    {
        // remove all customers
        customerSpawner.ClearCustomers();

        // reset score
        scoreKeeper.ResetScore();
    }

    private void PopulateEndOfDay()
    {
        // customers served
        servedText.SetText(customersServed.ToString());

        // customers lost
        lostText.SetText(customersLost.ToString());

        // total monies
        float score = scoreKeeper.GetScore();
        scoreText.SetText(score.ToString());
    }

    private void PopulateEndGame()
    {
        // populate days played
        string formattedText = $"{daysPlayed} / {daysToPlay}";
         daysPlayedText.SetText(formattedText);

        // populate total score
        finalScoreText.SetText(scoreKeeper.finalScore.ToString());
    }

    public void StartNewGame()
    {
        // increase days played
        daysPlayed = 0;

        // hide game over
        objectManager.ToggleVisibility(gameOverScreen);

        // reset time
        timeLeft = dayLengthInSeconds;

        // reset score
        scoreKeeper.ResetAllScores();

        // reset customer stats
        customersLost = 0;
        customersServed = 0;

        // clear all objects
        ClearScene();

        // start day up again
        isDayPassed = false;
    }
}
