using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    //movement
    public enum MovementState
    {
        Walking,
        Running,
        Crawling,
        Idle
    }
    private MovementState movementState;
    public float walkSpeed;
    public float runSpeed;
    public float crawlSpeed;

    //spawning
    private bool isTargetBarricadeBroken;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(movementState)
        {
            case MovementState.Walking:
                MoveTowardsTarget(walkSpeed);
                break;
                case MovementState.Running:
                MoveTowardsTarget(runSpeed);
                break;
                case MovementState.Idle:

                break;
            case MovementState.Crawling:
                MoveTowardsTarget(crawlSpeed);
                break;
        }
    }

    //getters & setters

    //attacks
    public void AttackBarricade()
    {

    }

    //animations
    private void SpawnAnimation()
    {

    }
    private void FallAnimation()
    {

    }
    private void BreakBarricadeAnimation()
    {

    }
    private void MantleBarricadeAnimation()
    {

    }
    private void AttackAnimation()
    {

    }

    //post death
    private void ResetStats()
    {
        
    }
}
