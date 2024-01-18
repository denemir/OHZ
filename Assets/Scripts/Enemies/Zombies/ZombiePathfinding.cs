using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombiePathfinding : MonoBehaviour, IPathfinding
{
    //zombies grrrrrrrrrr
    public Zombie zombieComponent;

    //targeting
    private Transform currentTarget;
    public float targetingRange;
    private Transform targetingZone; //zone for which barricade is searched for, if no barricade is found within bounds then target player

    //pathfinding
    public void FindInitialPath(Transform target)
    {
        target = TargetNearestBarricade();

        if(target == null || (target.GetComponent<Barricades>() != null && target.GetComponent<Barricades>().isBarricadeDestroyed()))
        {
            target = zombieComponent.TargetNearestPlayer();
        }
    }
    public void FindPath(Transform target)
    {
        target = zombieComponent.TargetNearestPlayer();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget == null)
            FindPath(zombieComponent.TargetNearestPlayer());
    }

    //actions
    private void VaultBarricade()
    {

    }
    private void BreakBarricade()
    {
        
    }

    //determining target
    private Transform TargetNearestBarricade()
    {
        return null;
    }
    private Transform TargetBarricade()
    {
        return null;
    }
    private bool IsWithinRangeOfBarricade()
    {
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
