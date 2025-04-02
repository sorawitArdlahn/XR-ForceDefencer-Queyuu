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

    private float originalMusicVolume;

    void Awake()
    {
        if (Instance == null) { Instance = this; }     
        else { Destroy(gameObject); }

        originalMusicVolume = musicSource.volume; // Store the original volume of the music source
        
    }

    public void PlayMusic(string name)
    {
        foreach (Sound s in musicSound)
        {
            if (s.Name == name)
            {
                // Check if the current track is already playing
                if (musicSource.clip == s.Clip && musicSource.isPlaying)
                {
                    return; // Do nothing if the same track is already playing
                }

                float finalVolume = s.Volume * originalMusicVolume; // Combine individual music volume with global music source volume
                musicSource.clip = s.Clip;
                musicSource.volume = finalVolume; // Set the calculated final volume
                musicSource.Play(); // Use the calculated final volume


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
                float finalVolume = s.Volume * sfxSource.volume; // Combine individual SFX volume with global SFX source volume
                sfxSource.PlayOneShot(s.Clip, finalVolume); // Use the calculated final volume
                return;
            }
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
    }

}





}

