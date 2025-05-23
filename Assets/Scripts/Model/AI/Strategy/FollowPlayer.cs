﻿using System.Collections.Generic;
using AI.Basic_Node;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : IStrategy
{
    readonly Transform entity;
    readonly NavMeshAgent agent;
    readonly float distance;
    bool isPathCalculated;
    private Animator animator;

    private SimpleBlackboard blackboard;
    private GameObject player;

    public FollowPlayer(SimpleBlackboard blackboard)
    {
        this.entity = blackboard.SelfTransform;
        agent = blackboard.SelfNavMeshAgent;
        player = blackboard.Player;
        this.animator = blackboard.SelfAnimator;
        distance = 5f;
    }

    public Node.Status Process()
    {
        agent.isStopped = false;
        agent.velocity = agent.desiredVelocity.normalized * Mathf.Clamp(agent.speed, 100, 150);
        if (Vector3.Distance(entity.position, player.transform.position) <= distance) {
            Reset();
            animator.SetBool("IsRun", false);
            agent.isStopped = true;
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            return Node.Status.Success;
        }
        
        animator.SetBool("IsRun", true);
        agent.SetDestination(player.transform.position);
        LookAtPlayer();
        if (agent.pathPending) {
            isPathCalculated = true;
        }
        return Node.Status.Running;
    }
    
    public void Reset() => isPathCalculated = false;

    void LookAtPlayer() {
	
        // get vector to player but take y coordinate from self to make sure we are not getting any rotation on the wrong axis
        try {
            Vector3 socketLookAt = new Vector3(player.transform.position.x, blackboard.SelfTransform.position.y, player.transform.position.z);
            Quaternion targetRotationSocket = Quaternion.LookRotation(socketLookAt - blackboard.SelfTransform.position);
        
        // slerp rotations and assign
            blackboard.SelfTransform.rotation = Quaternion.Slerp(blackboard.SelfTransform.rotation, targetRotationSocket, Time.deltaTime);  
        } catch (System.Exception e) {
           
        }
        
        // create rotations for socket and gun
        
    }
}
