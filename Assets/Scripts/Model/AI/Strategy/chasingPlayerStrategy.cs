using System;
using AI.Basic_Node;
using UnityEngine;

public class chasingPlayerStrategy : IStrategy
{
    private SimpleBlackboard blackboard;
    private GameObject player;

    private Rigidbody rb;

    public chasingPlayerStrategy(SimpleBlackboard blackboard)
    {
        this.blackboard = blackboard;
        player = blackboard.Player;
        rb = blackboard.SelfRigidbody;
    }
    public Node.Status Process()
    {
        Debug.Log("Chasing Player Running");
        
        if (Vector3.Distance(blackboard.SelfTransform.position, player.transform.position) < 30f)
        {
            blackboard.SelfNavMeshAgent.ResetPath();
            return Node.Status.Success;
        }
        else
        {
            blackboard.SelfNavMeshAgent.SetDestination(player.transform.position);
            LookAtPlayer();

            if (blackboard.SelfTransform.position.y != player.transform.position.y)
            {
                
                Accend();
            }
            
            return Node.Status.Running;
        }
    }

    void LookAtPlayer() {
	
        // get vector to player but take y coordinate from self to make sure we are not getting any rotation on the wrong axis
        Vector3 socketLookAt = new Vector3(player.transform.position.x, blackboard.SelfTransform.position.y, player.transform.position.z);
        
        // create rotations for socket and gun
        Quaternion targetRotationSocket = Quaternion.LookRotation(socketLookAt - blackboard.SelfTransform.position);
        
        // slerp rotations and assign
        blackboard.SelfTransform.rotation = Quaternion.Slerp(blackboard.SelfTransform.rotation, targetRotationSocket, Time.deltaTime);
    }


    void Accend()
    {
        var hight = Mathf.Lerp(blackboard.SelfNavMeshAgent.baseOffset, player.transform.position.y/5, Time.deltaTime);
        blackboard.SelfNavMeshAgent.baseOffset = hight;
    }
}