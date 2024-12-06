using System;
using System.Collections.Generic;
using Model.AI;
using Model.AI.Strategy;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Controller
{
    public class EnemyController : MonoBehaviour
    {
        private NavMeshAgent agent;
        private BehaviourTree behaviourTree;
        public List<Transform> waypoints;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            behaviourTree = new BehaviourTree("Enemy");
            behaviourTree.AddChild(new Leaf("Patrol", new PatrolStrategy(transform,agent,waypoints)));
        }

        private void Update()
        {
            behaviourTree.Process();
        }
    }
}
