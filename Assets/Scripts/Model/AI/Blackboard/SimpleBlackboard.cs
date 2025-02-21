using UnityEngine;
using UnityEngine.AI;
using Model.Combat;

public class SimpleBlackboard:MonoBehaviour
{
    public Transform SelfTransform;
    public Animator SelfAnimator;
    public NavMeshAgent SelfNavMeshAgent;
    public AnimationEventReceiver SelfAnimationEventReceiver;
    public Rigidbody SelfRigidbody;
    public PlayerGunSelector SelfGunSelector;
}
