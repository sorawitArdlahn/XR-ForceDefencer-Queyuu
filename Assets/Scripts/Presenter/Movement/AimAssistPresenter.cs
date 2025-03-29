using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controller.Movement;
using UnityEngine.UI;
using TMPro;
using Model.Stats;
using Controller.Stats;

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
        public Image HpSlider;
        public Image ArmorSlider;
        public TextMeshProUGUI targetDistanceText;
        public TextMeshProUGUI targetHpText;
        public TextMeshProUGUI targetArmorText;
        // Update is called once per frame

        void Start()
        {
            HpSlider.transform.parent.gameObject.SetActive(false);
            ArmorSlider.transform.parent.gameObject.SetActive(false);
            targetDistanceText.transform.parent.gameObject.SetActive(false);
        }
        void Update()
        {
            targetDistanceText.text = aimAssistantCtrl.targetDistance.ToString("F1") + "M";
            
            if (ReadTargetStats())
            {
                // HpSlider.maxValue = maxTargetHp;
                // ArmorSlider.maxValue = maxTargetArmor;

                // HpSlider.value = currentTargetHp;
                // ArmorSlider.value = currentTargetArmor;

                HpSlider.fillAmount = (float)currentTargetHp / maxTargetHp;
                ArmorSlider.fillAmount = (float)currentTargetArmor / maxTargetArmor;

                targetHpText.text = currentTargetHp.ToString();
                targetArmorText.text = currentTargetArmor.ToString();
            }
        }

        private bool ReadTargetStats()
        {
            if (aimAssistantCtrl.target != null)
            {
                HpSlider.transform.parent.gameObject.SetActive(true);
                ArmorSlider.transform.parent.gameObject.SetActive(true);
                targetDistanceText.transform.parent.gameObject.SetActive(true);
                
                HpSlider.gameObject.SetActive(true);
                ArmorSlider.gameObject.SetActive(true);
                maxTargetHp = aimAssistantCtrl.target.GetComponent<RobotInGameStats>().maxHP;
                maxTargetArmor = aimAssistantCtrl.target.GetComponent<RobotInGameStats>().maxArmor;
                currentTargetHp = aimAssistantCtrl.target.GetComponent<RobotInGameStats>().currentHP;
                currentTargetArmor = aimAssistantCtrl.target.GetComponent<RobotInGameStats>().currentArmor;
                return true;
            }else{

                HpSlider.transform.parent.gameObject.SetActive(false);
                ArmorSlider.transform.parent.gameObject.SetActive(false);
                targetDistanceText.transform.parent.gameObject.SetActive(false);
                maxTargetHp = 0;
                maxTargetArmor = 0;
                currentTargetHp = 0;
                currentTargetArmor = 0;
                return false;
            }
        }

    }
}

