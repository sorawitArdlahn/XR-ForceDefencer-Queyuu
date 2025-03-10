using UnityEngine;
using Model.Stats;
using GameController;

namespace Controller.StatMod
{
    public enum PlayerUpgradable {
        HealthPoint,
        Armor,
        Fuel,
        MovementSpeed
    }
    public class PlayerStatModifierController : MonoBehaviour
    {
        PlayerData playerInfo;

        void Awake()
        {
            playerInfo = GameManager.Instance.currentGameData.playerData;
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
                case PlayerUpgradable.MovementSpeed:
                    playerInfo.MovementSpeedMultiplier += playerStatModifier.Multiplier;
                    break;
            }

            playerInfo.currentResearchPoint -= playerInfo.researchPointRequired;
            playerInfo.researchPointRequired += 50;

            //TODO : Update UI
            //TODO : Call PlayerInfo Update

        }

        public PlayerStatModifier RandomStatModifier()
        {
            //which stat would be upgraded
            PlayerStatModifier statModifier = new PlayerStatModifier();
            statModifier.stat = (PlayerUpgradable)Random.Range(0, PlayerUpgradable.GetValues(typeof(PlayerUpgradable)).Length);

            //how much the stat would be upgraded
            float randomMultiplier = Random.Range(0.1f, 0.4f); // Adjust the range as needed
            //only 2 decimal points
            statModifier.Multiplier = Mathf.Round(randomMultiplier * 100f) / 100f;

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
    }
    
}
