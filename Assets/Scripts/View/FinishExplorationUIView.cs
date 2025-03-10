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
        OnContinueExploration.Raise(this);
    }

    private void OnFinishExplorationButtonClicked()
    {
        OnFinishExploration.Raise(this);
        GameStateManager.Instance.SetNextPhase(GameState.BattlePreparation);
    }
    
}
}
