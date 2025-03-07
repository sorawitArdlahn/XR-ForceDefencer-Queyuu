using System.Collections;
using System.Collections.Generic;
using GameController;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        Debug.Log("New Game Button Clicked");
        SceneManager.LoadScene("PreparationScene");
    }

    private void OnLoadGameButtonClicked()
    {
        Debug.Log("Load Game Button Clicked");
        gameManager.LoadGame();
    }

    private void OnQuitGameButtonClicked()
    {
        Debug.Log("Quit Game Button Clicked");
        Application.Quit();
    }
}
