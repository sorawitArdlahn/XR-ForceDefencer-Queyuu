using System;
using UnityEngine;
using UnityEngine.Serialization;
using Utils.Persistence;
using Controller.Level;

namespace Model.Stats
{
    public class RobotInGameStats : MonoBehaviour, IBind<PlayerData>
    {
        public CharacterType characterType;
        public RobotBaseStats PlayerBaseStats = null;
        public AIBaseStatsCurve aIBaseStatsCurve = null;
        public int currentLevel = 0;

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
        [SerializeField] private PersistentId _persistentId;
        public string Id => _persistentId.Id;
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

            OnResearchUpgraded();
        }

        public void OnResearchUpgraded() {
            
            maxHP = (int)(PlayerBaseStats.baseHP * data.HealthPointMultiplier);
            maxArmor = (int)(PlayerBaseStats.basedArmor * data.ArmorMultiplier);
            maxFuel = (int)(PlayerBaseStats.baseFuel * data.FuelMultiplier);
            speed = PlayerBaseStats.baseSpeed * data.MovementSpeedMultiplier;

            currentHP = maxHP;
            currentArmor = maxArmor;
            currentFuel = maxFuel;
            
            onHPChangedEvent?.Invoke(currentHP, maxHP);
            onArmorChangedEvent?.Invoke(currentArmor, maxArmor);
            onFuelChangedEvent?.Invoke(currentFuel, maxFuel);
        }


        private void Awake()
        {
            if(_persistentId == null)
            _persistentId = gameObject.AddComponent<PersistentId>();

            currentLevel = FindObjectOfType<LevelManagerController>().getCurrentLevel();

            if (characterType == CharacterType.player && PlayerBaseStats){
                maxHP = PlayerBaseStats.baseHP;
                maxArmor = PlayerBaseStats.basedArmor;
                maxFuel = PlayerBaseStats.baseFuel;
                speed = PlayerBaseStats.baseSpeed;
            }
            else if (characterType == CharacterType.AI && aIBaseStatsCurve){
                maxHP = (int)aIBaseStatsCurve.baseHPCurve.Evaluate(currentLevel);
                maxArmor = (int)aIBaseStatsCurve.basedArmorCurve.Evaluate(currentLevel);
                maxFuel = (int)aIBaseStatsCurve.baseFuelCurve.Evaluate(currentLevel);
                speed = (int)aIBaseStatsCurve.baseSpeedCurve.Evaluate(currentLevel);
            }
            else{
                Debug.LogError("From <RobotInGameStats> PlayerBaseStats or AIBaseStatsCurve is null");
            }
            
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

public enum CharacterType{
player, AI
}
