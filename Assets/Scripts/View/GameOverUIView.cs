using GameController;
using UnityEngine;
using UnityEngine.UI;
using Audio;
using TMPro;
using Controller.Level;
using UnityEngine.EventSystems;

namespace View.Exploration {
    public class GameOverUIView : MonoBehaviour
    {
        [Header("==== Result Texts ====")]
        [SerializeField] TextMeshProUGUI HighestLevelText;
        [SerializeField] TextMeshProUGUI AccumulatedResearchPointText;

        [Header("==== Buttons ====")]
        [SerializeField] Button MainMenuButton;
        [SerializeField] Button NewGameButton;

        [Header("==== Level Data ====")]
        [SerializeField] LevelManagerController levelData;

        [Header("==== Animation Controller ====")]
        public Animator animationController;

        [Header("==== Event System ====")]
        public EventSystem eventSystem;
    

    void Start(){
            MainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
            NewGameButton.onClick.AddListener(OnNewGameButtonClicked);
    }

    public void UpdateText() {
        HighestLevelText.text = levelData.getHighestLevel().ToString();
        AccumulatedResearchPointText.text = levelData.getAccumulatedResearchPoint().ToString();
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

    public void GameOverUIAnimationFinish() {
        EventSystem.current.SetSelectedGameObject(MainMenuButton.gameObject);
        PauseGame();
    }

    private void UnPauseGame(){
        Time.timeScale = 1;
    }

    private void PauseGame(){
        Time.timeScale = 0;
    
}

}

}


