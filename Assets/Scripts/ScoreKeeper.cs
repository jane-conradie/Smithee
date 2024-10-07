using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreField;

    [SerializeField] private Transform paymentHolderPrefab;
    // TO DO - CHANGE TO TMP_TEXT AND USE SET TEXT

    private float score = 0;

    private void Start()
    {
        scoreField.SetText("0");
    }

    public void AddMoney(float payment, float tip)
    {
        // add payment
        score += payment + tip;

        // instantiate payment holder
        Transform paymentHolder = Instantiate(paymentHolderPrefab, paymentHolderPrefab.transform.position, quaternion.identity);
        // display payment for user
        Payment paymentObject = paymentHolder.GetComponent<Payment>();
        paymentObject.UpdateDisplay(payment, tip);

        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreField.text = score.ToString();
    }
}
