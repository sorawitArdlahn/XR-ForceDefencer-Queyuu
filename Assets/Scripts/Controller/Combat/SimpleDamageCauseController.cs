using Model;

using UnityEngine;

public class SimpleDamageCauseController : MonoBehaviour
{
    public string tag;
    public AnimationEventReceiver animationEventReceiver;
    public string beginEventName;
    public string endEventName;
    public ParticleManager particleManager = null;

    private Collider collider;
    private bool isCanAttack = true;
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
            if (particleManager != null)
            {
                particleManager.PlayParticles();
            }
            
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
