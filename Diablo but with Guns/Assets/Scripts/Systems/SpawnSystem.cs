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

        for(int i = 0; i < 3/*spawnAmount*/; i++)
        {
            /*SQUARE 
            float xSpawnPos = transform.position.x + Random.Range(-spawnRange, spawnRange);
            float zSpawnPos = transform.position.z + Random.Range(-spawnRange, spawnRange);
            
            Vector3 spawnPoint = new Vector3(xSpawnPos, 0, zSpawnPos);
            */

            //Circle
            float theta = 360f * Random.value;
            float radius = Random.Range(0f, spawnRange);

            Vector3 center = transform.position;
            Vector3 point = new Vector3(radius * Mathf.Sin(theta), 0f, radius * Mathf.Cos(theta));

            Vector3 spawnPoint = center + point;

            //LOOKROTATION
            float rot = 360f * Random.value;
            Quaternion spawnRotation = Quaternion.Euler(0, rot, 0);

            GameObject newObject = (GameObject)Instantiate(enemiesToSpawn[Random.Range(0, enemiesToSpawn.Length)], spawnPoint, spawnRotation);
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
