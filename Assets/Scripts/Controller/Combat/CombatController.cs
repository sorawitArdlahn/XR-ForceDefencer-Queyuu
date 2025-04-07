using System;
using System.Collections;
using UnityEngine;
using Model.Combat;
using Controller.Movement;
using Presenter.Sound;

namespace Controller.Combat
{
   public class CombatController : MonoBehaviour
   {
      [SerializeField] private Animator _animator;
      [SerializeField] private PlayerGunSelector gunSelector;
      
      [SerializeField] private PlayerInputReceiver playerInputReceiver;

      [SerializeField] private AimAssistantCtrl aimAssistantCtrl;
      
      [Header("Missile Config")]
      [SerializeField] private MissileLauncherController missileLauncherCtrl;
      public float cooldown = 5;
      private float nextUseTime = 0;
      public TMPro.TMP_Text cooldownText;
      
      [Header("Melee Config")]
      public AnimationEventReceiver animationEventReceiver;
      public string endEventName;
      public AudioSource audioSource;
      public AudioClip meleeSound;
      private bool isCanMelee = true;

      private void Start()
      {
         RegisterEndAttackAnimationEvent();
      }

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

         if (playerInputReceiver.IsMelee && isCanMelee)
         {
            isCanMelee = false;
            _animator.SetTrigger("Punch");
            audioSource.PlayOneShot(meleeSound);
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

      private void RegisterEndAttackAnimationEvent(){
         AnimationEvent animationEvent = new AnimationEvent();
         animationEvent.eventName = endEventName;
         animationEvent.OnAnimationEvent += WaitForNextMelee;
         animationEventReceiver.AddAnimationEvent(animationEvent);
      }
      
      private void WaitForNextMelee(){
         isCanMelee = true;
      }
   }  
}
