﻿using System;
using UnityEngine.Events;

[Serializable]
public class AnimationEvent
{
    public string eventName;
    public Action OnAnimationEvent;
    
}
