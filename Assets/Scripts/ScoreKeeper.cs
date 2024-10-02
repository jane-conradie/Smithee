using TMPro;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreField;

    float score = 0;

    void Start()
    {
        scoreField.text = "0";
    }

    public void AddMoney()
    {
        // TO DO generate amount based on what was bought/done
        // temp add 10

        score += 10;

        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreField.text = score.ToString();
    }
}
