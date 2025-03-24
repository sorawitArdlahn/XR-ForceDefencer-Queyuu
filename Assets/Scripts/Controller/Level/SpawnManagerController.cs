using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventListener;
using System;
using Model.Level;


namespace Controller.Level {
public enum SpawnState { SPAWNING, WAITING, COUNTING, COMPLETED }
public class SpawnerManagerController : MonoBehaviour
{
    
    //MapGenerator.
    private MapGeneratorController mapGenerator;
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

    [Header("Events")]

    [SerializeField] GameEvent allEnemiesDead;
    [SerializeField] GameEvent researchPointEarned;

    private SpawnState state = SpawnState.COUNTING;
    
    void Awake() {
        mapGenerator = FindObjectOfType<MapGeneratorController>();
    }

    //Set initial Countdown Time

    public void ResetSpawn() {
        nextWave = 0;
        waveCountDown = timeBetweenWaves;
        numEnemiesInWave = 0;
        numEnemiesInWaveRemaining = 0;
        state = SpawnState.COUNTING;
        //inactiveButton.gameObject.SetActive(false);

        
        RemoveAllEnemies();
        setWaveBasedOnLevelData();
        StartCoroutine(WaitForMapInitialization());
    }

    void RemoveAllEnemies() {
        foreach (GameObject enemy in enemiesInWave) {
            Destroy(enemy);
        }
        enemiesInWave?.Clear();
    }

    IEnumerator WaitForMapInitialization()
    {
        // Wait until WaveFunctionCollapseV2 has finished initializing
        while (!mapGenerator.IsInitialized)
        {
            yield return null;
        }

        MapGeneratedFinish = true;
        OnEnemyDeath();
        
    }


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
        GameObject playerPos = GameObject.FindGameObjectWithTag("Player");
        bool validPosition = false;
        float minDistanceFromPlayer = 25f;
        float checkRadius = 1f;
        Vector3 spawnPosition = Vector3.zero;

        GameObject[] lowGrounds = GameObject.FindGameObjectsWithTag("LowGround");

        
        // Check Validity of Spawn Position
        while (!validPosition) {
            //CellV2 spawnTile = mapGenerator.gridComponent[UnityEngine.Random.Range(0, mapGenerator.gridComponent.Count)];
            //float tileHeight = spawnTile.tileOptions[0].originalMapPattern.GetPrefabHeight();
            GameObject randomGround = lowGrounds[UnityEngine.Random.Range(0, lowGrounds.Length)];
            spawnPosition = randomGround.transform.position;
            //spawnPosition.y += tileHeight + 8;
            spawnPosition.y += 5;

            // Check if the spawn position is too close to the player
            if (Vector3.Distance(spawnPosition, playerPos.transform.position) > minDistanceFromPlayer)
            {
                Collider[] colliders = Physics.OverlapSphere(spawnPosition, checkRadius);
                if (colliders.Length == 0)
                {
                    validPosition = true;
                }
            }
        }

        GameObject spawnedEnemy = Instantiate(_enemy, spawnPosition, Quaternion.identity);
        if (spawnedEnemy.TryGetComponent(out IDamageable damageable))
        {
            damageable.OnDeath += OnEnemyDeath;
        }

        enemiesInWave.Add(spawnedEnemy);
        Debug.Log("Spawning Enemy: " + _enemy.name);
    }


    void StageCleared() {
        state = SpawnState.COMPLETED;
        allEnemiesDead?.Raise(this);
        //inactiveButton.gameObject.SetActive(true);   
    }


    void setWaveBasedOnLevelData() {

        //Set Wave Detail based on Level;
        //Distribution of enemies in waves

        // number of waves in this round
        int subWavePerWave;
        int wavesPerRound = UnityEngine.Random.Range(3, 5);


        waves = new Wave[wavesPerRound];

        for (int i = 0; i < wavesPerRound; i++) {
            Wave wave = new Wave();

            wave.timeBetweenWaves = UnityEngine.Random.Range(2f, 4.5f);
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

    //OPTIONAL: Spawn Boss
    void SpawnBoss() {
        //Spawn Boss
        Debug.Log("Nice Job");
    }

    void OnEnemyDeath() {
        numEnemiesInWaveRemaining--;
        researchPointEarned?.Raise(this);
        
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
}