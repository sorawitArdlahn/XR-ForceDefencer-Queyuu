
 using AI.Basic_Node;
 using UnityEngine;


 public class KeepDistance : IStrategy
 {
     private float distance;
     private SimpleBlackboard blackboard;
     private Transform playerTransform;

     public KeepDistance(SimpleBlackboard blackboard,float distance)
     {
         this.blackboard = blackboard;
         this.distance = distance;
         this.playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
         
     }

     public Node.Status Process()
     {
         if (Vector3.Distance(blackboard.SelfTransform.position, playerTransform.position) >= distance)
         {
             blackboard.SelfNavMeshAgent.ResetPath();
             blackboard.SelfRigidbody.Sleep();
             return Node.Status.Success;
         }
         
         MoveBackwards();
         return Node.Status.Running;
     }

     private void MoveBackwards()
     {
         blackboard.SelfTransform.LookAt(playerTransform);
         Vector3 direction = blackboard.SelfTransform.forward;
         float speed = blackboard.SelfNavMeshAgent.speed;
         
         blackboard.SelfRigidbody.AddForce(-direction * (speed * blackboard.SelfRigidbody.mass), ForceMode.Force);
     }
 }
