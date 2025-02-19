using System;
using UnityEngine;

namespace Model.Stats
{
    public class RobotInGameStat : MonoBehaviour
    {
        public RobotBaseStat robotBaseStat;
        [Header("Base Stats")]
        private int maxHP;
        private int maxArmor;
        private int maxFuel;
        private float speed;
        
        [Header("In-game Updatable Stats")]
        private int currentHP;
        private int currentArmor;
        private int currentFuel;
        
        public event Action<int> onHPChangedEvent; 
        public event Action<int> onArmorChangedEvent;
        public event Action<int> onFuelChangedEvent;

        private void Awake()
        {
            maxHP = robotBaseStat.baseHP;
            maxArmor = robotBaseStat.basedArmor;
            maxFuel = robotBaseStat.baseFuel;
            speed = robotBaseStat.baseSpeed;
        }
        
        
    }
}
