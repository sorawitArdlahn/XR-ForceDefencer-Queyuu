using System;
using Controller.StatMod;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace View.Preparation {
    public class ModificationUIView : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button CloseButton;
    [SerializeField] Button BuyModificationButton;

    [SerializeField] Button ModificationButton01;
    [SerializeField] Button ModificationButton02;
    [SerializeField] Button ModificationButton03;

    [Header("Buttons link to other screen")]
    [SerializeField] Button ModificationScreenButton;

    [Header("This Modification Screen")]

    [SerializeField] PlayerStatModifierController playerStatModifierController;

    [Header("Modification Texts")]

    //Description of the Stat Modification
    [SerializeField] TextMeshProUGUI ModificationText01;
    [SerializeField] TextMeshProUGUI ModificationText02;
    [SerializeField] TextMeshProUGUI ModificationText03;

    [Header("Research Points Texts")]

    [SerializeField] TextMeshProUGUI RequiredResearchPointText;
    [SerializeField] TextMeshProUGUI CurrentResearchPointText;

    [Header("Event System")]
    public EventSystem eventSystem;

    //Modification Randomed
    PlayerStatModifier Modification01;
    PlayerStatModifier Modification02;
    PlayerStatModifier Modification03;

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
        gameObject.SetActive(false);
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

        ModificationText01.text = Modification01.stat.ToString() + "\n" + Modification01.Multiplier.ToString();
        ModificationText02.text = Modification02.stat.ToString() + "\n" + Modification02.Multiplier.ToString();
        ModificationText03.text = Modification03.stat.ToString() + "\n" + Modification03.Multiplier.ToString();

        eventSystem.SetSelectedGameObject(
            ModificationButton01.gameObject
        );

    }

    void ResetModificationButtons()
    {

        ModificationText01.text = "";
        ModificationText02.text = "";
        ModificationText03.text = "";

        ModificationButton01.interactable = false;
        ModificationButton02.interactable = false;
        ModificationButton03.interactable = false;

        eventSystem.SetSelectedGameObject(
            BuyModificationButton.gameObject
        );

        UpdateResearchPoint();
    }



    private void OnModificationButton03Clicked()
    {
        playerStatModifierController.UpgradeStat(Modification03);
        ResetModificationButtons();
        
    }

    private void OnModificationButton02Clicked()
    {
        playerStatModifierController.UpgradeStat(Modification02);
        ResetModificationButtons();
    }

    private void OnModificationButton01Clicked()
    {
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
