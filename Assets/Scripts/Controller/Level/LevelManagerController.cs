using Model;
using Model.Level;
using System.Collections.Generic;
using UnityEngine;
using GameController;
using EventListener;
using System;
using Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using View.Exploration;
using Utils.Persistence;
using Model.Stats;

namespace Controller.Level {
public class LevelManagerController : MonoBehaviour, IBind<LevelData>
{
    // Start is called before the first frame update
    [SerializeField] private PersistentId _persistentId;

    [Header("==== Level Data ====")]
    [SerializeField] LevelData data;
    public string Id => _persistentId.Id;



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
    [SerializeField] FinishExplorationUIView finishExplorationScreen;
    [SerializeField] GameOverUIView gameOverScreen;

    [Header("==== Button Link to Other Screen ====")]
    public Button FinishExplorationButton;
    public Button GameOverButton;

    [Header("==== Enemies List ====")]

    [SerializeField] List<GameObject> enemiesList = new List<GameObject>();

    [Header("==== Event Listener ====")]
    public GameEvent OnStartLevel;
    
    [Header("==== Animation Receivers ====")]
    public AnimationEventReceiver FEreceiver;
    public AnimationEventReceiver GOreceiver;
    

    [Header("==== Event System ====")]
    public EventSystem eventSystem;

    [Header("==== Debug ====")]

    [SerializeField] private bool forceDebug;
    [SerializeField] private int baseMapSize = 5;
    [SerializeField] int baseEnemy = 15;

    [SerializeField] int baseIncreaseMapSize = 5;

        //[NonSerialized] public static LevelManagerController Instance = null;
    void Awake()
    {
        if(_persistentId == null)
            _persistentId = gameObject.AddComponent<PersistentId>();
    }

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
            //Finish Exploration Animation Receiver
            AnimationEvent FEanimationEvent = new AnimationEvent();
            FEanimationEvent.eventName = "FinishExploration";
            FEanimationEvent.OnAnimationEvent += LevelCompleteAnimationFinished;
            FEreceiver.AddAnimationEvent(FEanimationEvent);
            
            
            //Game Over Animation Receiver
            AnimationEvent GOanimationEvent = new AnimationEvent();
            GOanimationEvent.eventName = "GameOver";
            GOanimationEvent.OnAnimationEvent += GameOverAnimationFinished;
            GOreceiver.AddAnimationEvent(GOanimationEvent);
            
            NewStage();
        }
    }

    void SetLevelDetailBasedOnCurrentLevel()
    {
        // Calculate additional size based on the current level
        int additionalSize = CalculateAdditionalSize(getCurrentLevel());
        mapGenerator?.setMapSize(additionalSize + baseMapSize);

        // Dynamically Increase Enemy Amount over time
        int enemyCount = Mathf.Min(
            getCurrentLevel() + baseEnemy,
            (int)(mapGenerator.getMapSize() * 0.75)
        );
        spawnerManager?.SetEnemies(enemiesList, enemyCount);
    }

    int CalculateAdditionalSize(int currentLevel)
    {
        int additionalSize = 0;
        int stagesToClear = baseIncreaseMapSize; // Initial stages to clear to increase map size

        while (currentLevel >= stagesToClear)
        {
            additionalSize++;
            currentLevel -= stagesToClear;
            stagesToClear++; // Increase the number of stages required for the next increment
        }

        return additionalSize;
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

    //LEVEL COMPLETE
    public void LevelCompleted()
    {
        AudioManagerController.Instance.PlaySFX("LevelComplete");
        finishExplorationScreen.animationController.SetTrigger("FinishExplorationOpen");
    }

    public void LevelCompleteAnimationFinished()
    {
        Debug.Log("******* CALL! LevelCompleteAnimationFinished()");
        eventSystem.SetSelectedGameObject(FinishExplorationButton.gameObject);
        PauseGame();
    }

    //GAME OVER
    public void OnPlayerDeath() {
        AudioManagerController.Instance.PlaySFX("GameOver");
        gameOverScreen.animationController.SetTrigger("GameOverOpen");
        Debug.LogWarning("Game Over!");
    }

    public void GameOverAnimationFinished()
    {
        eventSystem.SetSelectedGameObject(GameOverButton.gameObject);
        PauseGame();
    }
    
    

    private void PauseGame(){
        Time.timeScale = 0;
    }

    //Getter Setter
    public int getHighestLevel() {
        return data.highestLevel;
    }

    public int getTotalEnemies() {
        return spawnerManager.getTotalEnemies();
    }

    public int getCurrentLevel()
    {
        return data.currentLevel;
    }

    public int getAccumulatedResearchPoint()
    {
        RobotInGameStats robotInGameStats = player.GetComponent<RobotInGameStats>();
        return robotInGameStats.data.accumulatedResearchPoint;
    }

    public void setCurrentLevel(int level)
    {
        data.currentLevel = level;
    }

}
}