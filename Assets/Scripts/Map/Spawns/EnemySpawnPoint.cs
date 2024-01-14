using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : SpawnPoints
{
    public List<GameObject> enemyPrefabs; //which enemies can spawn
    public GameObject enemyPrefab; //for temporary purposes

    public new void Spawn()
    {
        if (enemyPrefab != null)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("Enemy prefab not found. Please assign enemy prefab to spawn point.");
        }
    }
}
