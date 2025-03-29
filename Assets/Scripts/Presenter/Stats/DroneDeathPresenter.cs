using System;
using System.Collections.Generic;
using Controller.Stats;
using Model.Stats;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

namespace Presenter.Stats
{
    public class DroneDeathPresenter : MonoBehaviour
    {
        public RobotInGameStatsCtrl robotInGameStatsCtrl;
        
        public NavMeshAgent navMeshAgent;
        public Rigidbody rigidbody;
        public MonoBehaviour scriptAIBehavior;

        private void Awake()
        {
            robotInGameStatsCtrl.OnDeath += FallingAnimation;
        }

        private void FallingAnimation()
        {
            scriptAIBehavior.enabled = false;
            navMeshAgent.enabled = false;
            rigidbody.useGravity = true;
           
        }
    }
}
