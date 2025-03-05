using System;
using System.Collections;
using System.Collections.Generic;
using AI.Basic_Node;
using AI.Basic_Node.Control_Node;
using BlackboardSystem;
using UnityEngine;
using UnityEngine.AI;
using Model;

public class AIBrain_Crusher : MonoBehaviour
{
    public SimpleBlackboard blackboard;
    private BehaviourTree behaviourTree;
    
    
    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;

    private void Awake()
    {

        blackboard.Player = GameObject.FindGameObjectWithTag("Player");
        
        behaviourTree = new BehaviourTree("AI");
        
        Selector selector = new Selector("Start");

        Sequence patrolSeq = new Sequence("Patrol");
        Inverter inverter = new Inverter("inverter");
        inverter.AddChild(new Leaf("inverter", new FindPlayer(blackboard)));
        Leaf patrol = new Leaf("Patrol", new PatrolStrategy(blackboard));
        patrolSeq.AddChild(inverter);
        patrolSeq.AddChild(patrol);

        Sequence actionSeq = new Sequence("Actions");
        Leaf foundPlayer = new Leaf("Found Player?", new FindPlayer(blackboard));
        Leaf followPlayer = new Leaf("followPlayer", new FollowPlayer(blackboard));
        Leaf randomCombat = new Leaf("WhatToDo",new CombatsStrategy(blackboard.SelfTransform,blackboard.SelfAnimator, blackboard.SelfNavMeshAgent, blackboard.SelfAnimationEventReceiver));
        
        actionSeq.AddChild(foundPlayer);
        actionSeq.AddChild(followPlayer);
        actionSeq.AddChild(randomCombat);
        
        selector.AddChild(patrolSeq);
        selector.AddChild(actionSeq);
        
        behaviourTree.AddChild(selector);
    }

    private void Update()
    {
        behaviourTree.Process();
    }

    
}
