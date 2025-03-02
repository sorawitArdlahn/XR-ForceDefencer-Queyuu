using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapgenerate;
using EventListener;
using System;


namespace Spawn {
public enum SpawnState { SPAWNING, WAITING, COUNTING, COMPLETED }
public class SpawnerManager : MonoBehaviour
{
    
    //MapGenerator.
    private WaveFunctionCollapseV2 waveFunctionCollapse;
    private bool MapGeneratedFinish = false;
    // Start is called before the first frame update 
    [NonSerialized] List<GameObject> enemyList = new List<GameObject>();
    //[SerializeField] public float minDistanceFromPlayerToSpawn, maxDistanceFromPlayerToSpawn;
    Wave[] waves;
    List<GameObject> enemiesInWave = new List<GameObject>();

    int numEnemiesInWave = 0;
    int numEnemiesInWaveRemaining = 0;

    [NonSerialized] public int totalEnemies;

    int nextWave = 0;
    public float timeBetweenWaves = 5f;
    float waveCountDown;

    [SerializeField] GameEvent allEnemiesDead;

    private SpawnState state = SpawnState.COUNTING;

    //private float searchCountdown = 2f;

    //Difficulty increase based on number of levels


    [System.Serializable]
    public class Wave {

    public int enemyCount;
    //public List<GameObject> enemyInWaves;
    public float timeBetweenSpawn;
    public float timeBetweenWaves;


    public List<SubWave> subWaves;
    
    }

    [System.Serializable] public class SubWave{
        public int enemyCount;
        public GameObject enemy;
    }

    //Initialization
    void Awake() {
        waveFunctionCollapse = FindObjectOfType<WaveFunctionCollapseV2>();
    }

    //Set initial Countdown Time

    public void ResetSpawn() {
        nextWave = 0;
        waveCountDown = timeBetweenWaves;
        numEnemiesInWave = 0;
        numEnemiesInWaveRemaining = 0;
        state = SpawnState.COUNTING;
        //inactiveButton.gameObject.SetActive(false);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        
        setWaveBasedOnLevelData();
        StartCoroutine(WaitForMapInitialization());
    }

    IEnumerator WaitForMapInitialization()
    {
        // Wait until WaveFunctionCollapseV2 has finished initializing
        while (!waveFunctionCollapse.IsInitialized)
        {
            yield return null;
        }

        MapGeneratedFinish = true;
        OnEnemyDeath();
        
    }

    //Countdown, if countdown reach 0, spawn wave. else continue countdown.
    /*
    void Update() {
        //WAIT FOR MAP TO BE GENERATED
        if (!MapGeneratedFinish)
        {
            return; // Skip the update logic until initialization is complete
        }



        if (state == SpawnState.WAITING) {
            if (!EnemyIsAlive() && nextWave == waves.Length) {
                WaveCompleted();
            }
            else if (!EnemyIsAlive()) {

            } else {
                return;
            }
        }

        if (waveCountDown <= 0 && state != SpawnState.SPAWNING && nextWave < waves.Length) {
            StartCoroutine(SpawnWave(waves[nextWave]));
            Debug.Log("Initiating new wave...");
        } else {
            waveCountDown -= Time.deltaTime;
        }
    }
    */

    //if all enemy is dead, spawn next wave


    IEnumerator SpawnWave(Wave wave) {
        state = SpawnState.SPAWNING;

        numEnemiesInWave = numEnemiesInWaveRemaining = wave.enemyCount;

        for (int i = 0; i < wave.subWaves.Count; i++) {

            yield return new WaitForSeconds(wave.timeBetweenWaves);

            for (int j = 0; j < wave.subWaves[i].enemyCount; j++) {
                SpawnEnemy(wave.subWaves[i].enemy);
                yield return new WaitForSeconds(wave.timeBetweenSpawn);
            }
        }

        state = SpawnState.WAITING;

        nextWave++;

        yield break;
    }

    
    //Spawn Enemy Logic
    void SpawnEnemy(GameObject _enemy) {
    GameObject[] lowGrounds = GameObject.FindGameObjectsWithTag("LowGround");
    Vector3 spawnPosition = Vector3.zero;
    //bool validPosition = false;

    //while (!validPosition)
    GameObject randomGround = lowGrounds[UnityEngine.Random.Range(0, lowGrounds.Length)];
    spawnPosition = randomGround.transform.position;

    GameObject spawnedEnemy = Instantiate(_enemy, spawnPosition, Quaternion.identity);

    if (spawnedEnemy.TryGetComponent(out IDamageable damageable))
    {
        damageable.OnDeath += OnEnemyDeath;
    }

    enemiesInWave.Add(spawnedEnemy);
    Debug.Log("Spawning Enemy: " + _enemy.name);
    }

    void StageCleared() {
        allEnemiesDead?.Raise(this);
        state = SpawnState.COMPLETED;
        //inactiveButton.gameObject.SetActive(true);
        
    }


    void setWaveBasedOnLevelData() {

        //Set Enemy Power based on Level
        /*
        for (int i = 0; i < enemies.Count; i++) {
            enemies[i].setPowerBasedOnLevel(currentLevel);
        }
        */

        //Set Wave Detail based on Level;
        //Distribution of enemies in waves

        // number of waves in this round
        int subWavePerWave;
        int wavesPerRound = UnityEngine.Random.Range(3, 5);


        waves = new Wave[wavesPerRound];

        for (int i = 0; i < wavesPerRound; i++) {
            Wave wave = new Wave();

            wave.timeBetweenWaves = UnityEngine.Random.Range(4f, 9f);
            wave.timeBetweenSpawn = UnityEngine.Random.Range(0.025f, 0.04f);


            if (i == wavesPerRound - 1 && totalEnemies % wavesPerRound != 0) {
                wave.enemyCount = totalEnemies / wavesPerRound + totalEnemies % wavesPerRound;
            } else {        
            wave.enemyCount = totalEnemies / wavesPerRound;
            }

            subWavePerWave = UnityEngine.Random.Range(2,3);

            wave.subWaves = new List<SubWave>(subWavePerWave);

            for (int j = 0; j < subWavePerWave; j++) {
                SubWave subWaves = new SubWave();
                subWaves.enemy = enemyList[UnityEngine.Random.Range(0, enemyList.Count)];

                if (j == subWavePerWave - 1 && wave.enemyCount % subWavePerWave != 0) {
                    subWaves.enemyCount = wave.enemyCount / subWavePerWave + wave.enemyCount % subWavePerWave;
                } else {
                    subWaves.enemyCount = wave.enemyCount / subWavePerWave;
                }

                wave.subWaves.Add(subWaves);
            }

            waves[i] = wave;

        }


    }

    void SpawnBoss() {
        //Spawn Boss
        Debug.Log("Nice Job");
    }

    void OnEnemyDeath(Vector3 Position = new Vector3()) {
        numEnemiesInWaveRemaining--;
        if (numEnemiesInWaveRemaining <= 0 && nextWave >= waves.Length) {
            StageCleared();
        }
        else if (numEnemiesInWaveRemaining <= 0) NewWave();

    }

    public void NewWave() {
        //Spawn Enemy
        if (!MapGeneratedFinish) {
            return;
        }

        if (state != SpawnState.SPAWNING && nextWave < waves.Length) {
            StartCoroutine(SpawnWave(waves[nextWave]));
            Debug.Log("Initiating new wave...");
        }
    }


    public void SetEnemies(List<GameObject> levelEnemyList, int levelTotalEnemy) {
        enemyList = new List<GameObject>();
        enemyList = levelEnemyList;
        totalEnemies = levelTotalEnemy;
    }

    public void getTotalEnemies() {
        Debug.Log("Total Enemies: " + totalEnemies);
    }

}
}