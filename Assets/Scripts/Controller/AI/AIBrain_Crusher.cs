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
    
    
    public int MaxHealth { get => blackboard.maxHealth; private set => blackboard.maxHealth = value; }
    public int MaxArmor { get => blackboard.maxArmor; private set => blackboard.maxArmor = value; }
    public int CurrentHealth { get => blackboard.currentHealth; set => blackboard.currentHealth = value; }
    public int CurrentArmor { get => blackboard.currentArmor; set => blackboard.currentArmor = value; }
    
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

    
    public void TakeDamage(int damage)
    {
        int damageTaken = damage;
            
        if (CurrentArmor > 0)
        {
            damageTaken = damage / 2; //Armor can absorb damage and reduce them to half.
        }
        
        int sign = (int)Mathf.Sign(CurrentArmor - damageTaken); //Check if Damage value over armor or not.
        
        if (sign >= 0) //if damage still less than armor. 
        {
            CurrentArmor -= damageTaken;
        }
        else
        {
            damageTaken =  Mathf.Abs(CurrentArmor - damageTaken); //Calculate the remaining damage after armor reduction.
            CurrentArmor = 0;
            
            CurrentHealth -= damageTaken;
            if (CurrentHealth <= 0) // Death
            {
                CurrentHealth= 0;  
            }
        }
    }
}
