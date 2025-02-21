using UnityEngine;

namespace Model.Combat
{
    [CreateAssetMenu(fileName = "Damage Config", menuName = "Our Scriptable Object/Guns/Damage Config", order = 1)]
    public class DamageConfigScriptableObject : ScriptableObject
    {
        public Vector2Int damageRange;

        public int GetDamage(int distance = 0)
        {
            return Random.Range(damageRange.x, damageRange.y);
        }
    }
}
