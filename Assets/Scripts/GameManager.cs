using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float dayLengthInSeconds = 180f;
    [SerializeField] private float timePassRate = 1f;
    [SerializeField] private TextMeshProUGUI timeText;

    private bool isDayPassed = false;

    private void Start() 
    {
        timeText.SetText((dayLengthInSeconds / 60).ToString());
    }

    // pass/control time of day
    private void Update() 
    {
        if (!isDayPassed)
        {
            // pass time
            PassTime();

            timeText.SetText((dayLengthInSeconds / 60).ToString());
        }
    }

    // trigger end of day state
    private void PassTime()
    {
        dayLengthInSeconds -= timePassRate * Time.deltaTime;

        if (dayLengthInSeconds <= 0)
        {
            isDayPassed = true;
            timeText.SetText("00 : 00");

            ShowEndOfDay();
        }
    }

    private void ShowEndOfDay()
    {

    }
}
