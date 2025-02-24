using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapPatternSet", menuName = "WFC/MapPatternSet")]
public class MapPatternSet : ScriptableObject
{
    [SerializeField] public List<MapPatternTemplate> mapPatternTemplatesInSet;
}


