using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePool : MonoBehaviour
{
    //vars
    public GameObject zombiePrefab;
    //public int poolSize; 
    public int poolLimit; //24

    private List<Zombie> zombiePool;
    public List<ZombieSpawnRegion> spawnRegion; //MAKE SURE WHEN A NEW REGION IS CREATED THAT IT IS ADDED TO THIS LIST, OTHERWISE IT WONT SPAWN POOP

    // Start is called before the first frame update
    void Start()
    {
        InitializeZombiePool();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //spawning
    public Vector3 DetermineSpawnPoint(ZombieSpawnRegion spawnRegion)
    {
        return spawnRegion.GetRandomSpawnPositionWithinBounds();
    }
    private ZombieSpawnRegion FindNearestSpawnRegion()
    {
        return null;
    }
    private ZombieSpawnRegion FindRandomSpawnRegion()
    {
        int randomNum = Random.Range(0, spawnRegion.Count);
        return spawnRegion[randomNum];
    }
    public bool SpawnZombie()
    {
        Vector3 spawnPosition;
        ZombieSpawnRegion spawnRegion = FindRandomSpawnRegion();

        spawnPosition = DetermineSpawnPoint(spawnRegion);

        Zombie chosenSubject = GetZombie();
        if(chosenSubject != null)
        {           
            chosenSubject.SpawnFromPool(spawnPosition);
            return true;
        }
        return false; //zombies are busy.
    }

    //pooling
    //initialization
    public void InitializeZombiePool()
    {
        zombiePool = new List<Zombie>();

        for (int i = 0; i < poolLimit; i++)
        {
            AddZombieToPool();
            //Debug.Log("adding");
        }
    }

    //pool functions
    //pull unused Zombie
    public Zombie GetZombie()
    {

        foreach (Zombie zombie in zombiePool)
        {
            if (!zombie.gameObject.activeInHierarchy)
            {
                //zombie.gameObject.SetActive(true);
                return zombie;
            }
        }

        //all zombies are currently in use
        return null;
    }

    //return zombie that is no longer in use
    public void ReturnZombie(Zombie zombie)
    {
        zombie.transform.position = transform.position;
        zombie.gameObject.SetActive(false);
    }
    //add individual bullet to pool
    private void AddZombieToPool()
    {
        //adding bullets to pool
        GameObject zombieInstance = Instantiate(zombiePrefab);
        Zombie zombie = zombieInstance.GetComponent<Zombie>();
        zombiePool.Add(zombie);
        zombie.gameObject.SetActive(false);
    }
    //returns if the pool is currently full (implying either all the zombies are eliminated or haven't spawned)
    public bool IsPoolEmpty()
    {
        if (GetZombie() == null)
        {
            return true;
        }
        return false;
    }
    public bool IsPoolFull()
    {
        if (GetNumberOfAvailableZombies() == poolLimit)
            return true;
        return false;
    }
    public int GetNumberOfAvailableZombies()
    {
        int count = 0;
        foreach (Zombie zombie in zombiePool)
        {
            if (!zombie.gameObject.activeInHierarchy)
            {
                count++;
            }
        }
        return count;
    }

}
