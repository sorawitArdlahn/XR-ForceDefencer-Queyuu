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
            Debug.LogError("Prefab list is not assigned.");
            return 0f;
        }

        float maxHeight = 0f;
        foreach (GameObject prefab in prefabList)
        {
            Renderer renderer = prefab.GetComponent<Renderer>();
            if (renderer == null)
            {
                // Try to get Renderer from children
                renderer = prefab.GetComponentInChildren<Renderer>();
            }

            if (renderer != null)
            {
                maxHeight = Mathf.Max(maxHeight, renderer.bounds.size.y);
            }
            else
            {
                // Try to get BoxCollider or MeshCollider from children
                BoxCollider boxCollider = prefab.GetComponentInChildren<BoxCollider>();
                if (boxCollider != null)
                {
                    maxHeight = Mathf.Max(maxHeight, boxCollider.bounds.extents.y * 2);
                }
                else
                {
                    MeshCollider meshCollider = prefab.GetComponentInChildren<MeshCollider>();
                    if (meshCollider != null)
                    {
                        maxHeight = Mathf.Max(maxHeight, meshCollider.bounds.extents.y * 2);
                    }
                    else
                    {
                        Debug.LogError("Prefab does not have a Renderer, BoxCollider, or MeshCollider component.");
                    }
                }
            }
        }

        return maxHeight;
    }
}


    
    
}
