using System.Collections;
using System.Collections.Generic;
using Model.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presenter.Stats
{
    public class PlayerRobotCombatHUDStatusPresenter : MonoBehaviour
    {
        public RobotInGameStats robotInGameStats;
        public Image healthSlider;
        public TextMeshProUGUI healthValueText;
        public Image armorSlider;
        public TextMeshProUGUI armorValueText;
        public Image fuelSlider;
        public TextMeshProUGUI fuelValueText;
        // public Image fuelSlider;
        void Awake()
        {
            robotInGameStats.onHPChangedEvent += UpdateHealth;
            robotInGameStats.onArmorChangedEvent += UpdateArmor;
            robotInGameStats.onFuelChangedEvent += UpdateFuel;
        }

        private void UpdateHealth(int currentHP, int maxHP)
        {
            healthSlider.fillAmount = (float)currentHP / maxHP;
            healthValueText.text = $"{currentHP}";
        }

        private void UpdateArmor(int currentArmor, int maxArmor)
        {
            armorSlider.fillAmount = (float)currentArmor / maxArmor;
            armorValueText.text = $"{currentArmor}";
        }

        private void UpdateFuel(int currentFuel, int maxFuel)
        {
            fuelSlider.fillAmount = (float)currentFuel / maxFuel;
             fuelValueText.text = $"{currentFuel}";

        }
    }
}

