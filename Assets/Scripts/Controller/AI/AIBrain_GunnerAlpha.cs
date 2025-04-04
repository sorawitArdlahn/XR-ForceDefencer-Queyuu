
using AI.Basic_Node;
using AI.Basic_Node.Control_Node;

using UnityEngine;

public class AIBrain_GunnerAlpha : MonoBehaviour
{
    public SimpleBlackboard blackboard;
    private BehaviourTree behaviourTree;

    private void Start()
    {
        blackboard.Player = GameObject.FindGameObjectWithTag("Player");
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
    
}