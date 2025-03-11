using System;
using UnityEngine;
using UnityEngine.Serialization;
using System.Persistence;

namespace Model.Stats
{
    public class RobotInGameStats : MonoBehaviour, IBind<PlayerData>
    {
        [FormerlySerializedAs("robotBaseStat")] public RobotBaseStats robotBaseStats;
        [Header("Base Stats")]
        public int maxHP;
        public int maxArmor;
        public int maxFuel;
        public float speed;
        
        [Header("In-game Updatable Stats")]
        public int currentHP;
        public int currentArmor;
        public int currentFuel;
        
        public event Action<int,int> onHPChangedEvent; 
        public event Action<int,int> onArmorChangedEvent;
        public event Action<int,int> onFuelChangedEvent;


        //SOUP : Binding Part
        [field: SerializeField] public SerializableGuid Id { get; set; } = SerializableGuid.NewGuid();
        public PlayerData data;

        public void Bind(PlayerData data)
        {
            this.data = data;
            this.data.Id = Id;

            //Stats
            this.data.HealthPointMultiplier = data.HealthPointMultiplier;
            this.data.ArmorMultiplier = data.ArmorMultiplier;
            this.data.FuelMultiplier = data.FuelMultiplier;
            this.data.MovementSpeedMultiplier = data.MovementSpeedMultiplier;

            //ResearchPoint
            this.data.currentResearchPoint = data.currentResearchPoint;
            this.data.researchPointRequired = data.researchPointRequired;

        }


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

            onHPChangedEvent?.Invoke(currentHP, maxHP);
            onArmorChangedEvent?.Invoke(currentArmor, maxArmor);
            onFuelChangedEvent?.Invoke(currentFuel, maxFuel);
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
