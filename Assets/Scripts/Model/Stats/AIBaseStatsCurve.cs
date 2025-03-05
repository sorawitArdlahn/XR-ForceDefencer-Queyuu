using UnityEngine;

namespace Model.Stats
{
    [CreateAssetMenu(fileName = "AIBaseStatCurve", menuName = "Our Scriptable Object/Stats/AI Base Stat Curve", order = 0)]
    public class AIBaseStatsCurve : ScriptableObject
    {
        public AnimationCurve baseHPCurve;
        public AnimationCurve basedArmorCurve;
        public AnimationCurve baseFuelCurve;
        public AnimationCurve baseSpeedCurve;
    }
}
