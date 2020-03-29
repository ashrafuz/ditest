using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		//Clear Unused Textures
		Resources.UnloadUnusedAssets();

        SceneManager.LoadScene(GameConstants.MENU_SCENE);
		//Application.LoadLevel("menu"); obsolete
	}

}
