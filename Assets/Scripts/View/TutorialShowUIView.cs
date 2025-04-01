using System.Collections;
using Audio;
using GameController;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Utilities.Tweenables.Primitives;

namespace View.Tutorial {

public class TutorialShowUIView : MonoBehaviour
{
    [Header("==== Rotator ====")]
    [SerializeField] GameObject rotator;
    [SerializeField] float rotationDuration = 1f;
    [SerializeField] float rotationAngle = 90f;
    [Header("==== Buttons ====")]
    public Button leftButton;
    public Button rightButton;
    public Button returnButton;

    [Header("==== Event System ====")]
    [SerializeField] EventSystem eventSystem;

    void Start()
    {
        leftButton.onClick.AddListener(OnLeftButtonClicked);
        rightButton.onClick.AddListener(OnRightButtonClicked);
        returnButton.onClick.AddListener(OnReturnButtonClicked);
    }

    private void OnLeftButtonClicked()
    {
        AudioManagerController.Instance.PlaySFX("ButtonPressed");
        StartCoroutine(RotateObject(right : true)); // Rotate left
        Debug.Log("Left Button Pressed.");
    }

    private void OnRightButtonClicked()
    {
        AudioManagerController.Instance.PlaySFX("ButtonPressed");
        StartCoroutine(RotateObject(right : false)); // Rotate right
        Debug.Log("Right Button Pressed.");
    }

    private void OnReturnButtonClicked()
    {
        AudioManagerController.Instance.PlaySFX("ButtonPressed");
        GameStateManager.Instance.SetNextPhase(GameState.MainMenu);
        Debug.Log("Return Button Pressed.");
    }

    private IEnumerator RotateObject(bool right)
    {
        //Disable EventSystem to prevent interaction during rotation
        eventSystem.enabled = false;
        returnButton.gameObject.SetActive(false);
        leftButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(false); //Disable buttons during rotation
        //Start rotation coroutine

        float targetAngle = right ? rotationAngle : -rotationAngle;
        float startAngle = rotator.transform.eulerAngles.y;
        float endAngle = startAngle + targetAngle;

        float elapsedTime = 0f;
        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / rotationDuration);
            float angle = Mathf.LerpAngle(startAngle, endAngle, t);
            rotator.transform.eulerAngles = new Vector3(rotator.transform.eulerAngles.x, angle, rotator.transform.eulerAngles.z);
            yield return null;
        }

        rotator.transform.eulerAngles = new Vector3(rotator.transform.eulerAngles.x, endAngle, rotator.transform.eulerAngles.z);

        eventSystem.enabled = true; //Re-enable EventSystem after rotation
        returnButton.gameObject.SetActive(true);
        leftButton.gameObject.SetActive(true);
        rightButton.gameObject.SetActive(true); //Re-enable buttons after rotation

        Debug.Log("Rotation complete."); 

    }


}

}
