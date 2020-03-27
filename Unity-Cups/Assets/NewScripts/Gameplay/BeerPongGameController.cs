using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerPongGameController : MonoBehaviour
{
    public static Action<GameObject> OnBallInCup;
    public static Action<float> OnShoot;

    [SerializeField] private BallSpawner _ballSpanwer;
    [SerializeField] private BeerPongUI _ui;
    [SerializeField] private int _maxTime;

    private int score;
    private bool isEndlessTime;
    private float timer;
    private int lastUpdatedTime;
    
    void Start()
    {
        OnBallInCup += OnBallCup;
        OnShoot += BallShoot;
        
        int endlessConfig = PlayerPrefs.GetInt(GameConfig.GAME_MODE_KEY);
        if (endlessConfig == 1) {
            isEndlessTime = true;
            _ui.UpdateTimer(0);
        } else if (endlessConfig == 2) {
            isEndlessTime = false;
            _ui.UpdateTimer(_maxTime);
        }
        
        timer = 0;
        lastUpdatedTime = 0;
    }

    private void Update()
    {
        if (!isEndlessTime) {
            timer += Time.deltaTime;
            if (Mathf.FloorToInt(timer)  > lastUpdatedTime) { //updates each second, no need to use coroutine
                lastUpdatedTime = Mathf.FloorToInt(timer);
                _ui.UpdateTimer(Mathf.Clamp(_maxTime - lastUpdatedTime, 0, _maxTime) );
            }

            if (timer >= _maxTime) {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        
    }

    private void BallShoot(float force)
    {
        GameObject ball = _ballSpanwer.GetABall();
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.rotation = ball.transform.rotation;
        rb.isKinematic = false;
        rb.AddRelativeForce(new Vector3(0,0,force), ForceMode.VelocityChange);
    }

    private void OnBallCup(GameObject _obj)
    {
        //update score
        //update ui, particles and stuff
    }
    
    
    //unsubscribe
    private void OnDestroy()
    {
        OnBallInCup -= OnBallCup;
        OnShoot -= BallShoot;
    }
}
