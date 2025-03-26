using System;
using System.Collections;
using UnityEngine;

namespace Presenter.Sound
{
    public class AudioManagerV2 : MonoBehaviour
    {
        public AudioSource audioSource;
        public float fadeDuration = 0.3f;

        private AudioClip currentClip;
        private bool isCanPlaying;

        private void Start()
        {
            isCanPlaying = false;
            StartCoroutine(WaitForClip());
        }

        public void PlayNextClip(AudioClip nextClip, float volume = 1.0f)
        {
            if (!isCanPlaying) return;
            if (nextClip == currentClip) return;
            
            audioSource.playOnAwake = false;
            audioSource.loop = true;
            
            // audioSource.clip = nextClip;
            // currentClip = nextClip;
            //
            // if (nextClip)
            // {
            //     audioSource.Play();
            // }
            

            // เปลี่ยนเสียงโดยใช้ Fade
            StartCoroutine(FadeToNewSound(nextClip, volume));
        }
        
        public void Stop()
        {
            // audioSource.Stop();
            // audioSource.clip = null;
            // currentClip = null;
            StartCoroutine(FadeToNewSound(null, 0.5f));
        }
        
        private IEnumerator FadeToNewSound(AudioClip newClip , float targetVolume)
        {

            float startVolume = audioSource.volume;
            float elapsedTime = 0f;

            // ค่อยๆ ลดเสียงลงก่อนเปลี่ยน (Fade Out)
            while (elapsedTime < fadeDuration)
            {
                audioSource.volume = Mathf.Lerp(startVolume, 0, elapsedTime / fadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            audioSource.Stop();
            audioSource.clip = newClip;
            currentClip = newClip;

            if (newClip != null)
            {
                audioSource.Play();
                elapsedTime = 0f;

                // ค่อยๆ เพิ่มเสียงขึ้น (Fade In)
                while (elapsedTime < fadeDuration)
                {
                    audioSource.volume = Mathf.Lerp(0, targetVolume, elapsedTime / fadeDuration);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }

            audioSource.volume = targetVolume; // ตั้งค่าปิดท้ายให้แน่นอน
        }

        private IEnumerator WaitForClip()
        {
            yield return new WaitForSeconds(3);
            isCanPlaying = true;
        }
    }
}

