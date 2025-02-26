using UnityEngine;
using Gameevent;



public class Enemy : Robots, IDamagable {
    [SerializeField] private int researchPointsGiven;

    // TODO : Enemies Animations and Models and Hitbox.
    public GameEvent OnEnemyDeath;

    void OnDestroy()
    {
        OnEnemyDeath.Raise(this);        
    }





}
