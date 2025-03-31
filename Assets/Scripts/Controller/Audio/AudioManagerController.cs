using UnityEngine;

namespace Audio
{
    
public class AudioManagerController : MonoBehaviour
{

    public static AudioManagerController Instance = null;
    [Header("==== Music File Lists ====")]
    public Sound[] musicSound;
    [Header("==== SFX File Lists ====")]
    public Sound[] sfxSound;

    [Header("==== Audio Source ====")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    void Awake()
    {
        if (Instance == null) { Instance = this; }     
        else { Destroy(gameObject); }
        
    }

    public void PlayMusic(string name)
    {
        foreach (Sound s in musicSound)
        {
            if (s.Name == name)
            {
                musicSource.clip = s.Clip;
                musicSource.Play();
                return;
            }
        }
    }

    public void PlaySFX(string name)

    {
        foreach (Sound s in sfxSound)
        {
            if (s.Name == name)
            {
                sfxSource.clip = s.Clip;
                sfxSource.PlayOneShot(s.Clip);
                return;
            }
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

}





}

