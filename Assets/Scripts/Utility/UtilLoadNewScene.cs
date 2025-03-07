using UnityEditor;
using UnityEngine;

public class UtilLoadNewScene : MonoBehaviour
{
    public SceneAsset Scene;
    void Start()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(Scene.name);
    }
}
