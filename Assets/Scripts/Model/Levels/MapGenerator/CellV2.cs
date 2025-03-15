using UnityEngine;

namespace Model.Level {
public class CellV2 : MonoBehaviour
{
    
    [System.NonSerialized] public bool collapsed;
    
    public MapPatternV2[] tileOptions;

    public void CreateCell(bool collapsedState, MapPatternV2[] tiles)
    {
        collapsed = collapsedState;
        tileOptions = tiles;
    }

    public void RecreateCell(MapPatternV2[] tiles)
    {
        tileOptions = tiles;

    }
}
}
