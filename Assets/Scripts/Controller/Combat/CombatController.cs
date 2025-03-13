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

      [SerializeField] private GameObject missilePrefab;

      private void Update()
      {
         if (playerInputReceiver.IsFire && gunSelector.activeGun)
         {
            gunSelector.activeGun.Shoot();
         }

         if (Input.GetKeyDown(KeyCode.M))
         {
            var go = Instantiate(missilePrefab, transform.position, transform.rotation); 
            go.GetComponent<HomingMissileController>().AssignTarget(aimAssistantCtrl.GetCurrentTarget());
         }
      }

      
   }  
}
