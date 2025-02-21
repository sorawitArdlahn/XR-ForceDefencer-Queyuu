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


    public FindPlayer(Transform entity, float minDistance)
    {
        this.entity = entity;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        this.minDistance = minDistance;
    }

    public Node.Status Process()
    {
        if (Vector3.Distance(entity.position, target.position) <= minDistance)
        {
            Debug.Log("Player found");
            return Node.Status.Success;
        }
        Debug.Log("Player not found");
        return Node.Status.Failure;
    }

    public void Reset()
    {
        isPathCalculated = false;
    }
}
