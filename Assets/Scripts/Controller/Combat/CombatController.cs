using UnityEngine;
using Model.Combat;

namespace Controller.Combat
{
   public class CombatController : MonoBehaviour
   {
      [SerializeField] private PlayerGunSelector gunSelector;
      [SerializeField] private PlayerInputReceiver playerInputReceiver;

      private void Update()
      {
         if (playerInputReceiver.IsFire && gunSelector.activeGun)
         {
            gunSelector.activeGun.Shoot();
         }
      }
   }
}
