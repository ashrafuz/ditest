using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeerPongGameController : MonoBehaviour
{
    [SerializeField] private BallSpawner _ballSpanwer;
    [SerializeField] private BeerPongUI _ui;
    [SerializeField] private int _maxTime;
    [SerializeField] private float _shootInterval;
    [SerializeField] private GameObject _dummyBall;
    [SerializeField] private ShooT _shooter;

    private int score;
    private bool isEndlessTime;
    
    private float timer;
    private int lastUpdatedTime;
    private float lastShotTime = 0;

    private void Awake()
    {
        Validate_Score.OnScore += OnBallCup;
        ShooT.OnShoot += BallShoot;
    }

    void Start()
    {
        int endlessConfig = PlayerPrefs.GetInt(GameConfig.GAME_MODE_KEY);
        if (endlessConfig == 1) {
            isEndlessTime = true;
            _ui.UpdateTimer(0);
        }
        else if (endlessConfig == 2) {
            isEndlessTime = false;
            _ui.UpdateTimer(_maxTime);
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        	MainMenu.SceneLoad();
#endif

        timer = 0;
        lastUpdatedTime = 0;
    }

    private void Update()
    {
        if (!isEndlessTime) {
            timer += Time.deltaTime;
            if (Mathf.FloorToInt(timer) > lastUpdatedTime) {
                //updates each second, no need to use coroutine, not a fan of coroutine because it creates garbage in memory
                lastUpdatedTime = Mathf.FloorToInt(timer);
                _ui.UpdateTimer(Mathf.Clamp(_maxTime - lastUpdatedTime, 0, _maxTime));
            }

            if (timer >= _maxTime) {
                GameOver();
            }
        }

        if (lastShotTime > 0) {
            lastShotTime -= Time.deltaTime;
            if (lastShotTime <= 0) {
                //reactivate shooting mechanisms
                _dummyBall.gameObject.SetActive(true);
                _shooter.gameObject.SetActive(true);
            }
        }
    }

    private void GameOver()
    {
        //clean up ui, exit game
        Time.timeScale = 1;
        _ui.OnGameOver();

        if (!isEndlessTime) {
            MainMenu.TotalPoints(score.ToString());
            isEndlessTime = true;
            MainMenu.Exit();
        }
    }

    private void BallShoot(float force)
    {
        if (lastShotTime>0) { //shooting interval
            return;
        }
        
        lastShotTime = _shootInterval;
        GameObject ball = _ballSpanwer.GetABall();
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.rotation = ball.transform.rotation;
        
        rb.velocity = Vector3.zero;
        rb.isKinematic = false;
        rb.AddRelativeForce(new Vector3(0, 0, force), ForceMode.VelocityChange);
        
        _ui.HideIndicator(false);
        _dummyBall.gameObject.SetActive(false);
        _shooter.gameObject.SetActive(false);
    }

    private void OnBallCup(GameObject ball, int sc)
    {
        score += sc;
        _ui.UpdateScore(score);
        _ballSpanwer.HideBall(ball);
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

    public void TryAgain()
    {
        Time.timeScale = 1;
        PlayerPrefs.SetString("nextLevel", GameConstants.BEER_PONG_SCENE_NAME);

        //Clear Unused Textures
        Resources.UnloadUnusedAssets();
        SceneManager.LoadScene(GameConstants.BEER_PONG_SCENE_NAME);
    }

    public void Pause()
    {
        Time.timeScale = 0;
        _ui.Pause();
    }

    public void CancelPause()
    {
        Time.timeScale = 1;
        _ui.CancelPause();
    }


    //unsubscribe
    private void OnDestroy()
    {
        Validate_Score.OnScore -= OnBallCup;
        ShooT.OnShoot -= BallShoot;
    }
}