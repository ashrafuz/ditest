using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeerPongUI : MonoBehaviour
{
    public Transform BackButton;
    public Text TimerText;

    void Awake()
    {
        // Check if is practice mode
        // tournamentID == 1 is practice
        string tournamentID = PlayerPrefs.GetString("tournamentID");

        if (!tournamentID.Equals("1")) {
            // Is a challenge hidden the back button
            BackButton.gameObject.SetActive(false);
        }
    }

    public void UpdateTimer(int time)
    {
        TimerText.text = time < 10 ? "00:" + time : "00:0" + time;
    }
}
