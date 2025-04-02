using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio {

[System.Serializable]
public class Sound
{
    public string Name;
    public AudioClip Clip;
    [Range(0f, 1f)] public float Volume = 1f; // Add a volume property with a default value of 1 (full volume)
}

}