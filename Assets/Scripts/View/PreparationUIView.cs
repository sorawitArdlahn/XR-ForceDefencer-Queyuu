using UnityEngine.UI;
using UnityEngine;
using GameController;

namespace View.Preparation {
    public class PreparationUIView : MonoBehaviour {
        [Header("Buttons")]
        public Button InBattleButton;
        public Button ModificationButton;
        public Button MainMenuButton;

        [Header("Modification Screen")]
        [SerializeField] GameObject ModificationScreen;

        void Start()
        {
            InBattleButton.onClick.AddListener(OnInBattleButtonClicked);
            ModificationButton.onClick.AddListener(OnModificationButtonClicked);
            MainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        }

        private void OnInBattleButtonClicked()
        {
            //GameStateManager.Instance.SetNextPhase(GameState.InBattle);
            Debug.Log("In Battle Button Pressed.");
        }

        private void OnModificationButtonClicked()
        {
            //TODO : Open Modification UI
            ModificationScreen.SetActive(true);
            Debug.Log("Modification Button Pressed.");
        }

        private void OnMainMenuButtonClicked()
        {
            GameManager.Instance?.SaveGame();
            GameStateManager.Instance.SetNextPhase(GameState.MainMenu);
            Debug.Log("Main Menu Button Pressed.");
        }

    }

}
