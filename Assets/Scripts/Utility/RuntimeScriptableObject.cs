using System.Collections.Generic;
using UnityEngine;

public abstract class RuntimeScriptableObject : ScriptableObject
{
    static readonly List<RuntimeScriptableObject> instances = new();

    void OnEnable()
    {
        instances.Add(this);
    }

    void OnDisable()
    {
        instances.Remove(this);
    }

    protected abstract void OnReset();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void ResetAllInstances()
    {
        foreach (var instance in instances)
        {
            instance.OnReset();
        }
    }
}
