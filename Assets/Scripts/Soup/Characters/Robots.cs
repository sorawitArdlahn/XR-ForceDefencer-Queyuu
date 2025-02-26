using UnityEngine;
using Gameevent;

public class Robots : MonoBehaviour, IDamagable
{
    public float startingHealth;
    [SerializeField] protected float health;
    public float Health => health;
    protected bool dead;
    public GameEvent OnDeath;
    public event System.Action<float> OnDamage;

    protected virtual void Start(){
        health = startingHealth;
    }
    public void TakeDamage(float damage){
        // Debug.Log($"took damage");
        if(OnDamage != null)
            OnDamage(damage);
    }
    public void TakeHit(float damage, RaycastHit hit)
    {
        Debug.Log($"took hit");
        health -= damage;

        if(health<=0&&!dead){
            Die();
        }
    }
    public void Die(){
        dead = true;
        if(OnDeath != null)
            OnDeath.Raise(this);
    }
}
