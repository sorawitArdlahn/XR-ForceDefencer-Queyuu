using UnityEngine;

namespace Model.Combat
{
    [CreateAssetMenu(fileName = "Trail Config", menuName = "Our Scriptable Object/Guns/Gun Trail Configuration", order = 4)]
    public class TrailConfigScriptableObject : ScriptableObject
    {
        public Material trailMaterial;
        public AnimationCurve widthCurve;
        public float Duration = 0.5f;
        public float MinVertexDistance = 0.1f;
        public Gradient color;
    
        public float missDistance = 100f;
        public float SimulationSpeed = 100f;
    }
}
