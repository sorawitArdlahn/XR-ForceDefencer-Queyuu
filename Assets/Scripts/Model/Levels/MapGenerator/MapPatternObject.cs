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
    public GameObject prefab;
 
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

    public bool playerSpawnAble;
    public bool isFlipped = false;
    public bool isRotated = false;

    public int timeRotated = 0;

    public float GetPrefabHeight()
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab is not assigned.");
            return 0f;
        }

        Renderer renderer = prefab.GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogError("Prefab does not have a Renderer component.");
            return 0f;
        }

        return renderer.bounds.size.y;
    }
}


    
    
}
