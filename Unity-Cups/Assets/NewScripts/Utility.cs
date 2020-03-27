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



public struct GameConstants
{
    public const string MENU_SCENE = "menu";
    public const string BEER_PONG_SCENE_NAME = "Beer Pong";
}