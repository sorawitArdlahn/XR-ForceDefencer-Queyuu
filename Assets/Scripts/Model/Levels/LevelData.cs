using Utils.Persistence;
using UnityEngine;

namespace Model.Level {
    [System.Serializable]
    public class LevelData : ISaveable
    {
        [field: SerializeField] public string Id { get; set; }
        public int currentLevel = 0;
        public int highestLevel = 0;
        public int checkpointLevel = 0;
    }
}
