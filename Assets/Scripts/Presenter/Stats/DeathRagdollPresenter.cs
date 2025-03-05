using System.Collections;
using System.Collections.Generic;
using Controller.Stats;
using UnityEngine;
using UnityEngine.AI;

public class DeathRagdollPresenter : MonoBehaviour
{
    public RobotInGameStatsCtrl robotInGameStatsCtrl;
    public List<Collider> ragdollColliders;

    public NavMeshAgent navMeshAgent;

    public Collider mainCollider;

    public Animator animator;

    public AIBrain_Crusher aiBrainCrusher;

    private void Start()
    {
        robotInGameStatsCtrl.OnDeath += EnabledRagdoll;
        foreach (var ragdollCollider in ragdollColliders)
        {
            ragdollCollider.enabled = false;
        }
    }

    private void EnabledRagdoll()
    {
        foreach (var ragdollCollider in ragdollColliders)
        {
            aiBrainCrusher.enabled = false;
            navMeshAgent.enabled = false;
            animator.enabled = false;
            mainCollider.enabled = false;
            ragdollCollider.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            ragdollCollider.enabled = true;
        }
    }
}
