using System;
using UnityEngine;
using Model.Combat;
using Controller.Movement;
using Presenter.Sound;

namespace Controller.Combat
{
   public class CombatController : MonoBehaviour
   {
      [SerializeField] private PlayerGunSelector gunSelector;
      
      [SerializeField] private PlayerInputReceiver playerInputReceiver;

      [SerializeField] private AimAssistantCtrl aimAssistantCtrl;
      
      [Header("Missile Config")]
      [SerializeField] private MissileLauncherController missileLauncherCtrl;
      public float cooldown = 5;
      private float nextUseTime = 0;
      public TMPro.TMP_Text cooldownText;
      

      private void Update()
      {
         float timeLeft = nextUseTime - Time.time;
         cooldownText.text = Mathf.Ceil(timeLeft).ToString("0");
         cooldownText.color = Color.red;
         
         if (timeLeft < 0)
         {
            cooldownText.text = "Ready";
            cooldownText.color = Color.black;
         }
      
         if (playerInputReceiver.IsFire && gunSelector)
         {
            gunSelector.guns[0].Shoot();
            gunSelector.guns[1].Shoot();
         }
      
      
         if (playerInputReceiver.IsMissile)
         {
            if (Time.time >= nextUseTime)
            {
               nextUseTime = Time.time + cooldown;
               missileLauncherCtrl.LaunchMissile(); 
            }
         }
         
      }
   }  
}
