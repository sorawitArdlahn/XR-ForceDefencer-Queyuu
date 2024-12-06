using System.Collections.Generic;
using Model.AI;
using UnityEngine;
using UnityEngine.AI;

namespace Model.AI.Strategy
{
    public class PatrolStrategy : IStrategy
    {
        private readonly Transform entity;
        private readonly NavMeshAgent agent;
        private readonly List<Transform> patrolPoints;
        private readonly float patrolSpeed;
        private int currentIndex;
        private bool isPatrolCalculated;

        public PatrolStrategy(Transform entity, NavMeshAgent agent, List<Transform> patrolPoints, float patrolSpeed = 2f)
        {
            this.entity = entity;
            this.agent = agent;
            this.patrolPoints = patrolPoints;
            this.patrolSpeed = patrolSpeed;
        }

        public Node.Status Process()
        {
            if (currentIndex == patrolPoints.Count) return Node.Status.Success;
            var target = patrolPoints[currentIndex];
            agent.SetDestination(target.position);
            entity.LookAt(target.position);

            if (isPatrolCalculated && agent.remainingDistance < 0.1f)
            {
                currentIndex++;
                isPatrolCalculated = false;
            }

            if (agent.pathPending)
            {
                isPatrolCalculated = true;
            }
            
            return Node.Status.Running;
        }

        public void Reset() => currentIndex = 0;
    }
}