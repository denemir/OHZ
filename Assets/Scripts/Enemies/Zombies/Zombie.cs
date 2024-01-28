using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Zombie : Enemy
{
    //targeting
    public float targetRange;
    private Transform targetZone; //boundary in front of the player in which determines if a player can interact with an interactable
    public LayerMask targetLayer;
    public LayerMask barricadeLayer;

    //attacking
    public float attackRange;
    private Transform attackZone;

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
        targetZone = new GameObject("TargetZone").transform;
        targetZone.SetParent(transform); //attach to zombie
        targetZone.localPosition = new Vector3(0, 0f, 0f);

        attackZone = new GameObject("AttackZone").transform;
        attackZone.SetParent(transform); //attach to zombie
        attackZone.localPosition = new Vector3(0, 0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            switch (movementState)
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
        else SearchForTarget();
    }

    private void FixedUpdate()
    {
        if (currentHealth <= 0)
            Die();

        if(target == null)
            SearchForTarget();
    }

    //getters & setters

    //targeting
    private void SearchForTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(targetZone.position, targetRange, targetLayer); //detect players or barricades within zone
        List<Player> playersInRange = new List<Player>();
        List<Barricades> barricadesInRange = new List<Barricades>();

        //check colliders
        foreach (Collider collider in hitColliders)
        {
            if (collider.GetComponent<Player>() != null)
                playersInRange.Add(collider.GetComponent<Player>());
            else if (collider.GetComponent<Barricades>() != null)
                barricadesInRange.Add(collider.GetComponent<Barricades>());
        }

        //check potential targets
        if (playersInRange.Count > 0)
            target = playersInRange[0].transform;
        else if(barricadesInRange.Count > 0)
            target = barricadesInRange[0].transform;
    }

    //attacks
    public void IsTargetWithinAttackingRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(attackZone.position, targetRange, targetLayer); //detect players or barricades within attack range
        foreach(Collider collider in hitColliders)
        {
            if(collider.GetComponent<GameObject>() == target.gameObject)
            {
                
            }
        }
    }
    private void AttackTarget()
    {
        
    }
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

    public void OnDrawGizmosSelected()
    {
        if (targetZone != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(targetZone.position, targetRange);
        }

    }
}
