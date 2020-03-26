using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using System.IO;
using LitJson;

public class MainMenu : MonoBehaviour
{
	public string Test_Json;
	//{  "gameID": "7",  "tournamentID":"1",  "scene":"Trivia_practice_mode",  "updateTime": "20","isDebug":"Debug" }
	private JsonData Data;
	public static string MODE = "game_mode";




	#if UNITY_IOS

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public void TotalPoints(string points);
	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public void UpdatePoints(string points);
	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public void LoadingError(string errorM);
	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public void NativeLog(string log);

	/*
	public static void TotalPoints(string points)
	{
		//Debug.Log ("Recibio TOTALPOINTS "+points);
	}
	public static void UpdatePoints(string points)
	{
		//Debug.Log ("Recibio UPDATEPOINTS "+points);
	}
	public static void LoadingError(string errorM)
	{
		//Debug.Log ("Recibio LoadingError "+errorM);
	}
	*/

	#endif



	//dummy for tests in the editor
	#if UNITY_EDITOR_IOS
	public static void TotalPoints(string points)
	{
	//Debug.Log ("Recibio TOTALPOINTS "+points);
	}
	public static void UpdatePoints(string points)
	{
	//Debug.Log ("Recibio UPDATEPOINTS "+points);
	}
	public static void LoadingError(string errorM)
	{
	//Debug.Log ("Recibio LoadingError "+errorM);
	}
	public static void NativeLog(string log)
	{
	}
	#endif


	#if UNITY_ANDROID

	public static void LoadingError(string errorM)
	{
		using (var javaUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (var currentActivity = javaUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				currentActivity.Call("LoadingError", errorM);
			}

		}
	}

	public static void NativeLog(string log)
	{
		/*
		using (var javaUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (var currentActivity = javaUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				currentActivity.Call("LoadingError", errorM);
			}

		}
		*/
	}

	public static void TotalPoints(string points)
	{
		using (var javaUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (var currentActivity = javaUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				currentActivity.Call("TotalPoints", points);
			}

		}
	}
	public static void UpdatePoints(string points)
	{
		using (var javaUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (var currentActivity = javaUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				currentActivity.Call("UpdatePoints", points);
			}

		}
	}

	public static void SceneLoad()
	{
		using (var javaUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (var currentActivity = javaUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				currentActivity.Call("SceneLoad", "ChangeScene");
			}
		}
	}
	#endif

	public static void Exit()
	{
		//Clear Unused Textures
		Resources.UnloadUnusedAssets();

		Time.timeScale = 1;
        SceneManager.LoadScene("menu");
		//Application.LoadLevel("menu"); obsolete
	}
	void Awake()
	{

		DontDestroyOnLoad(transform.gameObject);
	}

	void Start()
	{
		//Resources.UnloadUnusedAssets();
	}

	public void TestSendString()
	{

		ChangeScene(Test_Json);
	}
	/// <summary>
	/// Save the variable to load the game mode in the scene
	/// </summary>
	/// <param name="value">num of the scene to be loaded</param>
	public void GameModeSelect(int value)
	{
		//Clear Unused Textures
		Resources.UnloadUnusedAssets();

		//Application.LoadLevel(value);
		//PlayerPrefs.SetString("nextLevel", value);
		//SceneManager.LoadScene("Loading");

        SceneManager.LoadScene(value);
        // Better way to change scenes
        // Appication.LoadLevel is obsolete

	}
	/// <summary>
	/// This method has expose to code ios for Changes the between scene.
	/// </summary>
	/// <param name="levelName">Level key name.</param>
	// public void StartData(string levelName)
	public void ChangeScene(string JsonString)
	{
		NativeLog(JsonString);
		//#if UNITY_EDITOR
		Debug.Log(JsonString);
		Data = JsonMapper.ToObject(JsonString.ToString());
		Debug.Log(Data["scene"]);
		string DateLv = Data["scene"].ToString();
		Debug.Log(DateLv);
		JsonString = DateLv;

		string gameId = Data["gameID"].ToString();
		string tournamentID = Data["tournamentID"].ToString();
		string updateTime = Data["updateTime"].ToString();
		// This parameter is used to build the connection string for Trivia Game
		//string isDebug = Data["isDebug"].ToString();
		string url = Data["url"].ToString();
		//string urlTrivia = Data["urlTrivia"].ToString();
		string token = Data["token"].ToString();;

		PlayerPrefs.SetString("gameID", gameId);
		PlayerPrefs.SetString("tournamentID", tournamentID);
		//PlayerPrefs.SetString("isDebug", isDebug);
		PlayerPrefs.SetString("updateTime", updateTime);
		PlayerPrefs.SetString("url", url + "api/v1/games/randomness");
		PlayerPrefs.SetString("urlTrivia", url + "game/");

		PlayerPrefs.SetString("token", token);

		int value = GetLevelID(JsonString);
		GameModeSelect(value);
		//#endif
		//#if !UNITY_EDITOR
		//string updateTime = "20";
		//PlayerPrefs.SetString("updateTime", updateTime);
		//int value = GetLevelID(JsonString);
		//GameModeSelect(value);
		//#endif
	}
    public void Unload(){
        Application.Unload();
    }

	public int GetLevelID(string levelName)
	{
		switch (levelName)
		{
		case "practice":
			PlayerPrefs.SetInt(MODE, 1);
			return 2;
		case "timer_mode":
			PlayerPrefs.SetInt(MODE, 2);
			return 2;
		case "candy_practice":
			PlayerPrefs.SetInt(MODE, 1);
			PlayerPrefs.SetString("GameToLoad", "game_Match3");
			return 3;
		case "candy_timer_mode":
			PlayerPrefs.SetInt(MODE, 2);
			PlayerPrefs.SetString("GameToLoad", "game_Match3");
			return 3;
		case "bubble_practice_mode":
			PlayerPrefs.SetInt(MODE, 1);
			PlayerPrefs.SetString("GameToLoad", "game_BubbleShooter");
			return 7;
		case "bubble_timer_mode":
			PlayerPrefs.SetInt(MODE, 2);
			PlayerPrefs.SetString("GameToLoad", "game_BubbleShooter");
			return 7;
		case "Tetris_practice_mode":
			PlayerPrefs.SetInt(MODE, 1);
			PlayerPrefs.SetString("GameToLoad", "game_Tetris");
			return 4;
		case "Tetris_timer_mode":
			PlayerPrefs.SetInt(MODE, 2);
			PlayerPrefs.SetString("GameToLoad", "game_Tetris");
			return 4;
		case "Beer_pong_practice_mode":
			PlayerPrefs.SetInt(MODE, 1);
			PlayerPrefs.SetString("GameToLoad", "Beer_pong");
			return 5;
		case "Beer_pong_timer_mode":
			PlayerPrefs.SetInt(MODE, 2);
			PlayerPrefs.SetString("GameToLoad", "Beer_pong");
			return 5;
		case "Trivia_practice_mode":
			PlayerPrefs.SetInt(MODE, 1);
			PlayerPrefs.SetString("GameToLoad", "GameTrivia");
			return 6;
		case "Trivia_timer_mode":
			PlayerPrefs.SetInt(MODE, 2);
			PlayerPrefs.SetString("GameToLoad", "GameTrivia");
			return 6;
		default:
			return 1;
		}
	}

	/// <summary>
	/// Go to the main menu.
	/// </summary>
	public void GoMainMenu()
	{
		//Clear Unused Textures
		Resources.UnloadUnusedAssets();

		PlayerPrefs.SetInt(MODE, -1);
		SceneManager.LoadScene("menu");
		//Application.LoadLevel("menu"); obsolete
	}

	//public void TotalScore ()
	//{

	//}
}
