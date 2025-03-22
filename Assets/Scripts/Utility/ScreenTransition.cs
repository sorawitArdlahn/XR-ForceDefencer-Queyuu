using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Audio;

public class ScreenTransition : MonoBehaviour
{
    public float transitionWait = 0.5f;
    private Image TransitionScreenImage;

    void Awake()
    {
        TransitionScreenImage = GetComponentInChildren<Image>();
        gameObject.SetActive(false);
    }

    public IEnumerator TransitionScreenFadeIn() {
            Color startColor = new Color(TransitionScreenImage.color.r,
            TransitionScreenImage.color.g,
            TransitionScreenImage.color.b,
            1);

            Color endColor = new Color(TransitionScreenImage.color.r,
            TransitionScreenImage.color.g,
            TransitionScreenImage.color.b,
            0);

            yield return StartCoroutine(TransitionScreenFade(startColor, endColor, transitionWait));

            gameObject.SetActive(false);
    }

    public IEnumerator TransitionScreenFadeOut() {
        gameObject.SetActive(true);

        Color startColor = new Color(TransitionScreenImage.color.r,
        TransitionScreenImage.color.g,
        TransitionScreenImage.color.b,
        0);

        Color endColor = new Color(TransitionScreenImage.color.r,
        TransitionScreenImage.color.g,
        TransitionScreenImage.color.b,
        1);

            yield return StartCoroutine(TransitionScreenFade(startColor, endColor, transitionWait));
    }

    private IEnumerator TransitionScreenFade(Color start, Color end, float duration) {
        float elapsedTime = 0.0f;
        float elapsedPercentage = 0.0f;

        while (elapsedPercentage < 1.0f) {

            elapsedPercentage = elapsedTime / duration;
            TransitionScreenImage.color = Color.Lerp(start, end, elapsedPercentage);
            yield return null;

            elapsedTime += Time.deltaTime;
        }

    }

    public float getTransitionWait() {
        return transitionWait;
    }
}
