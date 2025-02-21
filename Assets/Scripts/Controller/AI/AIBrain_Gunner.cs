using System;
using System.Collections;
using System.Collections.Generic;
using AI.Basic_Node;
using AI.Basic_Node.Control_Node;
using BlackboardSystem;
using UnityEngine;

public class AIBrain_Gunner : MonoBehaviour
{
    public SimpleBlackboard blackboard;
    public GunnerType gunnerType;
    private BehaviourTree behaviourTree;
    private void Awake()
    {
       behaviourTree = new BehaviourTree("AIGunner");
       Sequence sequence = new Sequence("Start");
       sequence.AddChild(new Leaf("Found Player?", new FindPlayer(blackboard.SelfTransform, 50f)));
       Parallel parallel = new Parallel("parallel");
       parallel.AddChild(new Leaf("KeepDistance", new KeepDistance(blackboard, 30f)));
       parallel.AddChild(new Leaf("Shoot", new ShootIStrategy(blackboard)));
       sequence.AddChild(parallel);
       behaviourTree.AddChild(sequence);
    }

    private void Update()
    {
        behaviourTree.Process();
    }

    private void AlphaGunnerSetup()
    {
        
    }
    
    private void BetaGunnerSetup()
    {
        
    }
}

public enum GunnerType
{
    Alpha, Beta
}