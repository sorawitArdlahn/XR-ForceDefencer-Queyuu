using System;
using System.Collections;
using System.Collections.Generic;
using AI.Basic_Node;
using AI.Basic_Node.Control_Node;
using BlackboardSystem;
using UnityEngine;
using UnityEngine.AI;
using Model;

public class AIBrain_Crusher : MonoBehaviour, IDamageable
{
    public SimpleBlackboard blackboard;
    private BehaviourTree behaviourTree;
    
    
    public int CurrentHealth { get => blackboard.currentHealth; private set => blackboard.currentHealth = value; }
    public int CurrentArmor { get; }
    public int MaxHealth { get => blackboard.maxHealth; private set => blackboard.maxHealth = value; }
    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;

    private void Awake()
    {
        blackboard.currentHealth = blackboard.maxHealth;
        blackboard.currentArmor = blackboard.maxArmor;
        blackboard.currentFuel = blackboard.maxFuel;
        
        behaviourTree = new BehaviourTree("AI");
        
        Selector selector = new Selector("Start");

        Sequence patrolSeq = new Sequence("Patrol");
        Inverter inverter = new Inverter("inverter");
        inverter.AddChild(new Leaf("inverter", new FindPlayer(blackboard.SelfTransform,20f)));
        Leaf patrol = new Leaf("Patrol", new PatrolStrategy(blackboard.SelfTransform, blackboard.SelfNavMeshAgent, blackboard.SelfAnimator));
        patrolSeq.AddChild(inverter);
        patrolSeq.AddChild(patrol);

        Sequence actionSeq = new Sequence("Actions");
        Leaf foundPlayer = new Leaf("Found Player?", new FindPlayer(blackboard.SelfTransform,20f));
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

    
    public void TakeDamage(int Damage)
    {
        int damageTaken = Mathf.Clamp(Damage, 0, CurrentHealth);
        CurrentHealth -= damageTaken;

        if (damageTaken != 0)
        {
            OnTakeDamage?.Invoke(damageTaken);
        }

        if (CurrentHealth == 0 && damageTaken != 0)
        {
            OnDeath?.Invoke(transform.position);
        }
    }
}
