using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups; //list group of enemy to spawn
        //public List<GameObject> enemyPrefabs; //list of prefab enemies for this wave type
        //public List<string> enemyNames;
        //public List<int> enemyCount; //num enemie to spawn in wave
        public int waveQuota; //total num of spawned enemies
        public float spawnInterval; //time btw each spawn
        public int spawnCount; //num of enemies arleady in wave

    }
    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount;//num enemie to spawn in wave
        public int spawnCount;//num of enemies arleady in wave
        public GameObject enemyPrefab; 
    }

    public List<Wave> waves; //list all the waves in the game
    public int currentWaveCount; //index of wave **starts from 0** 


    [Header("Spawner Attributes")]
    float spawnerTimer; //timer use to spawn next enemy
    public int enemiesAlive;
    public int maxEnemiesAllowed; //max num of enemies on the field 
    public bool maxEnemiesReached = false; //to flag for max capacity so there is no overcrowding
    public float waveInterval;
    bool isWaveActive = false;

    [Header("Spawn Positions")]
    public List<Transform> relativeSpawnPoints; //list of spawn points


    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        CalculateWaveQuota();
        //SpawnEnemies(); for testing
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0 && !isWaveActive) //check if wave has ended before spawning new ones
        {
            StartCoroutine(BeginNextWave());
        }
        spawnerTimer += Time.deltaTime;

        //checks if time to spawn more enemies
        if (spawnerTimer >= waves[currentWaveCount].spawnInterval)
        {
            spawnerTimer = 0f;
            SpawnEnemies();
        }
    }

    IEnumerator BeginNextWave()
    {
        isWaveActive = true
            ;
        //wave for wave interval sec before start of next wave
        yield return new WaitForSeconds(waveInterval);

        //if there are more waves to start after the current wave move to the next wave
        if(currentWaveCount < waves.Count - 1)
        {
            isWaveActive=false;
            currentWaveCount++;
            CalculateWaveQuota();
        }
    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach(var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }

        waves[currentWaveCount].waveQuota = currentWaveQuota;
        Debug.LogWarning(currentWaveQuota);
    }

    void SpawnEnemies()
    {
        //checks for min num of enemies in wave spawned
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemiesReached)
        {
            //spawn each type of enemy till quota reached
            foreach(var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                //check if min num of enemy of type has been spawned
                if(enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    //spawn enemies randomly close to player
                    Instantiate(enemyGroup.enemyPrefab, player.position + relativeSpawnPoints[Random.Range(0, relativeSpawnPoints.Count)].position, Quaternion.identity);

                    //Vector2 spawnPos = new Vector2(player.transform.position.x + Random.Range(-10f, 10f), player.transform.position.y + Random.Range(-10f, 10f));
                    //Instantiate(enemyGroup.enemyPrefab, spawnPos, Quaternion.identity);

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;

                    //limit the num of enemies
                    if (enemiesAlive >= maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return;

                    }
                }
            }
        }
    }

    public void OnEnemyKill()
    {
        enemiesAlive--;

        //reset the the flag if num is less
        if (enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }
}
