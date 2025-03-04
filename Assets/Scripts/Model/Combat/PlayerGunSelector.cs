using System.Collections.Generic;
using UnityEngine;

namespace Model.Combat
{
    [DisallowMultipleComponent]
    public class PlayerGunSelector : MonoBehaviour
    {
        [SerializeField] private GunType gun;
        [SerializeField] private Transform gunParent;
        [SerializeField] private List<GunScriptableObject> guns; 
        //[SerializeField] private PlayerIK inverseKinematic; <- for model thing
        [Space] 
        [Header("Runtime Filled")] 
        public GunScriptableObject activeGun;

        private void Start()
        {
            GunScriptableObject usesGun = guns.Find(usedGun => usedGun.Type == gun);
            if (usesGun == null)
            {
                Debug.LogError($"No GunScriptableObject found for Guntype: {usesGun}");
                return;
            }

            activeGun = usesGun;
        }
    }
}
