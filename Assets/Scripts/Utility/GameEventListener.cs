using UnityEngine;
using UnityEngine.Events;

namespace EventListener
{
    [System.Serializable]
    public class CustomGameEvent : UnityEvent<Component, object> { }
    
    public class GameEventListener : MonoBehaviour
    {
    public GameEvent Event;
    public CustomGameEvent Response;

    private void OnEnable()
    {
        Event.RegisterListeners(this);
    }

    private void OnDisable()
    {
        Event.UnRegisterListeners(this);
    }

    public void OnEventRaised(Component sender, object data)
    {
        Response.Invoke(sender, data);
    }
}
    
}

