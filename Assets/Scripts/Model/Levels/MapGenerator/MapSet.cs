using System.Collections.Generic;
using UnityEngine;

namespace Model.Level
{
    [CreateAssetMenu(fileName = "MapPatternSet", menuName = "WFC/MapPatternSet")]
    public class MapPatternSet : ScriptableObject
    {
        [SerializeField] public List<MapPatternTemplate> mapPatternTemplatesInSet;
    }
}


