using UnityEngine;
using UnityEngine.AI;
using Model.Combat;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;
using Controller.Stats;

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
public class SimpleBlackboard:MonoBehaviour
{
    [Header("AI Components")]
    public Transform SelfTransform;
    public Animator SelfAnimator = null;
    public NavMeshAgent SelfNavMeshAgent;
    public AnimationEventReceiver SelfAnimationEventReceiver = null;
    public Rigidbody SelfRigidbody;
    public PlayerGunSelector SelfGunSelector = null;

    [Header("AI Stats")] 
    public RobotInGameStatsCtrl SelfStats;

    [Header("Implicated Variables")]
    public GameObject Player;

}
