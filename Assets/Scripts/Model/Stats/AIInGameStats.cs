using System;
using UnityEngine;
using UnityEngine.Serialization;
using Model.Stats;

namespace Model.Stats
{
    public class AIInGameStats : MonoBehaviour
    {
        [FormerlySerializedAs("robotBaseStat")] public AIBaseStatsCurve AIBaseStats;
        [Header("Base Stats")]
        public int Level;
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
            //Level = LevelManager.Level;
            maxHP = (int)AIBaseStats.baseHPCurve.Evaluate(Level);
            maxArmor = (int)AIBaseStats.basedArmorCurve.Evaluate(Level);
            maxFuel = (int)AIBaseStats.baseFuelCurve.Evaluate(Level);
            speed = AIBaseStats.baseSpeedCurve.Evaluate(Level);
            
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
