using System;
using GameController;
using UnityEngine;
using UnityEngine.UI;
using Audio;

namespace View.MainMenu
{   
public class MainMenuUIView : MonoBehaviour
{
    [Header("==== Buttons ====")]
    public Button NewGameButton;
    public Button LoadGameButton;
    public Button TutorialButton;
    public Button QuitGameButton;

    [Header("==== Save/Load Scripts ====")]
    public  GameManager gameManager = null;

    public void Start()
    {
        gameManager = GameManager.Instance;
        NewGameButton.onClick.AddListener(OnNewGameButtonClicked);
        LoadGameButton.onClick.AddListener(OnLoadGameButtonClicked);
        TutorialButton.onClick.AddListener(OnTutorialButtonClicked);
        QuitGameButton.onClick.AddListener(OnQuitGameButtonClicked);
    }

    private void OnNewGameButtonClicked()
    {
        gameManager.newGame();
        AudioManagerController.Instance.PlaySFX("ButtonPressed");
        GameStateManager.Instance.SetNextPhase(GameState.BattlePreparation);
        Debug.Log("New Game Button Pressed.");
    }

    private void OnLoadGameButtonClicked()
    {
        AudioManagerController.Instance.PlaySFX("ButtonPressed");
        try {
            gameManager.LoadGame();
            GameStateManager.Instance.SetNextPhase(GameState.BattlePreparation);
            Debug.Log("Save Loaded.");
        }
        catch (ArgumentException) {
            gameManager.newGame();
            Debug.LogWarning($"Save not found.");
        }
    }

    private void OnTutorialButtonClicked()
    {
        AudioManagerController.Instance.PlaySFX("ButtonPressed");
        GameStateManager.Instance.SetNextPhase(GameState.Tutorial);
        Debug.Log("Tutorial Button Pressed.");
    }

    private void OnQuitGameButtonClicked()
    {
        AudioManagerController.Instance.PlaySFX("ButtonPressed");
        Debug.Log("Quit Game Button Clicked");
        Application.Quit();
    }
}

}
