using System.Collections.Generic;
using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
   private List<AnimationEvent> animeEvents = new List<AnimationEvent>();
    
    public void AddAnimationEvent(AnimationEvent animationEvent) => animeEvents.Add(animationEvent);

    public void OnAnimationEventTriggered(string eventName)
    {
        AnimationEvent matchingEvent = animeEvents.Find(x => x.eventName == eventName);
        matchingEvent?.OnAnimationEvent?.Invoke();
    }
}