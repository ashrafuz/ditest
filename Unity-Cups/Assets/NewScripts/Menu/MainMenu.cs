using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using System.IO;
using LitJson;

public class MainMenu : MonoBehaviour
{
	public GameConfig TestConfig;

	public static void Exit()
	{
		//Clear Unused Textures
		Resources.UnloadUnusedAssets();

		Time.timeScale = 1;
        SceneManager.LoadScene(GameConstants.MENU_SCENE);
		//Application.LoadLevel("menu"); obsolete
	}

	public void TestSendString()
	{
		ChangeScene(TestConfig);
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
	/// <param name="gameConfig">Level key name.</param>
	// public void StartData(string levelName)
	public void ChangeScene(GameConfig gameConfig)
	{
		NativeLog(gameConfig.GetJSONString());
		Debug.Log(gameConfig.GetJSONString());

		PlayerPrefs.SetString(GameConstants.GAME_ID_KEY, gameConfig.gameID.ToString());
		PlayerPrefs.SetString(GameConstants.TOURNAMENT_ID_KEY, gameConfig.tournamentID.ToString());
		PlayerPrefs.SetString(GameConstants.DEBUG_KEY, gameConfig.debug);
		PlayerPrefs.SetString(GameConstants.UPDATE_TIME_KEY, gameConfig.updateTime.ToString());
		PlayerPrefs.SetString(GameConstants.URL_KEY, gameConfig.url + GameConstants.URL_END_POINT);
		PlayerPrefs.SetString(GameConstants.URL_TRIVIA_KEY, gameConfig.url + GameConstants.URL_TRIVIA_ENDPOINT);

		// ASSUMPTION: levelID / sceneID will be provided with the config,
		// I understand that remote config *shouldn't* know about scene indices,
		// but mapping string with switch cases are messy and more prone to errors
		// my suggestion would be, use gameID as a mapper.

		PlayerPrefs.SetString("token", gameConfig.token);
		PlayerPrefs.SetInt(GameConfig.GAME_MODE_KEY, gameConfig.mode);
		PlayerPrefs.SetString(GameConfig.GAME_TO_LOAD_KEY, gameConfig.gameToLoad);
		GameModeSelect(gameConfig.levelID);
	}
	
    public void Unload(){
        Application.Unload();
    }

    /// <summary>
	/// Go to the main menu.
	/// </summary>
	public void GoMainMenu()
	{
		//Clear Unused Textures
		Resources.UnloadUnusedAssets();

		PlayerPrefs.SetInt(GameConfig.GAME_MODE_KEY, -1);
		SceneManager.LoadScene(GameConstants.MENU_SCENE);
	}

	#region  NATIVE_LOG
#if UNITY_IOS

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public void TotalPoints(string points);
	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public void UpdatePoints(string points);
	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public void LoadingError(string errorM);
	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public void NativeLog(string log);

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

#elif UNITY_EDITOR
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
	
#elif UNITY_ANDROID

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
	#endregion
	
}
