using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoint : SpawnPoints
{
    // Start is called before the first frame update
    public GameObject playerPrefab; //which player spawns

    public new void Spawn()
    {
        if (playerPrefab != null)
        {
            Instantiate(playerPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("Player prefab not found. Please assign player prefab to spawn point.");
        }
    }
}
