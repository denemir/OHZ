using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ZombieSpawnRegion : MonoBehaviour
{
    public Collider spawnRegion;
    public float spawnDepth;

    private void Start()
    {
        if(spawnRegion == null)
        {
            Debug.Log("Spawn region collider not attached. Please attach a collider to this object.");
        }
    }

    //getters & setters
    public Vector3 GetRandomSpawnPositionWithinBounds()
    {
        Bounds bounds = spawnRegion.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(randomX, bounds.min.y - spawnDepth, randomZ);
    }
}
