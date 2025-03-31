using UnityEngine;
using Model.Stats;
using GameController;
using EventListener;
using System.Collections.Generic;

namespace Controller.StatMod
{
    public enum PlayerUpgradable {
        HealthPoint,
        Armor,
        Fuel,
        Damage
    }
    public class PlayerStatModifierController : MonoBehaviour
    {
        PlayerData playerInfo;
        [Header("Events")]
        [SerializeField] GameEvent OnResearchUpgraded;

        [Header("==== Stat Icons ====")]
        [SerializeField] private Sprite healthPointIcon;
        [SerializeField] private Sprite armorIcon;
        [SerializeField] private Sprite fuelIcon;
        [SerializeField] private Sprite damageIcon;

        private Dictionary<PlayerUpgradable, Sprite> statIcons;


        void Awake()
        {
            playerInfo = GameManager.Instance?.currentGameData.playerData;

            statIcons = new Dictionary<PlayerUpgradable, Sprite>
            {
            { PlayerUpgradable.HealthPoint, healthPointIcon },
            { PlayerUpgradable.Armor, armorIcon },
            { PlayerUpgradable.Fuel, fuelIcon },
            { PlayerUpgradable.Damage, damageIcon }
            };
        }

        public void UpgradeStat(PlayerStatModifier playerStatModifier)
        {
            switch (playerStatModifier.stat) {
                case PlayerUpgradable.HealthPoint:
                    playerInfo.HealthPointMultiplier += playerStatModifier.Multiplier;
                    break;
                case PlayerUpgradable.Armor:
                    playerInfo.ArmorMultiplier += playerStatModifier.Multiplier;
                    break;
                case PlayerUpgradable.Fuel:
                    playerInfo.FuelMultiplier += playerStatModifier.Multiplier;
                    break;
                case PlayerUpgradable.Damage:
                    playerInfo.DamageMultiplier += playerStatModifier.Multiplier;
                    break;
            }

            playerInfo.currentResearchPoint -= playerInfo.researchPointRequired;
            playerInfo.researchPointRequired += 50;

            OnResearchUpgraded.Raise(this);

            //TODO : Update UI
            //TODO : Call PlayerInfo Update

        }

        public PlayerStatModifier RandomStatModifier()
        {
            //which stat would be upgraded
            PlayerStatModifier statModifier = new PlayerStatModifier();
            statModifier.stat = (PlayerUpgradable)Random.Range(0, PlayerUpgradable.GetValues(typeof(PlayerUpgradable)).Length);

            //how much the stat would be upgraded
            float randomMultiplier = Random.Range(0.1f, 0.35f); // Adjust the range as needed
            //only 2 decimal points
            statModifier.Multiplier = Mathf.Round(randomMultiplier * 100f) / 100f;

            statModifier.Icon = statIcons[statModifier.stat];

            return statModifier;
        }

        public int getCurrentResearchPoint() {
            return playerInfo.currentResearchPoint;
        }

        public int getResearchPointRequired() {
            return playerInfo.researchPointRequired;
        }
    }

    public struct PlayerStatModifier
    {
        public PlayerUpgradable stat;
        public float Multiplier;
        public Sprite Icon;
    }
    
}
