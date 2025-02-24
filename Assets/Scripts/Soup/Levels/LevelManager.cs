using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Spawn;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameEvent OnStartLevel;

    SpawnerManager spawnerManager;

    [SerializeField] List<GameObject> enemiesList = new List<GameObject>();


    int currentLevel = 0;

    void Start()
    {
        ResetStage();
      
    }

    public int getCurrentLevel()
    {
        return currentLevel;
    }

    public void setCurrentLevel(int level)
    {
        currentLevel = level;
    }

    void SetLevelDetailBasedOnLevel() {
        spawnerManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnerManager>();
        //TODO : Modify Enemies based on level
        spawnerManager.SetEnemies(enemiesList, currentLevel + 15);
        

        //TODO : Sent Enemies Data and Enemies Amount to SpawnerManager
        
    }

    public void ResetStage()
    {
        currentLevel++;
        SetLevelDetailBasedOnLevel();
        spawnerManager.getTotalEnemies();
        OnStartLevel.Raise(this);
        //Reset Robot Stats
        //Reset Map
        //Reset Spawner
    }

}
