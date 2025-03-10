using GameController;
using UnityEngine;
using UnityEngine.UI;

namespace View.Exploration {
    public class GameOverUIView : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] Button MainMenuButton;
        [SerializeField] Button NewGameButton;
    

    void Start(){
            MainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
            NewGameButton.onClick.AddListener(OnNewGameButtonClicked);
    }

    private void OnMainMenuButtonClicked()
    {
        GameManager.Instance?.DeleteGame();
        GameStateManager.Instance.SetNextPhase(GameState.MainMenu);
        Debug.Log("Main Menu Button Pressed.");
    }

    private void OnNewGameButtonClicked()
    {
        GameManager.Instance?.DeleteGame();
        GameManager.Instance?.newGame();
        GameStateManager.Instance.SetNextPhase(GameState.BattlePreparation);
        Debug.Log("New Game Button Pressed.");
    }

    
}

}


