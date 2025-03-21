using UnityEngine.UI;
using UnityEngine;
using GameController;
using UnityEngine.EventSystems;

namespace View.Preparation {
    public class PreparationUIView : MonoBehaviour {
        [Header("Buttons")]
        public Button InBattleButton;
        public Button ModificationButton;
        public Button MainMenuButton;

        [Header("Buttons link to other screen")]
        public Button BuyResearchButton;
        public Button CloseResearchButton;

        [Header("Buttons link to other screen")]
        [SerializeField] ModificationUIView ModificationScreen;

        [Header("Event System")]
        public EventSystem eventSystem;

        void Start()
        {
            InBattleButton.onClick.AddListener(OnInBattleButtonClicked);
            ModificationButton.onClick.AddListener(OnModificationButtonClicked);
            MainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        }

        private void OnInBattleButtonClicked()
        {
            GameManager.Instance?.SaveGame();
            GameStateManager.Instance.SetNextPhase(GameState.InBattle);
            Debug.Log("In Battle Button Pressed.");
        }

        private void OnModificationButtonClicked()
        {
            //TODO : Open Modification UI;

            if (BuyResearchButton.interactable == false)
            {
                eventSystem.SetSelectedGameObject(
                CloseResearchButton.gameObject
            );
            }
            else {eventSystem.SetSelectedGameObject(
                BuyResearchButton.gameObject
            );}

            ModificationScreen.animationController.SetTrigger("ModificationOpen");
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
