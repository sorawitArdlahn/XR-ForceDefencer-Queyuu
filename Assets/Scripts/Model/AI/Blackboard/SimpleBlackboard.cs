using UnityEngine;
using UnityEngine.AI;
using Model.Combat;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;
using Controller.Stats;
using Model.Stats;
using Controller.Level;

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
public class SimpleBlackboard:MonoBehaviour
{
    [Header("AI Components")]
    public Transform SelfTransform;
    public Animator SelfAnimator = null;
    public NavMeshAgent SelfNavMeshAgent;
    public AnimationEventReceiver SelfAnimationEventReceiver = null;
    public Rigidbody SelfRigidbody;
    public PlayerGunSelector SelfGunSelector = null;

    // [Header("AI Stats")] 
    // public RobotInGameStatsCtrl SelfStats;

    [Header("Implicated Variables")]
    public GameObject Player;

    [Header("AI Stats")] 
    public AIBaseStatsCurve aIBaseStatsCurve;
    public int maxHP;
    public int maxArmor;
    public int maxFuel;
    public float speed;
        
    [Header("In-game Updatable Stats")]
    public int currentHP;
    public int currentArmor;
    public int currentFuel;

    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        int currentLevel;
        try {currentLevel = FindObjectOfType<LevelManagerController>().getCurrentLevel();}
        catch {currentLevel = 1;}
        maxHP = (int)aIBaseStatsCurve.baseHPCurve.Evaluate(currentLevel);
        maxArmor = (int)aIBaseStatsCurve.basedArmorCurve.Evaluate(currentLevel);
        maxFuel = (int)aIBaseStatsCurve.baseFuelCurve.Evaluate(currentLevel);
        speed = aIBaseStatsCurve.baseSpeedCurve.Evaluate(currentLevel);

        currentHP = maxHP;
        currentArmor = maxArmor;
        currentFuel = maxFuel;
    }

}
