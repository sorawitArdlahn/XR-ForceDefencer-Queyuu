using UnityEngine.UI;
using UnityEngine;
using GameController;
using UnityEngine.EventSystems;
using Audio;
using TMPro;

namespace View.Preparation {
    public class PreparationUIView : MonoBehaviour {
        [Header("==== Text ====")]
        public TextMeshProUGUI HighestLevelText;
        public TextMeshProUGUI CheckpointLevelText;
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

        [Header("==== Other UI Screen ====")]
        [SerializeField] StatShowUIView StatShowScreen;

        [Header("Event System")]
        public EventSystem eventSystem;

        [Header("==== Animation Controller ====")]
        public Animator animationController;

        void Start()
        {

            InBattleButton.onClick.AddListener(OnInBattleButtonClicked);
            ModificationButton.onClick.AddListener(OnModificationButtonClicked);
            MainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);

            HighestLevelText.text = GameManager.Instance?.currentGameData.levelData.highestLevel.ToString();
            CheckpointLevelText.text = GameManager.Instance?.currentGameData.levelData.checkpointLevel.ToString();

            eventSystem.SetSelectedGameObject(FirstButton.gameObject);
        }

        private void OnInBattleButtonClicked()
        {
            GameManager.Instance?.SaveGame();
            AudioManagerController.Instance.PlaySFX("ButtonPressed");
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

            StatShowScreen.UpdateText();
            ModificationScreen.animationController.SetTrigger("ModificationOpen");
            AudioManagerController.Instance.PlaySFX("ModificationBuyOpen");
            Debug.Log("Modification Button Pressed.");
        }

        private void OnMainMenuButtonClicked()
        {
            GameManager.Instance?.SaveGame();
            AudioManagerController.Instance.PlaySFX("ButtonPressed");
            GameStateManager.Instance.SetNextPhase(GameState.MainMenu);
            Debug.Log("Main Menu Button Pressed.");
        }

    }

}
