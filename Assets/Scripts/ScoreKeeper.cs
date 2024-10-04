using TMPro;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreField;

    private float score = 0;

    private void Start()
    {
        scoreField.SetText("0");
    }

    public void AddMoney()
    {
        // TO DO generate amount based on what was bought/done
        // temp add 10

        score += 10;

        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreField.text = score.ToString();
    }
}
