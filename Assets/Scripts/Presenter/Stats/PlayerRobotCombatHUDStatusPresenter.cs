using System.Collections;
using System.Collections.Generic;
using Model.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace Presenter.Stats
{
    public class PlayerRobotCombatHUDStatusPresenter : MonoBehaviour
    {
        public RobotInGameStats robotInGameStats;
        public Image healthSlider;
        public Image armorSlider;
        // public Image fuelSlider;
        void Awake()
        {
            robotInGameStats.onHPChangedEvent += UpdateHealth;
            robotInGameStats.onArmorChangedEvent += UpdateArmor;
            // robotInGameStats.onFuelChangedEvent += UpdateFuel;
        }

        private void UpdateHealth(int currrntHP, int maxHP)
        {
            healthSlider.fillAmount = (float)currrntHP / maxHP;
        }

        private void UpdateArmor(int currentArmor, int maxArmor)
        {
            armorSlider.fillAmount = (float)currentArmor / maxArmor;
        }

        private void UpdateFuel(int currentFuel, int maxFuel)
        {
            // fuelSlider.fillAmount = (float)currentFuel / maxFuel;
        }
    }
}

