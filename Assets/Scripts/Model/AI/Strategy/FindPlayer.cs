using System.Collections.Generic;
using AI.Basic_Node;
using UnityEngine;
using UnityEngine.AI;

public class FindPlayer : IStrategy
{
    readonly Transform entity;
    readonly Transform target;
    readonly float minDistance;
    bool isPathCalculated;

    private GameObject player;
    private SimpleBlackboard blackboard;


    public FindPlayer(SimpleBlackboard blackboard)
    {
        this.entity = blackboard.SelfTransform;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        this.minDistance = 10f;
        this.blackboard = blackboard;
        player = blackboard.Player;
    }

    public Node.Status Process()
    {
        if (Vector3.Distance(entity.position, target.position) <= minDistance)
        {

            Debug.Log("Player found");
            LookAtPlayer();
            return Node.Status.Success;
        }
        Debug.Log("Player not found");
        return Node.Status.Failure;
    }

    public void Reset()
    {
        isPathCalculated = false;
    }

    void LookAtPlayer() {
	
        // get vector to player but take y coordinate from self to make sure we are not getting any rotation on the wrong axis
        Vector3 socketLookAt = new Vector3(player.transform.position.x, blackboard.SelfTransform.position.y, player.transform.position.z);
        
        // create rotations for socket and gun
        Quaternion targetRotationSocket = Quaternion.LookRotation(socketLookAt - blackboard.SelfTransform.position);
        
        // slerp rotations and assign
        blackboard.SelfTransform.rotation = Quaternion.Slerp(blackboard.SelfTransform.rotation, targetRotationSocket, Time.deltaTime*2);
    }
}
