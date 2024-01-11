using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    //spawning
    private bool spawnedOutsideBarricade;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //attack barricade
    public void AttackBarricade()
    {

    }

    //getters & setters
    public bool DidZombieSpawnOutsideBarricade()
    { return spawnedOutsideBarricade; }
}
