using System.Collections.Generic;
using UnityEngine;

namespace Model.Level {
public class MapPatternV2
{
    // Start is called before the first frame update
    public MapPatternTemplate originalMapPattern;

    string prefabName;

    public List<MapPatternType> upNeighbors;
    public List<MapPatternType> downNeighbors;
    public List<MapPatternType> leftNeighbors;
    public List<MapPatternType> rightNeighbors;

    public bool playerSpawnAble;

    private int timeRotated;

    public MapPatternV2(MapPatternTemplate originalMapPattern)
    {
        this.originalMapPattern = originalMapPattern;
        this.prefabName = originalMapPattern.prefabName;
        this.upNeighbors = originalMapPattern.upNeighbors;
        this.downNeighbors = originalMapPattern.downNeighbors;
        this.leftNeighbors = originalMapPattern.leftNeighbors;
        this.rightNeighbors = originalMapPattern.rightNeighbors;
        this.timeRotated = originalMapPattern.timeRotated;
        this.playerSpawnAble = originalMapPattern.playerSpawnAble
        ;
    }

    public MapPatternV2(
        MapPatternV2 mapPattern,
        List<MapPatternType> upNeighbors,
        List<MapPatternType> downNeighbors,
        List<MapPatternType> leftNeighbors,
        List<MapPatternType> rightNeighbors,
        bool playerSpawnAble)
    {
        this.originalMapPattern = mapPattern.originalMapPattern;
        this.timeRotated = mapPattern.timeRotated + 1;
        this.prefabName = originalMapPattern.prefabName + "_rotated_" + timeRotated;
        this.upNeighbors = upNeighbors;
        this.downNeighbors = downNeighbors;
        this.leftNeighbors = leftNeighbors;
        this.rightNeighbors = rightNeighbors;
        this.playerSpawnAble = playerSpawnAble;
    }

    public string GetPrefabName()
    {
        return prefabName;
    }

    public MapPatternType GetThisPatternType()
    {
        return originalMapPattern.patternType;
    }

    public GameObject GetPrefab()
    {
        return originalMapPattern.prefabList[Random.Range(0, originalMapPattern.prefabList.Count)];
    }

    public Quaternion GetPrefabRotation()
    {
        GameObject prefab = GetPrefab();
        if (prefab != null)
        {
            return prefab.transform.rotation;
        }
        return Quaternion.identity;;
    }

    public int getRotatedAngle() {
        return timeRotated * 90;
    }


}


}

