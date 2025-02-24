using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class CellV2 : MonoBehaviour
{
    public bool collapsed;
    
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
