using UnityEngine;
using Model.Stats;

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
        RobotInGameStats playerInfo;

        void Awake()
        {
            playerInfo = FindObjectOfType<RobotInGameStats>();
        }

        public void UpgradeStat(PlayerStatModifier playerStatModifier)
        {
            switch (playerStatModifier.stat) {
                case PlayerUpgradable.HealthPoint:
                    playerInfo.data.HealthPointMultiplier += playerStatModifier.Multiplier;
                    break;
                case PlayerUpgradable.Armor:
                    playerInfo.data.ArmorMultiplier += playerStatModifier.Multiplier;
                    break;
                case PlayerUpgradable.Fuel:
                    playerInfo.data.FuelMultiplier += playerStatModifier.Multiplier;
                    break;
                case PlayerUpgradable.MovementSpeed:
                    playerInfo.data.MovementSpeedMultiplier += playerStatModifier.Multiplier;
                    break;
            }

            playerInfo.data.currentResearchPoint -= playerInfo.data.researchPointRequired;
            playerInfo.data.researchPointRequired += 50;

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
    }

    public struct PlayerStatModifier
    {
        public PlayerUpgradable stat;
        public float Multiplier;
    }
    
}
