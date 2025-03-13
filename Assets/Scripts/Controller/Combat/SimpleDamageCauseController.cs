using Model;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;

public class SimpleDamageCauseController : MonoBehaviour
{
    public string tag;
    public AnimationEventReceiver animationEventReceiver;
    public string beginEventName;
    public string endEventName;
    public ParticleManager particleManager;

    private Collider collider;
    private bool isCollisionActive = false;
    
    void Start()
    {
        collider = GetComponent<Collider>();
        collider.enabled = false;
        RegisterBeginAttackAnimationEvent();
        RegisterEndAttackAnimationEvent();
    }

    private void OnBeginAttack()
    {
        collider.enabled = true;
    }
    private void OnEndAttack(){
        collider.enabled = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tag) && other.TryGetComponent(out IDamageable damageable)){
            particleManager.PlayParticles();
            damageable.TakeDamage(20);
            collider.enabled = false;
        }
    }

    private void RegisterBeginAttackAnimationEvent(){
        AnimationEvent animationEvent = new AnimationEvent();
        animationEvent.eventName = beginEventName;
        animationEvent.OnAnimationEvent += OnBeginAttack;
        animationEventReceiver.AddAnimationEvent(animationEvent);
    }

    private void RegisterEndAttackAnimationEvent(){
        AnimationEvent animationEvent = new AnimationEvent();
        animationEvent.eventName = endEventName;
        animationEvent.OnAnimationEvent += OnEndAttack;
        animationEventReceiver.AddAnimationEvent(animationEvent);
    }
}
