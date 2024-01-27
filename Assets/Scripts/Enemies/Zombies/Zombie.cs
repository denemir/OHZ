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

        //Debug.Log(hitColliders.Length);

        foreach (Collider collider in hitColliders)
        {
            Debug.Log(collider.name);
            if (collider.GetComponent<Player>() != null)
                playersInRange.Add(collider.GetComponent<Player>());
            else if (collider.GetComponent<Barricades>() != null)
                barricadesInRange.Add(collider.GetComponent<Barricades>());
        }

        //Debug.Log(playersInRange.Count);
        //Debug.Log(barricadesInRange.Count);

        if (playersInRange.Count > 0)
            target = playersInRange[0].transform;
        else if(barricadesInRange.Count > 0)
            target = barricadesInRange[0].transform;

        //Player potTarget = IsPlayerWithinTargetingRange(); //potential target
        //if (potTarget == null)
        //{
        //    Barricades bar = IsBarricadeWithinTargetingRange();
        //    if (bar == null)
        //        RemainIdle();
        //    else target = bar.transform;
        //}
        //else { target = potTarget.transform; Debug.Log("set"); }
    }
    private Player IsPlayerWithinTargetingRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(targetZone.position, targetRange, targetLayer); //detect players or barricades within zone
        List<Player> targetsInRange = new List<Player>();

        Debug.Log(hitColliders.Length);
        foreach (Collider collider in hitColliders)
        {
            Debug.Log(collider.GetComponent<Player>());
            targetsInRange.Add(collider.GetComponent<Player>());
        }

        if (targetsInRange.Count > 0)
        {
            return targetsInRange[0];
        }
        return null;
    }
    private Barricades IsBarricadeWithinTargetingRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(targetZone.position, targetRange, barricadeLayer); //detect players or barricades within zone
        List<Barricades> targetsInRange = new List<Barricades>();

        if (targetsInRange.Count > 0)
        {
            return targetsInRange[0];
        }
        return null;
    }

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

    public void OnDrawGizmosSelected()
    {
        if (targetZone != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(targetZone.position, targetRange);
        }

    }
}
