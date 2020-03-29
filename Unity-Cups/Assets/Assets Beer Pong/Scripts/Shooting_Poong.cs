using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;


public class Shooting_Poong : MonoBehaviour
{
    public Transform Point_Iniciate;
    public Transform Shadows_Iniciate;
    public GameObject Balls;
    public GameObject Shadows;
    public float Force = 10;
    public float MaxForce = 20;
    public int maxTime = 60;
    public int Time_update;
    public int Time_update_Max;
    public int Time_shooting;
    public bool Shooting;
    public Text scoreValue;
    public Text timeLabel;
    public Text Totalscore;
    public Text ActualForce;
    public static int score;
    public GameObject PopUpExit;
    public GameObject PopUpGameOver;
    public GameObject HUD;
    public bool isEndlessTime = false;
    public Text gameOverTitle;
    public GameObject Exit_BTN;
    public bool Full;
    public float speed;
    public Transform BarForce;
    public Transform Bar_indForce;
    public GameObject Ball_img;
    public GameObject Ball_force;
    public GameObject Validate_Force;
    public GameObject Validate_Force2;
    public GameObject BackButton;
    

    // Use this for initialization
    void Start()
    {
    }

    public void Pause()
    {
        Time.timeScale = 0;

    }

    public void CancelPause()
    {
        Time.timeScale = 1;
    }

    public void Exit()
    {
        //Clear Unused Textures
        Resources.UnloadUnusedAssets();

        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }
    
    public void TryAgain()
    {
        Time.timeScale = 1;
        PlayerPrefs.SetString("nextLevel", GameConstants.BEER_PONG_SCENE_NAME);

        //Clear Unused Textures
        Resources.UnloadUnusedAssets();
        SceneManager.LoadScene(GameConstants.BEER_PONG_SCENE_NAME);
    }
}