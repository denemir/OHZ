using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePool : MonoBehaviour
{
    //vars
    public GameObject zombiePrefab;
    public int poolSize;
    public int poolLimit;

    private List<Zombie> zombiePool;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //spawning
    public Transform DetermineSpawnPoint()
    {
        return null;
    }
    private ZombieSpawnRegion FindNearestSpawnRegion()
    {
        return null;
    }
    private ZombieSpawnRegion FindRandomSpawnRegion()
    {
        return null;
    }
    public void SpawnZombie()
    {

    }

    //pooling
    //initialization
    public void InitializeZombiePool()
    {
        zombiePool = new List<Zombie>();

        for (int i = 0; i < poolSize; i++)
        {
            AddZombieToPool();
        }

        poolLimit = 100; //for testing
    }

    //pull unused Zombie
    public Zombie GetZombie()
    {

        foreach (Zombie zombie in zombiePool)
        {
            //if (zombiePool.Count(Zombie => Zombie.gameObject.activeInHierarchy) >= poolSize)
            //{
            //    ExpandZombiePool();
            //    ZombiePool.Last().gameObject.SetActive(true);
            //    return ZombiePool.Last(); //return most recently created Zombie
            //}
            if (!zombie.gameObject.activeInHierarchy)
            {
                zombie.gameObject.SetActive(true);
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

}
