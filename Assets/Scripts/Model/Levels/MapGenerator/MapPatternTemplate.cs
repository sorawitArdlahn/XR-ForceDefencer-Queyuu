using System.Collections.Generic;
using UnityEngine;

namespace Model.Level
{
    [CreateAssetMenu(fileName = "MapPatternTemplate", menuName = "WFC/MapPatternTemplate")]
    [System.Serializable]
public class MapPatternTemplate : ScriptableObject
{
    // Start is called before the first frame update
    public string prefabName;
    public MapPatternType patternType;
    [Header("Prefabs")]
    public List<GameObject> prefabList;
 
    [Header("Neighbors")]
    //posX
    [Tooltip("posX")]
    public List<MapPatternType> upNeighbors;
    //negX
    [Tooltip("negX")]
    public List<MapPatternType> downNeighbors;
    //posZ
    [Tooltip("posZ")]
    public List<MapPatternType> leftNeighbors;
    //negZ
    [Tooltip("negZ")]
    public List<MapPatternType> rightNeighbors;

    [Header("Rotations")]
    public bool playerSpawnAble;
    public bool isFlipped = false;
    public bool isRotated = false;

    public int timeRotated = 0;

    public float GetPrefabHeight()
    {
        if (prefabList == null)
        {
            Debug.LogError("Prefab is not assigned.");
            return 0f;
        }

        float maxHeight = 0f;
        foreach (GameObject prefab in prefabList)
        {
            Renderer renderer = prefab.GetComponent<Renderer>();
            if (renderer == null)
            {
            Debug.LogError("Prefab does not have a Renderer component.");
            continue;
            }
            maxHeight = Mathf.Max(maxHeight, renderer.bounds.size.y);
        }

        return maxHeight;
    }
}


    
    
}
