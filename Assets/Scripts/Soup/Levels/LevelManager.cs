using System;
using System.Persistence;
using System.Collections.Generic;
using UnityEngine;
using GameController;
using EventListener;
using Mapgenerate;

namespace Spawn {
public class LevelManager : MonoBehaviour, IBind<LevelData>
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

    WaveFunctionCollapseV2 mapGenerator;

    SpawnerManager spawnerManager;

    GameObject player;

    [SerializeField] List<GameObject> enemiesList = new List<GameObject>();

    [SerializeField] private bool forceDebug;

    void Start()
    {
        if (GameObject.FindGameObjectWithTag("SpawnManager") != null)
        {
            spawnerManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnerManager>();
        }

        if (GameObject.FindGameObjectWithTag("MapGenerator") != null)
        {
            mapGenerator = GameObject.FindGameObjectWithTag("MapGenerator").GetComponent<WaveFunctionCollapseV2>();
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

        spawnerManager?.SetEnemies(enemiesList, getCurrentLevel() + 15);
        spawnerManager?.getTotalEnemies();
        //TODO : Sent Enemies Data and Enemies Amount to SpawnerManager
        
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
        //Reset Robot Stats
        //Reset Map
        //Reset Spawner
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

[Serializable]
public class LevelData : ISaveable
{
    [field: SerializeField] public SerializableGuid Id { get; set; }
    public int currentLevel;
    public int highestLevel = 0;
    public int checkpointLevel = 0;
}

}