using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeerPongUI : MonoBehaviour
{
    [Header("Texts")]
    public Text TimerText;
    public Text ScoreText;
    public Text TotalScore;
    
    [Space][Header("Gameobjects/Transforms")]
    public Transform PopupExit;
    public Transform BackButton;
    public Transform HUD;
    public Transform BallImage;

    public Image IndicatorImage;
    public Image BarImage;
    public Slider ForceSlider;

    void Awake()
    {
        // Check if is practice mode
        // tournamentID == 1 is practice
        string tournamentID = PlayerPrefs.GetString(GameConstants.TOURNAMENT_ID_KEY);

        if (!tournamentID.Equals("1")) {
            // Is a challenge hidden the back button
            BackButton.gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        BarImage.transform.localScale = new Vector3(ForceSlider.value, 1,1);
    }

    public void HideIndicator(bool _flag)
    {
        IndicatorImage.gameObject.SetActive(_flag);
    }

    public void Pause()
    {
        BallImage.gameObject.SetActive(false);
        PopupExit.gameObject.SetActive(true);
        HUD.gameObject.SetActive(false);
    }

    public void CancelPause()
    {
        BallImage.gameObject.SetActive(true);
        PopupExit.gameObject.SetActive(false);
        HUD.gameObject.SetActive(true);
    }

    public void OnGameOver()
    {
        BallImage.gameObject.SetActive(false);
        HUD.gameObject.SetActive(false);
        TotalScore.text = ScoreText.text; //final score text
    }

    public void UpdateTimer(int time)
    {
        TimerText.text = time > 9 ? "00:" + time : "00:0" + time;
    }

    public void UpdateScore(int score)
    {
        ScoreText.text = score.ToString();
    }
}
