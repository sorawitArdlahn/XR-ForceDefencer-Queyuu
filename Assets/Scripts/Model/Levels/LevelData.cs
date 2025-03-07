using System.Persistence;
using UnityEngine;

namespace Model.Level {
    [System.Serializable]
    public class LevelData : ISaveable
    {
        [field: SerializeField] public SerializableGuid Id { get; set; }
        public int currentLevel = 0;
        public int highestLevel = 0;
        public int checkpointLevel = 0;
    }
}
