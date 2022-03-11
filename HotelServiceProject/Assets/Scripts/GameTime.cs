using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameTime : MonoBehaviour
{
    // 240 sec in game time
    private float timeOffset;
    [SerializeField] private float gameLengthSec = 10f;

    [SerializeField] private TextMeshProUGUI TimerText;

    // Start is called before the first frame update
    void Start()
    {
        timeOffset = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hour = 0;
        float min = 0;

        min = (Time.realtimeSinceStartup - timeOffset) * (240 / gameLengthSec);

        while (min > 60)
        {
            hour++;
            min -= 60;
        }

        if (hour == 0)
            hour = 12;

        if (hour == 4)
        {
            hour = 4;
            min = 0;

            // Run end game code here
        }

        displayTime((int)hour, (int)min);
        
    }

    void displayTime(int hour, int min)
    {
        string sHour = hour.ToString();
        string sMin = min.ToString();

        if (hour < 10)
        {
            sHour = "0" + sHour;
        }

        if (min < 10)
        {
            sMin = "0" + sMin;
        }

        TimerText.text = "- " + sHour + ":" + sMin + " -";
    }


}
