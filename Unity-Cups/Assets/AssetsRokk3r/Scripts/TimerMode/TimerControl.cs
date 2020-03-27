using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerControl: MonoBehaviour
{
	public bool _isTimerActive = false;
	public bool _endGame = false;
	public Text _timerText;
	public int _timerDuraction = 60;
	public int _countDown = 1;
	public int _countDownEndGame = 1;
	public GameObject BackButton;

	private float _timeInit = 0;
	private static TimerControl shared;
	public int _gameMode = -1;
	private int _points = 0;
	public int Time_update;
	public GameObject LoadingScreen;

	public static TimerControl SharedInstance ()
	{
		return shared;
	}

	// Use this for initialization
	void Awake ()
	{
		shared = this;
		_gameMode = PlayerPrefs.GetInt (GameConfig.GAME_MODE_KEY);

		// Check if is practice mode
		// tournamentID == 1 is practice
		string tournamentID = PlayerPrefs.GetString("tournamentID");
		if (tournamentID != "1") {
			// Is a challenge hidden the back button
			BackButton.gameObject.SetActive(false);
		}


	}

	void Start ()
	{
		_timerDuraction = 60;
	}

	public void StartGame(){

		//check if is a timer mode
		if (_gameMode % 2 == 0) {
			//init the countdown
			StartCoroutine (StartCountDown ());
		}
		#if UNITY_ANDROID && !UNITY_EDITOR
		//Clear Unused Textures
		Resources.UnloadUnusedAssets();

		MainMenu.SceneLoad();
		#endif
		SetUpdateTime ();

	}

	// Update is called once per frame
	void Update ()
	{
		if (_isTimerActive && !_endGame) {
			float count = Time.time - _timeInit;
			count = _timerDuraction - count;

			if (count <= 0) {
				ShootAI.shareAI._canControlBall = false;
				ShootAI.shareAI._enableTouch = false;
				StartCoroutine (StartCountDownEndGame ());
				_endGame = true;
				SendTotalPoints ();
			}

			float minutes = count / 60;
			float seconds = count % 60;
			//print the countdown in screen
			if (_timerText != null && seconds >= 0 && seconds >= 0) {
				_timerText.text = ZeroToLeft (minutes) + ":" + ZeroToLeft (seconds);
			}
		}
	}

	/// <summary>
	/// sets Time_update, depending if Unity received it as param,
	/// otherwise it sets a default
	/// </summary>
	private void SetUpdateTime ()
	{
		bool receivedParam = int.TryParse (PlayerPrefs.GetString ("updateTime"), out Time_update);
		if (!receivedParam) {
			Time_update = 10;
		}
	}

	/// <summary>
	/// Sends the points accumulate in the game session.
	/// </summary>
	public void SendTotalPoints ()
	{
		#if !UNITY_EDITOR
		MainMenu.TotalPoints ("" + _points);
		#endif


	}

	/// <summary>
	/// Starts the count down.
	/// delay to start the new game
	/// </summary>
	/// <returns>The count down.</returns>
	IEnumerator StartCountDown ()
	{
		yield return new WaitForSeconds (_countDown);
		Time.timeScale = 1;
		StartTimer ();

		while(true)
		{
			yield return new WaitForSeconds (_countDown);
			Time_update--;
			if (Time_update == 0) {
				UpdatePoints ();
			}
		}
	}

	/// <summary>
	/// Starts the count down end game.
	/// delay to return to the main menu
	/// </summary>
	/// <returns>The count down end game.</returns>
	IEnumerator StartCountDownEndGame ()
	{
		yield return new WaitForSeconds (_countDownEndGame);

		//Clear Unused Textures
		Resources.UnloadUnusedAssets();

		Application.LoadLevelAsync (0);
	}

	/// <summary>
	/// Starts the timer
	/// </summary>
	public void StartTimer ()
	{
		if (!_isTimerActive) {
			_timeInit = Time.time;
		}
		_isTimerActive = true;
	}

	void OnDestroy()
	{
		shared = null;
	}

	/// <summary>
	/// sends the points to iOS/Android app
	/// and resets Time_update
	/// </summary>
	private void UpdatePoints ()
	{
		MainMenu.UpdatePoints ("" + _points);
		//Debug.Log ("Penalty UpdatePoints ");
		SetUpdateTime ();
	}

	/// <summary>
	/// Sets the points
	/// </summary>
	/// <param name="points">Points.</param>
	public void SetPoints (int points)
	{
		_points = points;
		//		Debug.Log ("SetPoints " + _points);
	}

	public void AddPoints(int pointsToAdd) {
		SetPoints (_points + pointsToAdd);
		TargetController.SharedInstance ().OnPointsChanged (_points);
	}

	/// <summary>
	/// Concatenate a "0" to left if the value is lower than 10
	/// </summary>
	/// <returns>The to left.</returns>
	/// <param name="num">Number.</param>
	public string ZeroToLeft (float num)
	{
		string text = "0" + (int)num;
		return (num < 10) ? text : "" + (int)num;
	}
}
