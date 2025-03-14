using EventListener;
using UnityEngine;
using UnityEngine.UI;
using GameController;

namespace View.Exploration
{
public class FinishExplorationUIView : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button ContinueExplorationButton;
    [SerializeField] Button FinishExplorationButton;
    [SerializeField] GameEvent OnContinueExploration;
    [SerializeField] GameEvent OnFinishExploration;

    void Start()
    {
        ContinueExplorationButton.onClick.AddListener(OnContinueExplorationButtonClicked);
        FinishExplorationButton.onClick.AddListener(OnFinishExplorationButtonClicked);
    }

    private void OnContinueExplorationButtonClicked()
    {
        UnPauseGame();
        OnContinueExploration.Raise(this);
    }

    private void OnFinishExplorationButtonClicked()
    {
        UnPauseGame();
        OnFinishExploration.Raise(this);
        GameStateManager.Instance.SetNextPhase(GameState.BattlePreparation);
    }

    private void UnPauseGame(){
        Time.timeScale = 1;
    }
    
}
}
