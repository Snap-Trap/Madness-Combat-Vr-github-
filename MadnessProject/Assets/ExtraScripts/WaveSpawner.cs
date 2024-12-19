using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public DefaultAi defaultAi;
    public GameObject enemyPrefab;
    private Transform spawnPoint;

    public float enemyCount, timeBetweenSpawn;

    [SerializeField] private bool isDead;

    private void Start()
    {
        spawnPoint = GameObject.Find("EnemySpawner").transform;
        defaultAi = enemyPrefab.GetComponent<DefaultAi>();
        enemyCount = 5;
        timeBetweenSpawn = 1f;
        isDead = true;


        StartCoroutine(SpawnWave(5f));
        Debug.Log("Started the coroutine for making the wave");
    }

    private void Update()
    {
        Debug.Log("Enemy count is " + enemyCount);
    }

    // L: Does it matter if it's private or not? I don't think it does
    private IEnumerator SpawnWave(float time)
    {
        // L: Okay I am like 99% sure this bool is fucking things up but idk what AAAAAAAAAAAAA
        // L: Keep this fucking variable on false or else shit does not progress
        // L: Alright so I guess it shouldn't be on false, fuck this man
        if (isDead)
        {
            Debug.Log("Enemy is Dead");
            if (enemyCount > 0)
            {
                Debug.Log("Enemy count is greater than 0");
                SpawnEnemy();
                enemyCount -= 1;
            }

            yield return new WaitForSeconds(timeBetweenSpawn);
            Debug.Log("Waiting for 5 seconds");
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("Enemy spawned");

        float enemyHealth = defaultAi.health;
        enemyHealth = 20;
        isDead = false;

        if (enemyHealth <= 0)
        {
            // L: Ok so the enemy script always has worked but because the hp wasn't defined the enemy instantly killed itself  
            Destroy(enemy, 5f);
            isDead = true;
        }
    }
}
