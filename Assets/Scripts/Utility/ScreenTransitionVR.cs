using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TransitionScreenVR : MonoBehaviour
{
    public float transitionWait = 0.5f;
    public Color transitionColor = Color.black;
    private Renderer rend;
    private EventSystem eventSystem;
    private bool wasEventSystemEnabled;

    void Start()
    {
        rend = GetComponent<Renderer>();
        //StartCoroutine(TransitionScreenFadeIn()); // Start with fade in
    }

    public IEnumerator TransitionScreenFadeIn()
    {
        // Disable interactions at start
        eventSystem = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();

        if (eventSystem != null)
        {
            wasEventSystemEnabled = eventSystem.enabled;
            eventSystem.enabled = false;

            Debug.Log("EventSystem disabled: " + eventSystem.enabled);
        }


        yield return StartCoroutine(TransitionScreenFade(alphaStart: 1, alphaEnd: 0, transitionWait));

        if (eventSystem != null)
        {
            eventSystem.enabled = wasEventSystemEnabled;

            Debug.Log("EventSystem Enabled: " + eventSystem.enabled);
        }

        gameObject.SetActive(false);

        //Re-enable interactions at end

    }

    public IEnumerator TransitionScreenFadeOut()
    {
        gameObject.SetActive(true);

        // Disable interactions at start
        if (eventSystem != null)
        {
            wasEventSystemEnabled = eventSystem.enabled;
            eventSystem.enabled = false;

            Debug.Log("EventSystem disabled: " + eventSystem.enabled);
        }


        yield return StartCoroutine(TransitionScreenFade(alphaStart: 0, alphaEnd: 1, transitionWait));
    }

    private IEnumerator TransitionScreenFade(float alphaStart, float alphaEnd, float duration)
    {
        float elapsedTime = 0.0f;
        float elapsedPercentage = 0.0f;

        while (elapsedPercentage < 1.0f)
        {
            elapsedPercentage = elapsedTime / duration;
            Color currentFadeColor = transitionColor;
            currentFadeColor.a = Mathf.Lerp(alphaStart, alphaEnd, elapsedPercentage);

            rend.material.SetColor("_BaseColor", currentFadeColor);
            yield return null;

            elapsedTime += Time.deltaTime;
        }
    }

    public float getTransitionWait()
    {
        return transitionWait;
    }
}
