using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class Sales : MonoBehaviour
{
    [SerializeField] private GameObject salesPrefab;
    [SerializeField] public TextMeshProUGUI salesText;

    [Header("Zone Mood Modifiers")]
    [SerializeField] private float greenZoneModifier = 1.5f;
    [SerializeField] private float orangeZoneModifier = 1f;
    [SerializeField] private float redZoneModifier = 0f;

    private MinigameManager minigameManager;
    private Indicator indicator;

    private void Start()
    {
        minigameManager = MinigameManager.instance;
    }

    public void StartSale()
    {
        // instantiate sales minigame prefab
        GameObject sale = Instantiate(salesPrefab, salesPrefab.transform.position, quaternion.identity);
        // reference the indicator
        indicator = sale.GetComponentInChildren<Indicator>();

        // set minigame text to right text
        minigameManager.minigame = sale;
        minigameManager.UpdateText(salesText.text);
    }

    public void SellItem(Customer customer)
    {
        // stop indicator
        indicator.ToggleMoveIndicator();

        // check where indicator is
        string zone = indicator.zone;
        float moodModifier = 1f;

        // change customer mood based on zone landed on
        switch (zone)
        {
            case "Green Zone":
                moodModifier = greenZoneModifier;
                break;
            case "Orange Zone":
                moodModifier = orangeZoneModifier;
                break;
            case "Red Zone":
                moodModifier = redZoneModifier;
                break;
            default:
                break;
        }

        customer.ChangeMood(moodModifier);

        // end game
        StartCoroutine(minigameManager.EndMiniGame());
    }

}
