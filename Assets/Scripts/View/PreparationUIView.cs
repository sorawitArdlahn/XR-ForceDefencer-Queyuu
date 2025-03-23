using UnityEngine.UI;
using UnityEngine;
using GameController;
using UnityEngine.EventSystems;
using Audio;

namespace View.Preparation {
    public class PreparationUIView : MonoBehaviour {
        [Header("Buttons")]
        public Button InBattleButton;
        public Button ModificationButton;
        public Button MainMenuButton;

        [Header("Initialize Button")]
        public Button FirstButton;

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

            eventSystem.SetSelectedGameObject(FirstButton.gameObject);
        }

        private void OnInBattleButtonClicked()
        {
            GameManager.Instance?.SaveGame();
            AudioManagerController.Instance.PlaySFX("TransitionScreenOut");
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
            AudioManagerController.Instance.PlaySFX("ModificationBuyOpen");
            Debug.Log("Modification Button Pressed.");
        }

        private void OnMainMenuButtonClicked()
        {
            GameManager.Instance?.SaveGame();
            AudioManagerController.Instance.PlaySFX("TransitionScreenOut");
            GameStateManager.Instance.SetNextPhase(GameState.MainMenu);
            Debug.Log("Main Menu Button Pressed.");
        }

    }

}
