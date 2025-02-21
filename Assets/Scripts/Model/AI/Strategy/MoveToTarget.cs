using System.Collections.Generic;
using AI.Basic_Node;
using UnityEngine;
using UnityEngine.AI;

public class MoveToTarget : IStrategy
 {
     readonly Transform entity;
     readonly NavMeshAgent agent;
     readonly Transform target;
     bool isPathCalculated;

     public MoveToTarget(Transform entity, NavMeshAgent agent, Transform target) {
         this.entity = entity;
         this.agent = agent;
         this.target = target;
     }

     public Node.Status Process() {
         
         if (Vector3.Distance(entity.position, target.position) < 1f) {
             return Node.Status.Success;
         }
            
         agent.SetDestination(target.position);
         entity.LookAt(target.position);

         if (agent.pathPending) {
             isPathCalculated = true;
         }
         return Node.Status.Running;
     }

     public void Reset() => isPathCalculated = false;
 }    
