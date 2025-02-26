using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapPatternType", menuName = "WFC/MapPatternType")]
[System.Serializable]
public class MapPatternType : ScriptableObject
{
    public string typeName;
    public List<MapPatternType> connectedTypes;

    public string GetTypeName()
    {
        return typeName;
    }

    public List<MapPatternType> GetConnectedTypes()
    {
        return connectedTypes;
    }
}
