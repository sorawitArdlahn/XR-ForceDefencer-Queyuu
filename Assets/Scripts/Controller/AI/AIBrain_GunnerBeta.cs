using System;
using System.Collections;
using System.Collections.Generic;
using AI.Basic_Node;
using AI.Basic_Node.Control_Node;
using UnityEngine;

using UnityEngine.Animations.Rigging;

public class AIBrain_GunnerBeta : MonoBehaviour
{
    public SimpleBlackboard blackboard;
    private BehaviourTree behaviourTree;
    public MultiAimConstraint multiAimConstraint;
    private void Start()
    {
       behaviourTree = new BehaviourTree("AIGunner");
       Sequence sequence = new Sequence("Start");
       sequence.AddChild(new Leaf("Random Movement", new PatrolStrategy(blackboard)));
       sequence.AddChild(new Leaf("Found Player?", new FindPlayer(blackboard)));
       Parallel parallel = new Parallel("parallel");
       parallel.AddChild(new Leaf("KeepDistance", new KeepDistance(blackboard, 50f)));
       parallel.AddChild(new Leaf("Shoot", new ShootIStrategy(blackboard)));
       sequence.AddChild(parallel);
       behaviourTree.AddChild(sequence);
    }

    private void Update()
    {
        behaviourTree.Process();
    }
}