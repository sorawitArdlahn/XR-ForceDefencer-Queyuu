using Audio;
using Controller.StatMod;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace View.Preparation {
    public class ModificationUIView : MonoBehaviour
{
    [Header("==== Other UI Screen ====")]
    [SerializeField] StatShowUIView statShowScreen;
    [Header("==== This UI Buttons ====")]
    [SerializeField] Button CloseButton;
    [SerializeField] Button BuyModificationButton;

    [SerializeField] Button ModificationButton01;
    [SerializeField] Button ModificationButton02;
    [SerializeField] Button ModificationButton03;

    [Header("==== Buttons link to other screen ====")]
    [SerializeField] Button ModificationScreenButton;

    [Header("==== This Modification Screen ====")]

    [SerializeField] PlayerStatModifierController playerStatModifierController;
    [SerializeField] CanvasGroup modificationCanvasGroup;

    [Header("==== Modification Images ====")]
    [SerializeField] Image ModificationImage01;
    [SerializeField] Image ModificationImage02;
    [SerializeField] Image ModificationImage03;

    [Header("==== Modification Texts ====")]

    //Description of the Stat Modification
    [SerializeField] TextMeshProUGUI ModificationText01;
    [SerializeField] TextMeshProUGUI ModificationText02;
    [SerializeField] TextMeshProUGUI ModificationText03;

    [Header("==== Research Points Texts ====")]

    [SerializeField] TextMeshProUGUI RequiredResearchPointText;
    [SerializeField] TextMeshProUGUI CurrentResearchPointText;

    [Header("==== Event System ====")]
    public EventSystem eventSystem;

    //Modification Randomed
    PlayerStatModifier Modification01;
    PlayerStatModifier Modification02;
    PlayerStatModifier Modification03;

    //Animation Controller;
    [Header("==== Animation Controller ====")]
    public Animator animationController;

    [Header("==== UI Mask ====")]
    public Sprite UIMask;

    void Start()
    {

        ModificationButton01.onClick.AddListener(OnModificationButton01Clicked);
        ModificationButton02.onClick.AddListener(OnModificationButton02Clicked);
        ModificationButton03.onClick.AddListener(OnModificationButton03Clicked);
        BuyModificationButton.onClick.AddListener(OnBuyModificationButtonClicked);
        CloseButton.onClick.AddListener(OnCloseButtonClicked);

        ResetModificationButtons();
    }

    private void OnCloseButtonClicked()
    {
        eventSystem.SetSelectedGameObject(
            ModificationScreenButton.gameObject
        );
        AudioManagerController.Instance.PlaySFX("ModificationClose");
        animationController.SetTrigger("ModificationClose");
    }

    private void OnBuyModificationButtonClicked()
    {
        //Random Modification Choice
        Modification01 = playerStatModifierController.RandomStatModifier();
        Modification02 = playerStatModifierController.RandomStatModifier();
        Modification03 = playerStatModifierController.RandomStatModifier();

        ModificationButton01.interactable = true;
        ModificationButton02.interactable = true;
        ModificationButton03.interactable = true;

        BuyModificationButton.interactable = false;

        ModificationImage01.sprite = Modification01.Icon;
        ModificationImage02.sprite = Modification02.Icon;
        ModificationImage03.sprite = Modification03.Icon;

        ModificationText01.text = ((int)(Modification01.Multiplier * 100)).ToString() + "%";
        ModificationText02.text = ((int)(Modification02.Multiplier * 100)).ToString() + "%";
        ModificationText03.text = ((int)(Modification03.Multiplier * 100)).ToString() + "%";

        AudioManagerController.Instance.PlaySFX("ModificationRandom");
        eventSystem.SetSelectedGameObject(
            ModificationButton01.gameObject
        );

    }

    void ResetModificationButtons()
    {

        ModificationText01.text = "Mod_01";
        ModificationText02.text = "Mod_02";
        ModificationText03.text = "Mod_03";

        ModificationImage01.sprite = UIMask;
        ModificationImage02.sprite = UIMask;
        ModificationImage03.sprite = UIMask;

        ModificationButton01.interactable = false;
        ModificationButton02.interactable = false;
        ModificationButton03.interactable = false;

        BuyModificationButton.interactable = false;

        if (modificationCanvasGroup.interactable) {
            if (BuyModificationButton.interactable == false) {
            eventSystem.SetSelectedGameObject(
            CloseButton.gameObject
            );
            }
            else {
            eventSystem.SetSelectedGameObject(
            BuyModificationButton.gameObject
            );
            }
        }

        UpdateResearchPoint();
        statShowScreen.UpdateText();
    }



    private void OnModificationButton03Clicked()
    {
        AudioManagerController.Instance.PlaySFX("ModificationBuy");
        playerStatModifierController.UpgradeStat(Modification03);
        ResetModificationButtons();
        
    }

    private void OnModificationButton02Clicked()
    {
        AudioManagerController.Instance.PlaySFX("ModificationBuy");
        playerStatModifierController.UpgradeStat(Modification02);
        ResetModificationButtons();
    }

    private void OnModificationButton01Clicked()
    {
        AudioManagerController.Instance.PlaySFX("ModificationBuy");
        playerStatModifierController.UpgradeStat(Modification01);
        ResetModificationButtons();
    }

    void UpdateResearchPoint()
    {
        CurrentResearchPointText.text = playerStatModifierController.getCurrentResearchPoint().ToString();
        RequiredResearchPointText.text = playerStatModifierController.getResearchPointRequired().ToString();

        if (playerStatModifierController.getCurrentResearchPoint() < 
        playerStatModifierController.getResearchPointRequired())
        {
            BuyModificationButton.interactable = false;
        }
        else
        {
            BuyModificationButton.interactable = true;
        }
    }

    


}

}
