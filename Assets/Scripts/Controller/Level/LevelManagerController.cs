using Model;
using Model.Level;
using System.Persistence;
using System.Collections.Generic;
using UnityEngine;
using GameController;
using EventListener;
using System;

namespace Controller.Level {
public class LevelManagerController : MonoBehaviour, IBind<LevelData>
{
    // Start is called before the first frame update
    [field: SerializeField] public SerializableGuid Id { get; set; } = SerializableGuid.NewGuid();
    [SerializeField] LevelData data;

    public void Bind(LevelData data)
    {
        this.data = data;
        this.data.Id = Id;
        this.data.currentLevel = data.checkpointLevel;
    }

    MapGeneratorController mapGenerator;

    SpawnerManagerController spawnerManager;

    GameObject player;

    [Header("UI")]
    [SerializeField] GameObject finishExplorationUIView;
    [SerializeField] GameObject gameOverUIView;

    [Header("Enemies")]

    [SerializeField] List<GameObject> enemiesList = new List<GameObject>();

    [Header("Events")]
    public GameEvent OnStartLevel;

    [Header("Debug")]

    [SerializeField] private bool forceDebug;

    void Start()
    {

        if (GameObject.FindGameObjectWithTag("SpawnManager") != null)
        {
            spawnerManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnerManagerController>();
        }

        if (GameObject.FindGameObjectWithTag("MapGenerator") != null)
        {
            mapGenerator = GameObject.FindGameObjectWithTag("MapGenerator").GetComponent<MapGeneratorController>();
        }

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player.TryGetComponent(out IDamageable damageable))
            {
            damageable.OnDeath += OnPlayerDeath;
            }
        }

        if (GameStateManager.Instance?.GetCurrentGameState() == GameState.InBattle || forceDebug)
        {
            NewStage();
        }
    }

    public int getCurrentLevel()
    {
        return data.currentLevel;
    }

    public void setCurrentLevel(int level)
    {
        data.currentLevel = level;
    }

    void SetLevelDetailBasedOnCurrentLevel() {
        //Dynamically Increase Map Size over time
        int additionalSize = Mathf.FloorToInt(Mathf.Log(getCurrentLevel() + 1, 3));
        mapGenerator?.setMapSize(additionalSize + 5);

        //Dynamically Increase Enemy Amount over time
        //TODO : Sent Enemies Data and Enemies Amount to SpawnerManager
        int enemyCount = Mathf.Min(
            getCurrentLevel() + 15, 
            (int)(mapGenerator.getMapSize() * 0.75)
            );
        spawnerManager?.SetEnemies(enemiesList, enemyCount);
        spawnerManager?.getTotalEnemies();
        
    }

    public void setPlayerPositionOnMap() {
        Vector3 playerSpawnPosition = mapGenerator.GetPlayerSpawnPosition();
        playerSpawnPosition.y += 15;
        player.transform.position = playerSpawnPosition;

        StartCoroutine(GameStateManager.Instance.TransitionScreen.TransitionScreenFadeIn());

    }

    public void NewStage()
    {
        finishExplorationUIView.SetActive(false);
        setCurrentLevel(getCurrentLevel() + 1);
        SetLevelDetailBasedOnCurrentLevel();
        OnStartLevel?.Raise(this);
    }

    public void calculateCheckpointLevel() {
        //TODO : Calculate Checkpoint Level
        //if current level does not reach previous record, 
        if (data.highestLevel > getCurrentLevel()) {
            data.checkpointLevel = Math.Max(data.checkpointLevel - 1, 1);
        }
        else {
            data.highestLevel = getCurrentLevel();
            data.checkpointLevel = Math.Max(data.highestLevel - 5, 1);
        }
    }

    //Post Match
    public void LevelCompleted() {
        finishExplorationUIView.SetActive(true);
        Debug.LogWarning("Level Completed!");
    }

    public void OnPlayerDeath() {
        gameOverUIView.SetActive(true);
        Debug.LogWarning("Game Over!");
    }
}

}