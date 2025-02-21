using System.Collections.Generic;
using AI.Basic_Node;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : IStrategy
{
    readonly Transform entity;
    readonly NavMeshAgent agent;
    readonly Transform target;
    readonly float distance;
    bool isPathCalculated;
    private Animator animator;

    public FollowPlayer(Transform entity, NavMeshAgent agent, Animator animator)
    {
        this.entity = entity;
        this.agent = agent;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        this.animator = animator;
        distance = 7f;
    }

    public Node.Status Process()
    {
        agent.isStopped = false;
        if (Vector3.Distance(entity.position, target.position) <= distance) {
            Reset();
            animator.SetBool("IsRun", false);
            agent.isStopped = true;
            agent.ResetPath();
            return Node.Status.Success;
        }
        
        animator.SetBool("IsRun", true);
        agent.SetDestination(target.position);
        entity.LookAt(target.position);

        if (agent.pathPending) {
            isPathCalculated = true;
        }
        return Node.Status.Running;
    }
    
    public void Reset() => isPathCalculated = false;
}
