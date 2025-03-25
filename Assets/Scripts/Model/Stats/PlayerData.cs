using System;
using System.Persistence;
using UnityEngine;

namespace Model.Stats {

    [Serializable]
    public class PlayerData : ISaveable 
    {
        [field: SerializeField] public SerializableGuid Id { get; set; }

        public float HealthPointMultiplier = 1;
        public float ArmorMultiplier = 1;
        public float FuelMultiplier = 1;
        public float MovementSpeedMultiplier = 1;

        public int currentResearchPoint = 0;
        public int researchPointRequired = 50;

        public int accumulatedResearchPoint = 0;
    }
}
