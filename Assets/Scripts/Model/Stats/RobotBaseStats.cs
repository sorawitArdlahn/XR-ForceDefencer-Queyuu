using UnityEngine;

namespace Model.Stats
{
    [CreateAssetMenu(fileName = "RobotBaseStat", menuName = "Our Scriptable Object/Stats/Robot Base Stat", order = 0)]
    public class RobotBaseStats : ScriptableObject
    {
        public int baseHP;
        public int basedArmor;
        public int baseFuel;
        public float baseSpeed;
    }
}
