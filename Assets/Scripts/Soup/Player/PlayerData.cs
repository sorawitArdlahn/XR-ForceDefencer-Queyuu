using System;
using System.Persistence;
using UnityEngine;

namespace CharacterData {

    [Serializable]
    public class PlayerData : ISaveable 
    {
        [field: SerializeField] public SerializableGuid Id { get; set; }

        public int HealthPointMultiplier = 1;
        public int ArmorMultiplier = 1;
        public int FuelMultiplier = 1;
        public int MovementSpeedMultiplier = 1;

        public int currentResearchPoint;
        public int researchPointRequired = 50;
    }
}
