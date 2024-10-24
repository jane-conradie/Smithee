using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager instance;

    [SerializeField] private GameObject minigamePrefab;

    [Header("Tip & Timing")]
    [SerializeField] private float countdown = 0.05f;
    [SerializeField] private float timeToComplete = 1f;
    public float timeLeft;

    [Header("Changable UI Elements")]
    private Slider timeSlider;
    private TextMeshProUGUI minigameText;

    public bool isGameInProgress = false;
    private string gameInProgress;

    private bool shouldCountdown = false;
    private Customer customerToServe;

    private Anvil anvil;
    private Sales sales;
    private PlayerMovement playerMovement;

    public GameObject minigame;
    public GameObject minigameBase;

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
        anvil = FindObjectOfType<Anvil>();
        sales = FindObjectOfType<Sales>();
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        if (shouldCountdown)
        {
            // countdown timer and set the value
            timeLeft -= countdown * Time.deltaTime;
            UpdateTimeSlider(timeLeft);

            if (timeLeft <= 0)
            {
                // customer rage quit
                StartCoroutine(customerToServe.RageQuit());

                // end mini game
                CancelGame();
            }
        }
    }

    public void StartMinigame(string tag, GameObject minigameObject)
    {
        customerToServe = minigameObject.GetComponent<Interactable>().customer;

        if (!isGameInProgress && customerToServe && customerToServe.isWaiting)
        {
            // reset time left
            timeLeft = timeToComplete;

            // instantiate the base minigame
            minigameBase = Instantiate(minigamePrefab, minigamePrefab.transform.position, quaternion.identity);
            // get the necessary components that need to change
            minigameText = minigameBase.GetComponentInChildren<TextMeshProUGUI>();
            timeSlider = minigameBase.GetComponentInChildren<Slider>();

            switch (tag)
            {
                case "Anvil":
                    anvil.StartGame();
                    break;
                case "Sellable":
                    sales.StartSale();
                    break;
                default:
                    break;
            }

            isGameInProgress = true;
            gameInProgress = tag;
            shouldCountdown = true;

            // disable player movement
            playerMovement.ToggleControlsOnOrOff(false);
        }
    }

    public IEnumerator EndMiniGame()
    {
        // stop timer
        shouldCountdown = false;

        // set game in progress
        gameInProgress = "";

        // change text to tell user game is done
        UpdateText("Done!");

        // TO DO CHANGE ALL AREAS WHERE WAIT FOR SECONDS IS HARDCODED
        yield return new WaitForSecondsRealtime(2);

        // calculate bonus tip for customer
        customerToServe.CalculateBonusTip(timeLeft);

        // remove customer to serve
        customerToServe = null;

        CancelGame();
    }

    public void CancelGame()
    {
        // end game in progress
        isGameInProgress = false;

        // stop countdown
        shouldCountdown = false;

        // destory minigame, whooo hooo
        Destroy(minigameBase);

        // reenable the controls
        playerMovement.ToggleControlsOnOrOff(true);
    }

    public void UpdateText(string text)
    {
        minigameText.text = text;
    }

    public void UpdateTimeSlider(float timeToComplete)
    {
        timeSlider.value = timeToComplete;
    }

    public void HandleClick(GameObject clickedObject)
    {
        // first check if cancel was clicked
        if (clickedObject.tag == "Cancel")
        {
            CancelGame();
            return;
        }

        if (gameInProgress == "Sellable")
        {
            // no matter what clicked on
            // trigger doing sale
            sales.SellItem(customerToServe);
        }

        // if clicked on a piece, trigger putting it back together
        if (clickedObject.tag == "Fixable Piece")
        {
            anvil.FixPiece(clickedObject);
        }
    }
}
