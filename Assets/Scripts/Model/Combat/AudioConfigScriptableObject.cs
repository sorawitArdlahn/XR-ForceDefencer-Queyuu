using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Combat
{
    [CreateAssetMenu(fileName = "Audio Config", menuName = "Our Scriptable Object/Guns/Audio Config", order = 4)]
    public class AudioConfigScriptableObject : ScriptableObject
    {
        [Range(0, 1f)]
        public float Volume = 1f;
        public AudioClip[] FireClips;
        // public AudioClip EmptyClip;
        // public AudioClip ReloadClip;
        // public AudioClip LastBulletClip;

        public void PlayShootingClip(AudioSource audioSource)
        {
            if (FireClips.Length != 0)
            {
                audioSource.PlayOneShot(FireClips[Random.Range(0, FireClips.Length)], Volume);
            }
            
        }
    }
}
