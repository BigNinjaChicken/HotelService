using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class GameTime : MonoBehaviour
{
    // 240 sec in game time
    private float timeOffset;
    [SerializeField] private float gameLengthSec = 60f * 6f;
    [SerializeField] private TextMeshProUGUI TimerText;

    public float currentInGameTime = -1f;

    [SerializeField] private StationaryPlayerController playerScript;

    // Start is called before the first frame update
    void Start()
    {
        timeOffset = Time.realtimeSinceStartup;
        currentInGameTime  = gameLengthSec;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hour = 0;
        float min = 0;

        min = (Time.realtimeSinceStartup - timeOffset) * (240 / gameLengthSec);
        currentInGameTime = (Time.realtimeSinceStartup - timeOffset);

        while (min > 60)
        {
            hour++;
            min -= 60;
        }

        if (hour == 0)
            hour = 12;

        if (hour >= 4 && hour <= 5)
        {
            hour = 4;
            min = 0;

            // Run end game code here
            if (playerScript.servicePoints >= 4)
            {
                SceneManager.LoadScene("Win", LoadSceneMode.Single);
            } 
            else
            {
                SceneManager.LoadScene("Lose", LoadSceneMode.Single);
            }
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
