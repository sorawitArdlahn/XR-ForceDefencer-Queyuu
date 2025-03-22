using Model;
using Model.Level;
using System.Persistence;
using System.Collections.Generic;
using UnityEngine;
using GameController;
using EventListener;
using System;
using Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Controller.Level {
public class LevelManagerController : MonoBehaviour, IBind<LevelData>
{
    // Start is called before the first frame update
    [field: SerializeField] public SerializableGuid Id { get; set; } = SerializableGuid.NewGuid();

    [Header("==== Level Data ====")]
    [SerializeField] LevelData data;

    public void Bind(LevelData data)
    {
        this.data = data;
        this.data.Id = Id;
        this.data.currentLevel = data.checkpointLevel;
    }

    [Header("==== Map & Spawn Manager ====")]

    MapGeneratorController mapGenerator;

    SpawnerManagerController spawnerManager;

    GameObject player;

    [Header("==== UI ====")]
    [SerializeField] GameObject finishExplorationUIView;
    [SerializeField] GameObject gameOverUIView;

    [Header("==== Button Link to Other Screen ====")]
    public Button ContinueExplorationButton;
    public Button GameOverButton;

    [Header("==== Enemies List ====")]

    [SerializeField] List<GameObject> enemiesList = new List<GameObject>();

    [Header("==== Event Listener ====")]
    public GameEvent OnStartLevel;

    [Header("==== Event System ====")]
    public EventSystem eventSystem;

    [Header("==== Debug ====")]

    [SerializeField] private bool forceDebug;
    [SerializeField] private int baseMapSize = 5;
    [SerializeField] int baseEnemy = 15;

    [NonSerialized] public LevelManagerController instance = null;

    void Start()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }

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
        int additionalSize = Mathf.FloorToInt(Mathf.Log(getCurrentLevel() + 1, 2));
        mapGenerator?.setMapSize(additionalSize + baseMapSize);

        //Dynamically Increase Enemy Amount over time
        //TODO : Sent Enemies Data and Enemies Amount to SpawnerManager
        int enemyCount = Mathf.Min(
            getCurrentLevel() + baseEnemy, 
            (int)(mapGenerator.getMapSize() * 0.75)
            );
        spawnerManager?.SetEnemies(enemiesList, enemyCount);
        spawnerManager?.getTotalEnemies();
        
    }

    public void setPlayerPositionOnMap() {
        Vector3 playerSpawnPosition = mapGenerator.GetPlayerSpawnPosition();
        playerSpawnPosition.y += 15;
        player.transform.position = playerSpawnPosition;

        if (GameStateManager.Instance != null) 
        {StartCoroutine(GameStateManager.Instance.TransitionScreen.TransitionScreenFadeIn());}
        AudioManagerController.Instance.PlaySFX("TransitionScreenIn");

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
        PauseGame();
    }

    public void OnPlayerDeath() {
        gameOverUIView.SetActive(true);
        Debug.LogWarning("Game Over!");
    }

    private void PauseGame(){
        Time.timeScale = 0;
    }
}

}