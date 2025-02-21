using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventStateBehaviour : StateMachineBehaviour
{
    public string eventName;
    [Range(0f, 1f)] public float triggerTime;
    
    bool isTriggered;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isTriggered = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float currentTime = stateInfo.normalizedTime % 1;
        if (!isTriggered && currentTime >= triggerTime)
        {
            NotifyReceiver(animator);
            isTriggered = true;
        }
    }

    private void NotifyReceiver(Animator animator)
    {
        AnimationEventReceiver receiver = animator.GetComponent<AnimationEventReceiver>();
        if (receiver != null) 
        {
            receiver.OnAnimationEventTriggered(eventName);
        }
    }
}
