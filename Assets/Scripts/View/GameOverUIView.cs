using GameController;
using UnityEngine;
using UnityEngine.UI;
using Audio;

namespace View.Exploration {
    public class GameOverUIView : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] Button MainMenuButton;
        [SerializeField] Button NewGameButton;

        [Header("Animation Controller")]
        public Animator animationController;
    

    void Start(){
            MainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
            NewGameButton.onClick.AddListener(OnNewGameButtonClicked);
    }

    private void OnMainMenuButtonClicked()
    {
        UnPauseGame();
        GameManager.Instance?.DeleteGame();
        AudioManagerController.Instance.PlaySFX("ButtonPressed");
        GameStateManager.Instance.SetNextPhase(GameState.MainMenu);
        Debug.Log("Main Menu Button Pressed.");
    }

    private void OnNewGameButtonClicked()
    {
        UnPauseGame();
        GameManager.Instance?.DeleteGame();
        GameManager.Instance?.newGame();
        AudioManagerController.Instance.PlaySFX("ButtonPressed");
        GameStateManager.Instance.SetNextPhase(GameState.BattlePreparation);
        Debug.Log("New Game Button Pressed.");
    }

    private void UnPauseGame(){
        Time.timeScale = 1;
    }
    
}

}


