using System.Collections.Generic;
using UnityEngine;

namespace EventListener {
[CreateAssetMenu(fileName = "GameEvent")]
[System.Serializable]
public class GameEvent : RuntimeScriptableObject
{
    // Start is called before the first frame update
    public List<GameEventListener> listeners = new List<GameEventListener>();
    
    public void Raise(Component sender, object data=null)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(sender, data);
        }
    }

    public void RegisterListeners(GameEventListener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public void UnRegisterListeners(GameEventListener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }

    protected override void OnReset()
    {
        listeners.Clear();
    }
}
}
