using EventListener;
using UnityEngine;
using UnityEngine.UI;
using GameController;
using Audio;
using Controller.Level;
using TMPro;
using UnityEngine.EventSystems;

namespace View.Exploration
{
public class FinishExplorationUIView : MonoBehaviour
{
    [Header("==== Texts ====")]
    [SerializeField] TextMeshProUGUI CurrentLevelText;
    [SerializeField] TextMeshProUGUI EnemyDefeatedText;
    [SerializeField] TextMeshProUGUI ResearchPointText;
    [Header("==== Buttons ====")]
    [SerializeField] Button ContinueExplorationButton;
    [SerializeField] Button FinishExplorationButton;

    [Header("==== Game Events ====")]
    [SerializeField] GameEvent OnContinueExploration;
    [SerializeField] GameEvent OnFinishExploration;

    [Header("==== Level Data ====")]
    //SOUP : Change to LevelData type
    [SerializeField] LevelManagerController levelData;

    [Header("==== Animation Controller ====")]
    public Animator animationController;

    [Header("==== Event System ====")]
    public EventSystem eventSystem;

    void Start()
    {
        ContinueExplorationButton.onClick.AddListener(OnContinueExplorationButtonClicked);
        FinishExplorationButton.onClick.AddListener(OnFinishExplorationButtonClicked);
    }

    public void UpdateText() {
        CurrentLevelText.text = (levelData.getCurrentLevel() + 1).ToString();
        EnemyDefeatedText.text = levelData.getTotalEnemies().ToString();
        ResearchPointText.text = (levelData.getTotalEnemies() * 75).ToString();
    }

    private void OnContinueExplorationButtonClicked()
    {
        UnPauseGame();
        AudioManagerController.Instance.PlaySFX("ButtonPressed");
        animationController.SetTrigger("FinishExplorationClose");

        if (GameStateManager.Instance != null) 
        {GameStateManager.Instance.SetNextPhase(GameState.InBattle);}
        else { Debug.Log("GameStateManager is null"); }

        OnContinueExploration.Raise(this);
    }

    private void OnFinishExplorationButtonClicked()
    {
        UnPauseGame();
        AudioManagerController.Instance.PlaySFX("ButtonPressed");
        OnFinishExploration.Raise(this);
        GameStateManager.Instance.SetNextPhase(GameState.BattlePreparation);
    }

    private void UnPauseGame(){
        Time.timeScale = 1;
    }

    public void AnimationFinishExplorationOpenFinish(){
        eventSystem.enabled = true;
        eventSystem.SetSelectedGameObject(ContinueExplorationButton.gameObject);
        PauseGame();
    }

    private void PauseGame(){
        Time.timeScale = 0;
    }
    
}
}
