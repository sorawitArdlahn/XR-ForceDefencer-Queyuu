using AI.Basic_Node;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class CombatsStrategy : IStrategy
{
    private Animator animator;
    private NavMeshAgent agent;
    private bool isBusy;
    private bool isAnimationFinished;
    private AnimationEventReceiver receiver;
    private Transform entity;
    private Transform target;

    private Vector3 velocity;
    public CombatsStrategy(SimpleBlackboard blackboard)
    {
        this.entity = blackboard.SelfTransform;
        this.animator = blackboard.SelfAnimator;
        this.agent = blackboard.SelfNavMeshAgent;
        isBusy = false;
        isAnimationFinished = false;
        this.receiver = blackboard.SelfAnimationEventReceiver;
        RegisterEvent();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        velocity = agent.velocity;
    }

    public Node.Status Process()
    {
        if (isAnimationFinished)
        {
            isAnimationFinished = false;
            isBusy = false;
            agent.isStopped = false;
            agent.velocity = velocity;
            Debug.Log("combats strategy success");
            return Node.Status.Success;
        }
        
        if (!isBusy)
        {
            isBusy = true;
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.velocity = Vector3.zero;
            animator.SetTrigger("HeavyAttack");
        }
        
        // entity.LookAt(target.position, Vector3.up);
        return Node.Status.Running;
        
    }

    private void RegisterEvent()
    {
        if (receiver != null)
        {
            AnimationEvent animationEvent = new AnimationEvent();
            animationEvent.eventName = "HeavyAttack";
            animationEvent.OnAnimationEvent += OnHeavyAttack;
            receiver.AddAnimationEvent(animationEvent);
        }
    }

    private void OnHeavyAttack()
    {
        isAnimationFinished = true;
    }
    
}
