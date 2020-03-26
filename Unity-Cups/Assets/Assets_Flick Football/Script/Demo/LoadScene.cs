using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

    public void LoadSceneName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        //Application.LoadLevel(sceneName);
    }
}
