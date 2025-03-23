using UnityEngine;
using Model.Combat;
using Controller.Movement;

namespace Controller.Combat
{
   public class CombatController : MonoBehaviour
   {
      [SerializeField] private PlayerGunSelector gunSelector;
      
      [SerializeField] private PlayerInputReceiver playerInputReceiver;

      [SerializeField] private AimAssistantCtrl aimAssistantCtrl;

      [SerializeField] private MissileLauncherController missileLauncherCtrl;

      private void Update()
      {
         if (playerInputReceiver.IsFire && gunSelector)
         {
            gunSelector.guns[0].Shoot();
            gunSelector.guns[1].Shoot();
         }

         if (Input.GetKeyDown(KeyCode.M))
         {
            missileLauncherCtrl.LaunchMissile(); 
         }
      }
   }  
}
