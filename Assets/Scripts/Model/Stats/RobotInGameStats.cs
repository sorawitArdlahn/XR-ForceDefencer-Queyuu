using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Model.Stats
{
    public class RobotInGameStats : MonoBehaviour
    {
        [FormerlySerializedAs("robotBaseStat")] public RobotBaseStats robotBaseStats;
        [Header("Base Stats")]
        private int maxHP;
        private int maxArmor;
        private int maxFuel;
        private float speed;
        
        [Header("In-game Updatable Stats")]
        public int currentHP;
        public int currentArmor;
        public int currentFuel;
        
        public event Action<int,int> onHPChangedEvent; 
        public event Action<int,int> onArmorChangedEvent;
        public event Action<int,int> onFuelChangedEvent;

        private void Awake()
        {
            maxHP = robotBaseStats.baseHP;
            maxArmor = robotBaseStats.basedArmor;
            maxFuel = robotBaseStats.baseFuel;
            speed = robotBaseStats.baseSpeed;
            
            Debug.Log("maxHP: " + maxHP);
            Debug.Log("basedArmor: " + robotBaseStats.basedArmor);
            currentHP = maxHP;
            currentArmor = maxArmor;
            currentFuel = maxFuel;
        }

        public void SetCurrentHP(int newHP)
        {
            currentHP = newHP;
            onHPChangedEvent?.Invoke(currentHP, maxHP);
        }

        public void setCurrentArmor(int newArmor)
        {
            currentArmor = newArmor;
            onArmorChangedEvent?.Invoke(currentArmor, maxArmor);
        }

        public void setCurrentFuel(int newFuel)
        {
            currentFuel = newFuel;
            onFuelChangedEvent?.Invoke(currentFuel, maxFuel);
        }

        public int MaxFuel => maxFuel;
    }
}
