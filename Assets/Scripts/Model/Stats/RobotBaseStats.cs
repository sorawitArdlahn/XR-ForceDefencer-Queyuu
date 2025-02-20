using UnityEngine;

namespace Model.Stats
{
    [CreateAssetMenu(fileName = "RobotBaseStat", menuName = "Data Object/Stats/Robot Base Stat")]
    public class RobotBaseStats : ScriptableObject
    {
        public int baseHP;
        public int basedArmor;
        public int baseFuel;
        public float baseSpeed;
    }
}
