using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    public GameObject target; //what spawns
    public bool hasSpawned = false;

    //debug
    public DebugHandler debugHandler;

    public void Spawn()
    {
        if(target != null)
        {
            Instantiate(target, transform.position, Quaternion.identity);
            hasSpawned = true;
        }
        else
        {
            Debug.LogError("Target not found. Please assign a GameObject Prefab to spawn point.");
        }
    }

    public void Update()
    {
        if (Input.GetButtonDown("Fire3") && !hasSpawned)
        {
            Spawn();
        }
    }
}
