using System;
using System.Persistence;
using System.Collections.Generic;
using UnityEngine;
using GameController;
using Gameevent;

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

    SpawnerManager spawnerManager;

    [SerializeField] List<GameObject> enemiesList = new List<GameObject>();

    void Start()
    {
        //ResetStage();
      
    }

    public int getCurrentLevel()
    {
        return data.currentLevel;
    }

    public void setCurrentLevel(int level)
    {
        data.currentLevel = level;
    }

    void SetLevelDetailBasedOnLevel() {

        if (GameObject.FindGameObjectWithTag("SpawnManager") == null)
        {
            Debug.LogWarning("SpawnManager not found, skipping level setup.");
            return;
        }
        spawnerManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnerManager>();
        //TODO : Modify Enemies based on level
        spawnerManager.SetEnemies(enemiesList, getCurrentLevel() + 15);
        spawnerManager.getTotalEnemies();
        

        //TODO : Sent Enemies Data and Enemies Amount to SpawnerManager
        
    }

    public void ResetStage()
    {
        setCurrentLevel(getCurrentLevel() + 1);
        SetLevelDetailBasedOnLevel();
        OnStartLevel.Raise(this);
        //Reset Robot Stats
        //Reset Map
        //Reset Spawner
    }

    public void backToPreparation() {
        calculateCheckpointLevel();
        GameStateManager.Instance.SetNextPhase(GameState.BattlePreparation);
    }

    void calculateCheckpointLevel() {
        //TODO : Calculate Checkpoint Level
        //if current level does not reach previous record, 
        if (data.highestLevel > getCurrentLevel()) return;

        data.highestLevel = getCurrentLevel();
        data.checkpointLevel = getCurrentLevel() - 2;
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