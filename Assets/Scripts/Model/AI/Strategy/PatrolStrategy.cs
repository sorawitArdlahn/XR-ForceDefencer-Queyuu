﻿using System.Collections.Generic;
using AI.Basic_Node;
using UnityEngine;
using UnityEngine.AI;

public class PatrolStrategy : IStrategy
    {
        private readonly Transform entity;
        private readonly NavMeshAgent agent;
        private readonly float patrolSpeed;
        private int currentIndex;
        private bool isPatrolCalculated;
        private Animator animator;
        private Vector3 targetPosition;
        private Vector3 result;
        private bool isReset;
        private SimpleBlackboard blackboard;
        public PatrolStrategy(SimpleBlackboard blackboard)
        {
            this.blackboard = blackboard;
            this.entity = blackboard.SelfTransform;
            this.agent = blackboard.SelfNavMeshAgent;
            this.animator = blackboard.SelfAnimator;
    
            isPatrolCalculated = true;
            this.animator.Rebind();
            isReset = false;
        }
        public Node.Status Process()
        {
            // if (Physics.Raycast(entity.position, entity.forward, out RaycastHit hit, 5f))
            // {
            //     Debug.DrawRay(entity.position, entity.forward * 3f, Color.red);
            //     if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
            //     {
            //         agent.ResetPath();
            //     }
            // }

            // if (isReset)
            // {
            //     agent.ResetPath();
            //     animator.SetBool("IsRun", false);
            //     isPatrolCalculated = true;
            //     isReset = false;
            //     return Node.Status.Failure;
            // }
            
            if (isPatrolCalculated)
            {
                RegisterDestination();
                isPatrolCalculated = false;
            }
            
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.ResetPath();
                animator.SetBool("IsRun", false);
                isPatrolCalculated = true;
                return Node.Status.Success;
            }
            
            
            return Node.Status.Running;
            
        }

        private void RegisterDestination()
        {
            Vector3 point;
            if (RandomPoint(entity.position, 20, out point))
            {
                Debug.DrawRay(point, Vector3.up, Color.red, 1f);
                agent.SetDestination(point);
                animator.SetBool("IsRun",true);
            }
            // else
            // {
            //     isReset = true;
            // }
        }

        private bool RandomPoint(Vector3 center, float range, out Vector3 result)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 2.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                
                return true;
            }
            result = Vector3.zero;
            
            return false;
        }

        public void Reset() => currentIndex = 0;
}
