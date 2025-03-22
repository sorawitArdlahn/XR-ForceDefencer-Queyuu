using EventListener;
using UnityEngine;
using UnityEngine.UI;
using GameController;
using Audio;

namespace View.Exploration
{
public class FinishExplorationUIView : MonoBehaviour
{
    [Header("==== Texts ====")]
    [SerializeField] Text ExplorationResultText;
    [SerializeField] Text ResearchPointText;
    [Header("==== Buttons ====")]
    [SerializeField] Button ContinueExplorationButton;
    [SerializeField] Button FinishExplorationButton;

    [Header("==== Game Events ====")]
    [SerializeField] GameEvent OnContinueExploration;
    [SerializeField] GameEvent OnFinishExploration;

    [Header("==== Animation Controller ====")]
    public Animator animationController;

    void Start()
    {
        ContinueExplorationButton.onClick.AddListener(OnContinueExplorationButtonClicked);
        FinishExplorationButton.onClick.AddListener(OnFinishExplorationButtonClicked);
    }

    private void OnContinueExplorationButtonClicked()
    {
        UnPauseGame();
        AudioManagerController.Instance.PlaySFX("ButtonPressed");
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
    
}
}
