using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePathfinding : MonoBehaviour, IPathfinding
{
    private Zombie zombieComponent;

    //targeting
    private Transform currentTarget;

    public void FindPath(Transform target)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //determining target
    private bool DidZombieSpawnOutsideBarricade()
    {
        if(zombieComponent != null)
        return zombieComponent.DidZombieSpawnOutsideBarricade();
        return false;
    }
    private bool IsBarricadeDestroyed()
    {
        return false;
    }
    private bool IsPlayerDead()
    {
        return false;
    }
}
