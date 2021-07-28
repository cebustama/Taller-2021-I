using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] enemyPrefabs;

    public float spawnTime = 1f;
    private float spawnTimer = 0f;

    private Collider2D spawnCollider;

    public int enemiesToSpawn = int.MaxValue;
    public int spawnedCounter = 0;

    [Header("Room")]
    public EnemyRoom parentRoom;

    private void Start()
    {
        spawnCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        // Cada cierto tiempo, instanciar el objeto enemigo SOLO SI no se ha alcanzado el límite
        if (spawnTimer <= 0f && spawnedCounter < enemiesToSpawn)
        {
            SpawnEnemy();
            spawnTimer = spawnTime;
        }
        spawnTimer -= Time.deltaTime;
    }

    private void SpawnEnemy()
    {
        Vector2 randomPosition = new Vector2(
            Random.Range(spawnCollider.bounds.min.x, spawnCollider.bounds.max.x),
            Random.Range(spawnCollider.bounds.min.y, spawnCollider.bounds.max.y));

        GameObject newEnemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], 
            randomPosition, Quaternion.identity, transform);

        if (parentRoom != null) parentRoom.enemies.Add(newEnemy);

        spawnedCounter++;
    }
}
