using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreenWin;
    [SerializeField] private GameObject gameOverScreenLose;
    [SerializeField] private float daysToPlay = 3f;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI daysPlayedText;
    [SerializeField] private float totalLives = 3f;
    [SerializeField] private TextMeshProUGUI totalLivesText;
    [SerializeField] private Slider livesSlider;

    public bool isDayPassed = false;

    ObjectManager objectManager;

    public static GameManager instance;

    public float customersServed = 0f;
    public float customersLost = 0f;

    private float timeLeft = 0f;
    private float daysPlayed = 0f;

    private bool isGameOver = false;
    private bool hasWon = false;

    ScoreKeeper scoreKeeper;
    CustomerSpawner customerSpawner;
    PlayerMovement playerMovement;

    MinigameManager minigameManager;

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
        minigameManager = MinigameManager.instance;
        playerMovement = FindObjectOfType<PlayerMovement>();

        // set time left 
        timeLeft = dayLengthInSeconds;

        // lives
        livesSlider.maxValue = totalLives;
        customersLost = 0;
        UpdateLives();
    }

    private void Update()
    {
        // if day still going and not in a minigame
        // pass time
        if (!isDayPassed && !minigameManager.isGameInProgress)
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

        if (timeLeft <= 0)
        {
            isDayPassed = true;
            ShowEndOfDay();
        }
    }

    private void ShowEndOfDay()
    {
        // disable player movement
        playerMovement.ToggleControlsOnOrOff(false);

        // increase days played
        daysPlayed++;

        // increase final score
        scoreKeeper.UpdateFinalScore();

        // if in minigame - close and hide
        if (minigameManager.isGameInProgress)
        {
            // destroy it
            minigameManager.CancelGame();
        }

        if (daysPlayed >= daysToPlay)
        {
            hasWon = true;
            isGameOver = true;
            ShowGameOver();
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

        // reset customer stats
        customersLost = 0;
        customersServed = 0;

        // update slider
        UpdateLives();

        // clear all objects
        ClearScene();

        // start day up again
        isDayPassed = false;

        // enable player movement
        playerMovement.ToggleControlsOnOrOff(true);
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
        objectManager.ToggleVisibility(hasWon ? gameOverScreenWin : gameOverScreenLose);

        // reset time
        timeLeft = dayLengthInSeconds;

        // reset score
        scoreKeeper.ResetAllScores();

        // reset customer stats
        customersLost = 0;
        UpdateLives();

        customersServed = 0;

        // clear all objects
        ClearScene();

        // start day up again
        isDayPassed = false;

        // reset game over
        isGameOver = false;

        // enable player movement
        playerMovement.ToggleControlsOnOrOff(true);
    }

    private void UpdateLives()
    {
        // update text
        string formattedText = $"{customersLost} / {totalLives}";
        totalLivesText.SetText(formattedText);

        // update slider value
        livesSlider.value = customersLost;
    }

    public void TakeLife()
    {
        if (customersLost < totalLives)
        {
            customersLost++;
        }

        if (customersLost == totalLives)
        {
            isDayPassed = true;
            isGameOver = true;
            hasWon = false;
            ShowGameOver();
        }

        UpdateLives();
    }

    private void ShowGameOver()
    {
        // disable player movement
        playerMovement.ToggleControlsOnOrOff(false);

        // show end game summary
        objectManager.ToggleVisibility(hasWon ? gameOverScreenWin : gameOverScreenLose);
        // populate fields
        PopulateEndGame();
    }
}
