using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    private bool hasSpawned;
    private Transform player;

    public GameObject[] enemiesToSpawn;

    public int maxSpawnAmount;

    //RANGES
    public float spawnRange;
    public float detectionRange;

    void Start()
    {
        player = PlayerManager.instance.ourPlayer.transform;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if(distance <= detectionRange)
        {
            if (!hasSpawned)
            {
                SpawnEnemies();
            }   
        }
    }

    void SpawnEnemies()
    {
        hasSpawned = true;

        int spawnAmount = Random.Range(1, maxSpawnAmount+1);

        for(int i = 0; i < spawnAmount; i++)
        {
            float xSpawnPos = transform.position.x + Random.Range(-spawnRange, spawnRange);
            float zSpawnPos = transform.position.z + Random.Range(-spawnRange, spawnRange);

            Vector3 spawnPoint = new Vector3(xSpawnPos, 0, zSpawnPos);
            GameObject newObject = (GameObject)Instantiate(enemiesToSpawn[Random.Range(0, enemiesToSpawn.Length)], spawnPoint, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.blue;
        Handles.DrawWireArc(transform.position, transform.up, transform.right, 360, detectionRange);
        Handles.color = Color.red;
        Handles.DrawWireArc(transform.position, transform.up, transform.right, 360, spawnRange);
    }
}
