using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // assign in Inspector
    public Transform[] spawnPoints; // assign your spawn points in Inspector

    public float spawnInterval = 3f; // spawn every 3 seconds
    private float timer;
    private int spawnIndex = 0;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[spawnIndex];

        // Instantiate enemy at that location
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        spawnIndex = (spawnIndex + 1) % spawnPoints.Length;
    }
}
