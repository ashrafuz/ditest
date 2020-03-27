
using UnityEngine;
using  UnityEngine.UI;

public class GameConfigButton : MonoBehaviour
{
    public GameConfig Config;
    public MainMenu Menu;
    private Button _gameButton;
    
    void Start()
    {
        _gameButton = GetComponent<Button>();
        if (_gameButton == null) {
            Debug.LogWarning("Button not found in game config button mono behaviour");
            return;
        }
        _gameButton.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        Menu?.ChangeScene(Config);
    }
}
