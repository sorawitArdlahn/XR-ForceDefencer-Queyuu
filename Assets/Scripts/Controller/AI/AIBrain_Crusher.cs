using System;
using System.Collections;
using System.Collections.Generic;
using AI.Basic_Node;
using AI.Basic_Node.Control_Node;
using UnityEngine;
using UnityEngine.AI;
using Model;

public class AIBrain_Crusher : MonoBehaviour, IDamageable
{
    public SimpleBlackboard blackboard;
    private BehaviourTree behaviourTree;

    public int CurrentHealth => blackboard.currentHP;

    public int CurrentArmor => blackboard.currentArmor;

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;

    public GameObject deathEffect;

    private void Start()
    {
        
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
           
            int sign = (int)Mathf.Sign(blackboard.currentArmor - damageTaken); //Check if Damage value over armor or not.
            
            if (sign >= 0) //if damage still less than armor. 
            {
                blackboard.currentHP = blackboard.currentArmor - damageTaken;
            }
            else
            {
                int damageRemainder =  Mathf.Abs(CurrentArmor - damageTaken); //Calculate the remaining damage after armor reduction.
                blackboard.currentArmor = 0;
                
                blackboard.currentHP -= damageRemainder;
                if (CurrentHealth <= 0) // Death
                {
                    blackboard.currentHP = 0;
                    OnDeath?.Invoke();
                    var effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
                    Destroy(effect, 1f);
                    Destroy(gameObject,5f);
                }
            }
    }
}
