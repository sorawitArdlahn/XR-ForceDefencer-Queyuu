
using AI.Basic_Node;
using AI.Basic_Node.Control_Node;
using UnityEngine;



public class AIBrain_GunnerBeta : MonoBehaviour
{
    public SimpleBlackboard blackboard;
    private BehaviourTree behaviourTree;
    private void Start()
    {
        blackboard.Player = GameObject.FindGameObjectWithTag("Player");
       behaviourTree = new BehaviourTree("AIGunner");
       Sequence sequence = new Sequence("Start");
       sequence.AddChild(new Leaf("Random Movement", new PatrolStrategy(blackboard)));
       sequence.AddChild(new Leaf("Found Player?", new FindPlayer(blackboard)));
       
       Parallel parallel1 = new Parallel("parallel");
       parallel1.AddChild(new Leaf("Found Player?", new chasingPlayerStrategy(blackboard)));
       parallel1.AddChild(new Leaf("Shoot", new ShootIStrategy(blackboard)));
       
       sequence.AddChild(parallel1);
       
       Parallel parallel2 = new Parallel("parallel");
       parallel2.AddChild(new Leaf("KeepDistance", new KeepDistance(blackboard, 50f)));
       parallel2.AddChild(new Leaf("Shoot", new ShootIStrategy(blackboard)));
       sequence.AddChild(parallel2);
       
       behaviourTree.AddChild(sequence);
    }

    private void Update()
    {
        behaviourTree.Process();
    }
}