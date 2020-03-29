using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameConfig
{
    //should follow a common format (CamelCase or under_score, but since its a KEY and shouldn't be modified, i kept it as it was)
    public const string GAME_TO_LOAD_KEY = "GameToLoad";
    public const string GAME_MODE_KEY = "game_mode";
	
    public int gameID = 0;
    public int tournamentID = 0;
    public int updateTime = 20;
	
    public string debug = "Debug";
    public string url = "";
    public string token = "";
	
    [Space] [Header("Scene Info")]
    public string scene = "";
    public int levelID = -1;
    public string gameToLoad = "";
    public int mode = 0;
    public string GetJSONString()
    {
        return  JsonUtility.ToJson(this);
    }
}

//PlayerPrefs.SetString("gameID", gameConfig.gameID.ToString());
//PlayerPrefs.SetString("tournamentID", gameConfig.tournamentID.ToString());
//PlayerPrefs.SetString("isDebug", gameConfig.debug);
//PlayerPrefs.SetString("updateTime", gameConfig.updateTime.ToString());
//PlayerPrefs.SetString("url", gameConfig.url + "api/v1/games/randomness");
//PlayerPrefs.SetString("urlTrivia", gameConfig.url + "game/");

public struct GameConstants
{
    public const string MENU_SCENE = "menu";
    public const string BEER_PONG_SCENE_NAME = "Beer Pong";
    public const string SINGLE_PONG_TAG = "Beer_pong";
    public const string DOUBLE_PONG_TAG = "Doble_Pong";

    public const string TOURNAMENT_ID_KEY = "tournamentID";
    public const string GAME_ID_KEY = "gameID";
    public const string DEBUG_KEY = "isDebug";
    public const string UPDATE_TIME_KEY = "updateTime";
    public const string URL_KEY = "url";
    public const string URL_TRIVIA_KEY = "urlTrivia";
    
    public const string URL_END_POINT = "api/v1/games/randomness";
    public const string URL_TRIVIA_ENDPOINT = "game/";
}