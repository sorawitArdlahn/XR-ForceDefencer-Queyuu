using UnityEngine;

namespace Model.Combat
{
    [CreateAssetMenu(fileName = "Shoot Configuration", menuName = "Our Scriptable Object/Guns/Shoot Configuration", order = 2)]
    public class ShootConfigScriptableObject : ScriptableObject
    {
        public LayerMask hitLayerMask;
        public Vector3 Spread = new Vector3(0.1f, 0.1f, 0.1f);
        public float fireRate = 0.25f;
    }
}
