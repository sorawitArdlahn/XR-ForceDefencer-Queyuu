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

    public CombatsStrategy(Transform entity,Animator animator, NavMeshAgent agent, AnimationEventReceiver animationEventReceiver)
    {
        this.entity = entity;
        this.animator = animator;
        this.agent = agent;
        isBusy = false;
        isAnimationFinished = false;
        this.receiver = animationEventReceiver;
        RegisterEvent();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public Node.Status Process()
    {
        if (isAnimationFinished)
        {
            isAnimationFinished = false;
            isBusy = false;
            agent.isStopped = false;
            Debug.Log("combats strategy success");
            return Node.Status.Success;
        }
        
        if (!isBusy)
        {
            isBusy = true;
            agent.isStopped = true;
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
        Debug.Log("Animation finished");
    }
    
}
