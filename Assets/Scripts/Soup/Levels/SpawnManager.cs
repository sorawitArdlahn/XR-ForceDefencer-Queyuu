using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mapgenerate;

namespace Spawn {}
[System.Serializable]
public class SpawnerManager : MonoBehaviour
{
    private LevelManager levelManager;

    private WaveFunctionCollapseV2 waveFunctionCollapse;
    private bool MapGeneratedFinish = false;

    // Start is called before the first frame update

    public enum SpawnState { SPAWNING, WAITING, COUNTING, COMPLETED };
    public GameObject enemies;

    List<GameObject> enemyList = new List<GameObject>();

    //[SerializeField] public float minDistanceFromPlayerToSpawn, maxDistanceFromPlayerToSpawn;

    Wave[] waves;

    public Button inactiveButton;

    //Wave currentWave;

    public int totalEnemies;

    int nextWave = 0;
    public float timeBetweenWaves = 5f;
    float waveCountDown;

    private SpawnState state = SpawnState.COUNTING;

    //private float searchCountdown = 2f;

    //Difficulty increase based on number of levels
    private int currentLevel;

    //public Player player;


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
        state = SpawnState.COUNTING;
        inactiveButton.gameObject.SetActive(false);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        
        setStageDetailBasedOnLevel();
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
        SpawnTrigger();
        
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
    bool EnemyIsAlive() {

        if (GameObject.FindGameObjectWithTag("Enemy") == null) {
                return false;
        }
        return true;
    }

    IEnumerator SpawnWave(Wave wave) {
        state = SpawnState.SPAWNING;

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


    //TODO : IMPLEMENT TO SPAWN NOT TOO CLOSE TO THE PLAYER AND HITBOX NOT OVERLAP WITH OTHER ENEMIES

        //if (Vector3.Distance(spawnPosition, player.getLocation()) > 5f) // Ensure it's not too close to the player
        
            /*
            Collider[] colliders = Physics.OverlapSphere(spawnPosition, _enemy.hitboxRadius);
            validPosition = true;

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    validPosition = false;
                    break;
                }
            }
            */
        
    //Instantiated Object need to be Monobehavior class, or inherited from Monobehavior
    Instantiate(_enemy, spawnPosition, Quaternion.identity);

    Debug.Log("Spawning Enemy: " + _enemy.name);
    }

    void WaveCompleted() {
        Debug.Log("Wave Completed");
        state = SpawnState.COMPLETED;
        inactiveButton.gameObject.SetActive(true);
        
    }
     

    public void setCurrentLevel(int level) {
        currentLevel = level;
    }

    public int getCurrentLevel() {
        return currentLevel;
    }


    void setStageDetailBasedOnLevel() {

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

    int EnemyCount()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    public void SpawnTrigger() {
        //Spawn Enemy
        if (EnemyIsAlive() || !MapGeneratedFinish) {
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
