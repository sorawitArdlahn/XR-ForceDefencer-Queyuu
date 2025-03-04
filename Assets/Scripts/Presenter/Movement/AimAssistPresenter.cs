using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controller.Movement;
using UnityEngine.UI;
using TMPro;

namespace Presenter.Movement
{
    public class AimAssistPresenter : MonoBehaviour
    {
        public AimAssistantCtrl aimAssistantCtrl;
        public int maxTargetHp;
        public int maxTargetArmor;
        public int currentTargetHp;
        public int currentTargetArmor;
        
        [Header("UI")]
        public Slider HpSlider;
        public Slider ArmorSlider;
        public TextMeshProUGUI targetDistanceText;
        public TextMeshProUGUI targetHpText;
        public TextMeshProUGUI targetArmorText;
        // Update is called once per frame
        void Update()
        {
            targetDistanceText.text = aimAssistantCtrl.targetDistance.ToString("F1") + "m";
            
            if (ReadTargetStats())
            {
                HpSlider.maxValue = maxTargetHp;
                ArmorSlider.maxValue = maxTargetArmor;
                HpSlider.value = currentTargetHp;
                ArmorSlider.value = currentTargetArmor;
                targetHpText.text = currentTargetHp.ToString();
                targetArmorText.text = currentTargetArmor.ToString();
            }
        }

        private bool ReadTargetStats()
        {
            if (aimAssistantCtrl.target != null)
            {
                maxTargetHp = aimAssistantCtrl.target.GetComponent<SimpleBlackboard>().maxHealth;
                maxTargetArmor = aimAssistantCtrl.target.GetComponent<SimpleBlackboard>().maxArmor;
                currentTargetHp = aimAssistantCtrl.target.GetComponent<SimpleBlackboard>().currentHealth;
                currentTargetArmor = aimAssistantCtrl.target.GetComponent<SimpleBlackboard>().currentArmor;
                return true;
            }else{
                maxTargetHp = 0;
                maxTargetArmor = 0;
                currentTargetHp = 0;
                currentTargetArmor = 0;
                return false;
            }
        }

    }
}

