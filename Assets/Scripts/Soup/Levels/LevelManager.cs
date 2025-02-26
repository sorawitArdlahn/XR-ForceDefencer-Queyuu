using System.Collections.Generic;
using UnityEngine;
using Gameevent;

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
        if (GameObject.FindGameObjectWithTag("SpawnManager") == null)
        {
            Debug.LogWarning("SpawnManager not found, skipping level setup.");
            return;
        }
        //TODO : Modify Enemies based on level
        spawnerManager.SetEnemies(enemiesList, currentLevel + 15);
        spawnerManager.getTotalEnemies();
        

        //TODO : Sent Enemies Data and Enemies Amount to SpawnerManager
        
    }

    public void ResetStage()
    {
        currentLevel++;
        SetLevelDetailBasedOnLevel();
        OnStartLevel.Raise(this);
        //Reset Robot Stats
        //Reset Map
        //Reset Spawner
    }

}
