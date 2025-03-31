using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using EventListener;
using System;
using Model.Level;

namespace Controller.Level {
public class MapGeneratorController : MonoBehaviour
{

    //size of the map : eg 3 is 3x3
    [Header("Map Size")]
    public int dimensions;
    [Header("Map Patterns List")]

    public List<MapPatternSet> tileSet;
    List<MapPatternTemplate> templateList;
    private MapPatternV2[] tileObjects = new MapPatternV2[0];
    [NonSerialized] public List<CellV2> gridComponent;

    [Header("Cell Object Prefab")]
    public CellV2 cellV2Object;

    //size of the cellV2 : eg 1 is 1x1
    public float cellV2Size;

    private MapPatternV2 backupTile;
    private int iteration;

    private int backupUsedCount = 0;

    protected GameObject gridParent;

    private Dictionary<string, int> tileTypeFrequencies = new Dictionary<string, int>();
    private Dictionary<string, float> tileTypeWeights = new Dictionary<string, float>();

    [Header("Map Wall")]

    public GameObject wallPrefab;

    [SerializeField] private bool CreateWall;

    protected float borderHeight;

    private GameObject wallParent;


    //[SerializeField]
    //NavMeshSurfaceComponent
    private NavMeshSurface navMeshSurface;

    [Header("Events")]
    public GameEvent mapGenerated;
    public bool IsInitialized { get; private set; }

    public Vector3 GetPlayerSpawnPosition() {
        foreach (CellV2 cell in gridComponent) {
            if (cell.tileOptions[0].originalMapPattern.playerSpawnAble) {
                return cell.transform.position;
            }
        }
        return Vector3.zero;
    }

    public int getMapSize() {
        //Dimension is the size of the map
        return dimensions * dimensions;
    }

    public void setMapSize(int size) {
        //Dimension is the size of the map
        dimensions = size;
    }

    public void Awake() {
        if (mapGenerated == null)
        {
            Debug.Log("--------------------------> mapGenerated is null");
        }

        gridComponent = new List<CellV2>();

        gridParent = new GameObject("GeneratedMap");


        //Set Wall Parent
        wallParent = new GameObject("GeneratedWalls");
    }

    public void Start()
    {
        navMeshSurface = gridParent.AddComponent<NavMeshSurface>();
        navMeshSurface.useGeometry = UnityEngine.AI.NavMeshCollectGeometry.PhysicsColliders;      
    }

    void InitializeGrid() {


        for (int y = 0; y < dimensions; y++) {
            for (int x = 0; x < dimensions; x++) {
                Vector3 CellV2Position = new Vector3(x * cellV2Size, 0, y * cellV2Size);
                CellV2 newCellV2 = Instantiate(cellV2Object, CellV2Position, Quaternion.identity);
                newCellV2.CreateCell(false, tileObjects);
                newCellV2.transform.SetParent(gridParent.transform);
                gridComponent.Add(newCellV2);
            }
        }

        if (CreateWall && wallPrefab != null) {
            setBorderHeight();
            CreateBorderWalls();
        }
        StartCoroutine(CheckEntropy());

    }

    IEnumerator BuildNavMesh() {
        yield return new WaitForSeconds(0.5f);
        navMeshSurface.collectObjects = CollectObjects.Children;
        navMeshSurface.BuildNavMesh();
        IsInitialized = true;
        mapGenerated.Raise(this);
    }

    void setBorderHeight() {
        borderHeight = templateList.Max(template => template.GetPrefabHeight()) * 1.5f;
    }

    
    void CreateBorderWalls() {
    // Bottom border
    GameObject bottomWall = Instantiate(wallPrefab, new Vector3((dimensions - 1) * cellV2Size / 2, borderHeight / 2, -1 * cellV2Size / 2), Quaternion.identity, wallParent.transform);
    bottomWall.transform.localScale = new Vector3(dimensions * cellV2Size, borderHeight, bottomWall.transform.localScale.z);

    // Top border
    GameObject topWall = Instantiate(wallPrefab, new Vector3((dimensions - 1) * cellV2Size / 2, borderHeight / 2, dimensions * cellV2Size - cellV2Size / 2), Quaternion.identity, wallParent.transform);
    topWall.transform.localScale = new Vector3(dimensions * cellV2Size, borderHeight, topWall.transform.localScale.z);

    // Left border
    GameObject leftWall = Instantiate(wallPrefab, new Vector3(-1 * cellV2Size / 2, borderHeight / 2, (dimensions - 1) * cellV2Size / 2), Quaternion.identity, wallParent.transform);
    leftWall.transform.localScale = new Vector3(leftWall.transform.localScale.x, borderHeight, dimensions * cellV2Size);

    // Right border
    GameObject rightWall = Instantiate(wallPrefab, new Vector3(dimensions * cellV2Size - cellV2Size / 2, borderHeight / 2, (dimensions - 1) * cellV2Size / 2), Quaternion.identity, wallParent.transform);
    rightWall.transform.localScale = new Vector3(rightWall.transform.localScale.x, borderHeight, dimensions * cellV2Size);

    // Top cover
    GameObject topCover = Instantiate(wallPrefab, new Vector3((dimensions - 1) * cellV2Size / 2, borderHeight, (dimensions - 1) * cellV2Size / 2), Quaternion.Euler(90, 0, 0), wallParent.transform);
    topCover.transform.localScale = new Vector3(dimensions * cellV2Size, dimensions * cellV2Size, topCover.transform.localScale.z);
    }

    //check for item with the least varations
    IEnumerator CheckEntropy() {
        List<CellV2> tempGrid = new List<CellV2>(gridComponent);

        int minEntropy = int.MaxValue;
        foreach(CellV2 c in tempGrid)
        {
        if(c.collapsed) continue;
        if(c.tileOptions.Length < minEntropy) 
            minEntropy = c.tileOptions.Length;
        }

        for(int i = tempGrid.Count-1; i >= 0; i--)
        {
        if(tempGrid[i].collapsed || tempGrid[i].tileOptions.Length != minEntropy)
            tempGrid.RemoveAt(i);
        }


        yield return null;
        AdjustWeightsBasedOnTypeFrequencies();
        CollapseCellV2(tempGrid);

    }

    void CollapseCellV2(List<CellV2> tempGrid) {
        int randIndex = UnityEngine.Random.Range(0, tempGrid.Count);

        CellV2 cellV2ToCollapse = tempGrid[randIndex];

        cellV2ToCollapse.collapsed = true;

        if (cellV2ToCollapse.tileOptions.Length == 0) {
            MapPatternV2 selectedTile = backupTile;
            cellV2ToCollapse.tileOptions = new MapPatternV2[] { selectedTile };
            Debug.Log("Backup Tile Used Times =" + backupUsedCount);
            backupUsedCount++;
        }
        else {
            string selectedType = SelectTileType(cellV2ToCollapse.tileOptions.ToList());
            cellV2ToCollapse.tileOptions = cellV2ToCollapse.tileOptions.Where(tile => tile.GetThisPatternType().GetTypeName() == selectedType).ToArray();
            
            MapPatternV2 selectedTile = cellV2ToCollapse.tileOptions[UnityEngine.Random.Range(0, cellV2ToCollapse.tileOptions.Length)];
            cellV2ToCollapse.tileOptions = new MapPatternV2[] { selectedTile };
        }

        if (backupUsedCount > dimensions * 0.35) {
            Debug.Log("Backup Tile Used More Than " + backupUsedCount + " Times\nResetting Map....");
            ResetMap();
            return;
        }

        
        
        MapPatternV2 foundtile = cellV2ToCollapse.tileOptions[0];

        Vector3 adjustedPosition = new Vector3(
            cellV2ToCollapse.transform.position.x, 
            cellV2ToCollapse.transform.position.y, 
            cellV2ToCollapse.transform.position.z
            );

        
        //SOUP : Spawn Object / Tile / PreFab
        GameObject spawnedObject = Instantiate(
        //The object to be spawned
        foundtile.GetPrefab(),
        //The position of the object to be spawned
        adjustedPosition,
        //The rotation of the object to be spawned
        Quaternion.Euler(
        foundtile.GetPrefabRotation()
        .eulerAngles
        + new Vector3(0, foundtile.getRotatedAngle(), 0))

        );


        spawnedObject.transform.SetParent(cellV2ToCollapse.transform);

        UpdateGeneration();

    }

    //updating possible tiles & validating of existing tiles
    void UpdateGeneration() {
        List<CellV2> newGenerationCellV2 = new List<CellV2>(gridComponent);

        for (int y = 0; y < dimensions; y++) {
            
            for (int x = 0; x < dimensions; x++) {

                var index = x + y * dimensions;

                if (gridComponent[index].collapsed) {
                    newGenerationCellV2[index] = gridComponent[index];
                }
                else {

                    
                    //get all possible tiles
                    List<MapPatternV2> options = new List<MapPatternV2>();
                    foreach (MapPatternV2 t in tileObjects) {      
                        options.Add(t);
                    }
                

                    //up

                    
                    
                    if (y > 0) {
                        //check 2 conditions
                        /*
                        1. if options' item type matched up's downNeighbors type
                        2. if options' item upNeighbors is up's type
                        3. remove item that does not satisfy the conditions
                        */

                        CellV2 up = gridComponent[x + (y-1) * dimensions];
                        HashSet<MapPatternType> directionNeighbor = new HashSet<MapPatternType>();
                        HashSet<MapPatternType> directionType = new HashSet<MapPatternType>();

                        foreach (MapPatternV2 possibleOptions in up.tileOptions) {

                            foreach (MapPatternType type in possibleOptions.downNeighbors) {
                                directionNeighbor.Add(type);
                            }
                            
                            directionType.Add(possibleOptions.GetThisPatternType());
                        }
                        CheckValidity(options, directionNeighbor, directionType, "up");


                        
                    }

                    
                    //down
                    if (y < dimensions - 1) {
                        CellV2 down = gridComponent[x + (y+1) * dimensions];
                        HashSet<MapPatternType> directionNeighbor = new HashSet<MapPatternType>();
                        HashSet<MapPatternType> directionType = new HashSet<MapPatternType>();

                        foreach (MapPatternV2 possibleOptions in down.tileOptions) {
                            foreach (MapPatternType type in possibleOptions.upNeighbors) {
                                directionNeighbor.Add(type);
                            }
                            directionType.Add(possibleOptions.GetThisPatternType());
                        }
                        CheckValidity(options, directionNeighbor, directionType, "down");
                    }

                    //left
                    if (x < dimensions - 1) {
                        CellV2 left = gridComponent[(x + 1) + y * dimensions];
                        HashSet<MapPatternType> directionNeighbor = new HashSet<MapPatternType>();
                        HashSet<MapPatternType> directionType = new HashSet<MapPatternType>();

                        foreach (MapPatternV2 possibleOptions in left.tileOptions) {
                            foreach (MapPatternType type in possibleOptions.rightNeighbors) {
                                directionNeighbor.Add(type);
                            }
                        
                            directionType.Add(possibleOptions.GetThisPatternType());
                        }
                        CheckValidity(options, directionNeighbor, directionType, "left");
                    }

                    //right
                    if (x > 0) {
                        CellV2 right = gridComponent[(x - 1) + y * dimensions];
                        HashSet<MapPatternType> directionNeighbor = new HashSet<MapPatternType>();
                        HashSet<MapPatternType> directionType = new HashSet<MapPatternType>();

                        foreach (MapPatternV2 possibleOptions in right.tileOptions) {
                            foreach (MapPatternType type in possibleOptions.leftNeighbors) {
                                directionNeighbor.Add(type);
                            }
                            directionType.Add(possibleOptions.GetThisPatternType());
                        }
                        CheckValidity(options, directionNeighbor, directionType, "right");
                    }
                    
        
                    MapPatternV2[] newTileList = new MapPatternV2[options.Count];

                    for(int i = 0; i < options.Count; i++) {
                        newTileList[i] = options[i];
                    }

                    newGenerationCellV2[index].RecreateCell(newTileList);



                }
            }
        }

        gridComponent = newGenerationCellV2;
        iteration++;
        if (iteration < dimensions * dimensions) {
            StartCoroutine(CheckEntropy());
        }
        else {
            StartCoroutine(BuildNavMesh());
        }

    }

    void CheckValidity(List<MapPatternV2> optionList, HashSet<MapPatternType> NeighborList, HashSet<MapPatternType> TypeList, string direction=null) {

        if (direction == "up") {
        for (int x = optionList.Count - 1; x >= 0; x--) {
            var element = optionList[x];
            if (!NeighborList.Contains(element.GetThisPatternType()) || !element.upNeighbors.Any(TypeList.Contains)) {
                optionList.RemoveAt(x);
            }
        }
        }

        else if (direction == "down") {
        for (int x = optionList.Count - 1; x >= 0; x--) {
            var element = optionList[x];
            if (!NeighborList.Contains(element.GetThisPatternType()) || !element.downNeighbors.Any(TypeList.Contains)) {
                optionList.RemoveAt(x);
            }
        }
        }

        else if (direction == "left") {
        for (int x = optionList.Count - 1; x >= 0; x--) {
            var element = optionList[x];
            if (!NeighborList.Contains(element.GetThisPatternType()) || !element.leftNeighbors.Any(TypeList.Contains)) {
                optionList.RemoveAt(x);
            }
        }
        }

        else if (direction == "right") {
        for (int x = optionList.Count - 1; x >= 0; x--) {
            var element = optionList[x];
            if (!NeighborList.Contains(element.GetThisPatternType()) || !element.rightNeighbors.Any(TypeList.Contains)) {
                optionList.RemoveAt(x);
            }
        }
        }

        
    }

    
    // SOUP : Added Rotating Section
    private void InitializeTileObjects() {
        
        tileObjects = new MapPatternV2[0];

        for (int obj = templateList.Count - 1; obj >= 0; obj--) {
            MapPatternTemplate tile = templateList[obj];
            MapPatternV2 newTile = new MapPatternV2(tile);
            tileObjects = tileObjects.Concat(new MapPatternV2[] { newTile }).ToArray();
            if (tile.isRotated) {
                for (int i = 0; i < 3; i++) {
                    MapPatternV2 rotatedTile = new MapPatternV2(
                        newTile,
                        upNeighbors: newTile.leftNeighbors,
                        downNeighbors: newTile.rightNeighbors,
                        leftNeighbors: newTile.downNeighbors,
                        rightNeighbors: newTile.upNeighbors,
                        playerSpawnAble: newTile.playerSpawnAble
                    );
                    newTile = rotatedTile;
                    tileObjects = tileObjects.Concat(new MapPatternV2[] { rotatedTile }).ToArray();
                    
                }
            }
            else if (tile.isFlipped) {
                MapPatternV2 rotatedTile = new MapPatternV2(
                    newTile,
                    upNeighbors: newTile.rightNeighbors,
                    downNeighbors: newTile.leftNeighbors,
                    leftNeighbors: newTile.upNeighbors,
                    rightNeighbors: newTile.downNeighbors,
                    playerSpawnAble: newTile.playerSpawnAble
                );
                tileObjects = tileObjects.Concat(new MapPatternV2[] { rotatedTile }).ToArray();
            }
        }
        backupTile = tileObjects[0];
    }
    

    // SOUP : Added Weighting Section

    private void NormalizeWeights()
    {
        float totalWeight = tileTypeWeights.Values.Sum();
        foreach (string key in tileTypeWeights.Keys.ToList())
        {
            tileTypeWeights[key] /= totalWeight;
        }
    }

    void InitializeTileTypeFrequencies()
    {

    tileTypeFrequencies.Clear();
    
    foreach (var tile in tileObjects)
        {
            string typeName = tile.GetThisPatternType().GetTypeName();
            tileTypeFrequencies[typeName] = 1;
        }
    }


    private string SelectTileType(List<MapPatternV2> options)
    {
        List<string> optionType = options.Select(option => option.GetThisPatternType().GetTypeName()).ToList();
        float totalWeight = tileTypeWeights
            .Where(kvp => optionType.Contains(kvp.Key))
            .Sum(kvp => kvp.Value);

        float randomValue = UnityEngine.Random.Range(0, totalWeight);


        foreach (string type in optionType) 
        {
            if (randomValue < tileTypeWeights[type])
            {
                tileTypeFrequencies[type]++;
                return type;
            }
            randomValue -= tileTypeWeights[type];

        }
        return optionType.First(); // Fallback in case of rounding errors     
    }
    

    
    private void AdjustWeightsBasedOnTypeFrequencies()
    {
        int totalFrequency = tileTypeFrequencies.Values.Sum();
        foreach (string key in tileTypeFrequencies.Keys.ToList())
        {
            tileTypeWeights[key] = 1.0f / tileTypeFrequencies[key];
            
        }

    //normalize weight to 1
    NormalizeWeights();
    
    }

    //Initialize the map
    public void ResetMap() {


        foreach (Transform child in gridParent.transform) {
            Destroy(child.gameObject);
        }

        foreach (Transform child in wallParent.transform) {
            Destroy(child.gameObject);
        }

        //Select MapSet Prefab
        templateList = tileSet[UnityEngine.Random.Range(0, tileSet.Count)].mapPatternTemplatesInSet;

        InitializeTileObjects();
        InitializeTileTypeFrequencies();

        navMeshSurface.RemoveData();

        gridComponent.Clear();
        iteration = 0;
        backupUsedCount = 0;
        IsInitialized = false;
        InitializeGrid();
    }
    
    

}
}
