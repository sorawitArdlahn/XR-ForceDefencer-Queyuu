using AI.Basic_Node;
using UnityEngine;

public class ShootIStrategy : IStrategy
{
    private SimpleBlackboard blackboard;
    private bool isCanShoot;
    private Transform playerTransform;

    public ShootIStrategy(SimpleBlackboard blackboard)
    {
        
        this.blackboard = blackboard;
        isCanShoot = true;
    }

    public Node.Status Process()
    {
        if (isCanShoot)
        {
            isCanShoot = false;
            blackboard.SelfTransform.LookAt(playerTransform);
            blackboard.SelfGunSelector.activeGun.Shoot();
            isCanShoot = true;
            return Node.Status.Success;
        }

        return Node.Status.Running;
    }
}
