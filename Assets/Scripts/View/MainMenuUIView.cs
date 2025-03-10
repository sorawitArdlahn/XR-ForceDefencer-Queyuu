using System;
using GameController;
using UnityEngine;
using UnityEngine.UI;

namespace View.MainMenu
{   
public class MainMenuUIView : MonoBehaviour
{
    [Header("Buttons")]
    public Button NewGameButton;
    public Button LoadGameButton;
    public Button QuitGameButton;

    [Header("Save/Load Scripts")]
    public  GameManager gameManager = null;

    public void Start()
    {
        gameManager = GameManager.Instance;
        NewGameButton.onClick.AddListener(OnNewGameButtonClicked);
        LoadGameButton.onClick.AddListener(OnLoadGameButtonClicked);
        QuitGameButton.onClick.AddListener(OnQuitGameButtonClicked);
    }

    private void OnNewGameButtonClicked()
    {
        gameManager.newGame();
        GameStateManager.Instance.SetNextPhase(GameState.BattlePreparation);
        Debug.Log("New Game Button Pressed.");
    }

    private void OnLoadGameButtonClicked()
    {
        try {
            gameManager.LoadGame();
            GameStateManager.Instance.SetNextPhase(GameState.BattlePreparation);
            Debug.Log("Save Loaded.");
        }
        catch (ArgumentException) {
            Debug.LogWarning($"Save not found.");
        }
    }

    private void OnQuitGameButtonClicked()
    {
        Debug.Log("Quit Game Button Clicked");
        Application.Quit();
    }
}

}
