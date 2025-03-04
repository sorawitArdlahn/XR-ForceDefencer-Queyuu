using System;
using System.Collections;
using System.Collections.Generic;
using AI.Basic_Node;
using AI.Basic_Node.Control_Node;
using BlackboardSystem;
using Model;
using UnityEngine;

public class AIBrain_GunnerAlpha : MonoBehaviour, IDamageable
{
    public SimpleBlackboard blackboard;
    public GunnerType gunnerType;
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
        behaviourTree = new BehaviourTree("AIGunner");
        Sequence sequence = new Sequence("Start");
        sequence.AddChild(new Leaf("Found Player?", new FindPlayer(blackboard)));
        Parallel parallel = new Parallel("parallel");
        parallel.AddChild(new Leaf("Chesing", new chasingPlayerStrategy(blackboard)));
        parallel.AddChild(new Leaf("Shoot", new ShootIStrategy(blackboard)));
        sequence.AddChild(parallel);
        behaviourTree.AddChild(sequence);
    }

    private void Update()
    {
        behaviourTree.Process();
    }
    
    void IDamageable.TakeDamage(int damage)
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