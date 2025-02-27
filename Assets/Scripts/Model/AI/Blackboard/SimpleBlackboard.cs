using UnityEngine;
using UnityEngine.AI;
using Model.Combat;

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody), typeof(PlayerGunSelector))]
public class SimpleBlackboard:MonoBehaviour
{
    [Header("AI Components")]
    public Transform SelfTransform;
    public Animator SelfAnimator;
    public NavMeshAgent SelfNavMeshAgent;
    public AnimationEventReceiver SelfAnimationEventReceiver;
    public Rigidbody SelfRigidbody;
    public PlayerGunSelector SelfGunSelector;

    [Header("AI Stats")] 
    public int maxHealth;
    public int currentHealth;
    public int maxArmor;
    public int currentArmor;
    public int maxFuel;
    public int currentFuel;
    public float Speed;

    [Header("Implicated Variables")]
    public GameObject Player;

}
