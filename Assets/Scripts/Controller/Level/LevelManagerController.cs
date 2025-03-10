using Model.Level;
using System.Persistence;
using System.Collections.Generic;
using UnityEngine;
using GameController;
using EventListener;

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


    public GameEvent OnStartLevel;

    MapGeneratorController mapGenerator;

    SpawnerManagerController spawnerManager;

    GameObject player;

    [SerializeField] List<GameObject> enemiesList = new List<GameObject>();

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
        }

        if (GameStateManager.Instance?.GetCurrentGameState() == GameState.InBattle || forceDebug)
        {
            ResetStage();
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

    public void LevelCompleted() {
        //TODO : Level Completed
        Debug.LogWarning("Level Completed!");
    }

    public void setPlayerPositionOnMap() {
        Vector3 playerSpawnPosition = mapGenerator.GetPlayerSpawnPosition();
        playerSpawnPosition.y += 15;
        player.transform.position = playerSpawnPosition;

    }

    public void ResetStage()
    {
        setCurrentLevel(getCurrentLevel() + 1);
        SetLevelDetailBasedOnCurrentLevel();
        OnStartLevel?.Raise(this);
    }

    void calculateCheckpointLevel() {
        //TODO : Calculate Checkpoint Level
        //if current level does not reach previous record, 
        if (data.highestLevel > getCurrentLevel()) return;

        data.highestLevel = getCurrentLevel();
        data.checkpointLevel = getCurrentLevel() - 2;
    }

    //TO/do : Turn these below method into UI class or somewhere else

    public void backToPreparation() {
        calculateCheckpointLevel();
        GameStateManager.Instance.SetNextPhase(GameState.BattlePreparation);
    }

    public void GameOver() {
        //TODO : Game Over
        GameManager.Instance.DeleteGame();
        GameStateManager.Instance.SetNextPhase(GameState.MainMenu);
    }

}

}