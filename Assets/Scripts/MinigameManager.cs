using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    [Header("Tip & Timing")]
    [SerializeField] private float countdown = 0.05f;
    [SerializeField] private float timeToComplete = 1f;
    private float timeLeft;

    [Header("Changable UI Elements")]
    [SerializeField] private Slider timeSlider;
    [SerializeField] private TextMeshProUGUI minigameText;

    public bool isGameInProgress = false;
    private string gameInProgress;

    private bool shouldCountdown = false;
    private Customer customerToServe;

    private Anvil anvil;
    private Sales sales;
    private PlayerMovement playerMovement;

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

        if (!isGameInProgress && customerToServe)
        {
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

    private IEnumerator EndMiniGame()
    {
        // stop timer
        shouldCountdown = false;

        // change text to tell user game is done
        //minigameManager.UpdateDisplay();

        // TO DO CHANGE ALL AREAS WHERE WAIT FOR SECONDS IS HARDCODED
        yield return new WaitForSecondsRealtime(2);

        // calculate bonus tip for customer
        customerToServe.CalculateBonusTip(timeLeft);

        CancelGame();
    }

    public void CancelGame()
    {
        // reset time left
        timeLeft = timeToComplete;

        // end game in progress
        isGameInProgress = false;

        // stop countdown
        shouldCountdown = false;

        // destory minigame, whooo hooo
        // TO DO IF TIME - COMBINE THESE TWO SO ONLY DESTROY ONE
        //Destroy(minigame);
        //Destroy(fixableObject);

        // reenable the controls
        playerMovement.ToggleControlsOnOrOff(true);
    }

    public void UpdateDisplay()
    {
        minigameText.text = "Done!";
    }

    public void UpdateTimeSlider(float timeToComplete)
    {
        timeSlider.value = timeToComplete;
    }

    public void HandleClick(GameObject clickedObject)
    {
        Debug.Log(clickedObject.name);

        // first check if cancel was clicked
        if (clickedObject.tag == "Cancel")
        {
            Debug.Log("should cancel game");
            CancelGame();
            return;
        }

        if (gameInProgress == "Sellable")
        {
            // no matter what clicked on
            // trigger doing sale
            sales.SellItem();
        }

        // if clicked on a piece, trigger putting it back together
        if (clickedObject.tag == "Fixable Piece")
        {
            //miniGame.FixPiece(objectHit);
        }
    }
}
