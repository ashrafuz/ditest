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
    private int Timer;
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
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
        Time.timeScale = 1f;
        Time_update_Max = int.Parse(PlayerPrefs.GetString("updateTime"));
        Time_update = Time_update_Max;

        StartCoroutine("UpdateTimer");
        switch (PlayerPrefs.GetInt(GameConfig.GAME_MODE_KEY)) {
            case 1:
                isEndlessTime = true;
                break;
            case 2:
                isEndlessTime = false;
                break;
        }

        if (!isEndlessTime) {
            StartCoroutine("HiddenObjectsTimer");
            Exit_BTN.gameObject.SetActive(false);
        }
        else {
            timeLabel.text = "00:00";
        }

///////////
        Shooting = true;
        score = 0;
        scoreValue.text = score.ToString();
        ActualForce.text = Force.ToString();
#if UNITY_ANDROID && !UNITY_EDITOR
        	MainMenu.SceneLoad();
#endif
    }

    ///UPDATE TIME
    public IEnumerator UpdateTimer()
    {
        ////Update_Time
        while (true) {
            while ((Time_update > 0)) {
                Time_update--;
                yield return new WaitForSeconds(1);
            }

            yield return new WaitForEndOfFrame();
        }
    }

    /// 
    public IEnumerator HiddenObjectsTimer()
    {
        Timer = maxTime;
        Time_update = Time_update_Max;
        while (true) {
            while ((Timer > 0)) {
                Timer--;
                if (Timer < 10) {
                    timeLabel.text = "00:0" + Timer.ToString();
                }
                else {
                    timeLabel.text = "00:" + Timer.ToString();
                }

                yield return new WaitForSeconds(1);
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public void Shooting_time()
    {
        Ball_img.gameObject.SetActive(true);
        Ball_force.gameObject.SetActive(true);
        BarForce.GetComponent<Slider>().value = 0;
        Shooting = true;
    }

    public void Up_force()
    {
        if (Force < MaxForce) {
            Force += 1;
        }
    }

    public void Down_force()
    {
        if (Force > 1) {
            Force -= 1;
        }
    }

    public void shoot()
    {
        if (Shooting) {
            Ball_img.gameObject.SetActive(false);
            Ball_force.gameObject.SetActive(false);
            Shooting = false;
            GameObject Ball_Poong =
                (GameObject) Instantiate(Balls, Point_Iniciate.position, Point_Iniciate.transform.rotation);
            GameObject Ball_Shadow =
                (GameObject) Instantiate(Shadows, Shadows_Iniciate.position, Shadows_Iniciate.transform.rotation);

            Ball_Poong.GetComponent<Rigidbody>().rotation = Ball_Poong.transform.rotation;
            Ball_Poong.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, Force), ForceMode.VelocityChange);


            BarForce.GetComponent<Slider>().value = 0;
            //Debug.Log ("shoooot: "+Force);
            Invoke("Shooting_time", Time_shooting);
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        Ball_img.gameObject.SetActive(false);
        PopUpExit.gameObject.SetActive(true);
        HUD.gameObject.SetActive(false);
    }

    public void CancelPause()
    {
        Time.timeScale = 1;
        Ball_img.gameObject.SetActive(true);
        PopUpExit.gameObject.SetActive(false);
        HUD.gameObject.SetActive(true);
    }

    public void Exit()
    {
        //Clear Unused Textures
        Resources.UnloadUnusedAssets();

        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
        PlayerPrefs.SetInt(GameConfig.GAME_MODE_KEY, -1);
        SceneManager.LoadScene(GameConstants.MENU_SCENE);
    }

    public void GameOver()
    {
        Ball_img.gameObject.SetActive(false);
        Time.timeScale = 1;
        Totalscore.text = scoreValue.text;
        //PopUpGameOver.gameObject.SetActive(true);
        HUD.gameObject.SetActive(false);
        if (!isEndlessTime) {
            MainMenu.TotalPoints(Totalscore.text.ToString());
            isEndlessTime = true;
            MainMenu.Exit();
        }
    }

    public void TryAgain()
    {
        Time.timeScale = 1;
        PlayerPrefs.SetString("nextLevel", GameConstants.BEER_PONG_SCENE_NAME);

        //Clear Unused Textures
        Resources.UnloadUnusedAssets();
        SceneManager.LoadScene(GameConstants.BEER_PONG_SCENE_NAME);
    }

    // Update is called once per frame
    void Update()
    {
        Bar_indForce.transform.localScale = new Vector3(Force, 1.0f, 1.0f);
        Force = BarForce.GetComponent<Slider>().value;
        //if (Full) 
        //{
        //    Force += speed * Time.deltaTime;
        //    if (Force >= MaxForce)
        //    {
        //        Full = false;
        //    }
        //}
        //else { //full is false
        //    Force -= speed * Time.deltaTime;
        //    if (Force <= 1)
        //    {
        //        Full = true;
        //    }
        //}
        ////////////////
        if (Validate_Force.GetComponent<RectTransform>().localScale.x == 0) {
            BarForce.GetComponent<Slider>().value = 0;
        }

        if (!Validate_Force2.activeInHierarchy) {
            BarForce.GetComponent<Slider>().value = 0;
        }

        if (!isEndlessTime) {
            if (Timer == 0) {
                Time_update = 20;
                gameOverTitle.gameObject.SetActive(true);
                Invoke("GameOver", 2);
            }
        }
        else { }

        if (Time_update == 0) {
#if !UNITY_EDITOR
				UpdatePoints();
#endif
        }

        scoreValue.text = score.ToString();
        ActualForce.text = " " + Force.ToString("f0");

        if (Input.GetButtonDown("Jump")) {
            shoot();
        }
    }

    public void UpdatePoints()
    {
        Time_update = int.Parse(PlayerPrefs.GetString("updateTime"));
        MainMenu.UpdatePoints(score.ToString());
        //Debug.Log("actualizo puntos");
    }
}