using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerCollectableHandler : MonoBehaviour
{
    public float collisionRange;
    private Transform collisionZone;
    public LayerMask collisionLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    // Collision
    private void DetectCollision()
    {
        Collider[] hitColliders = Physics.OverlapSphere(collisionZone.position, collisionRange, collisionLayer); // detect objects colliding
        List<Interactable> interactablesInRange = new List<Interactable>();

        foreach (Collider collider in hitColliders)
        {
            Collectable collectable = collider.GetComponent<Collectable>();

            if (collectable != null)
            {
                //collectable.
            }
        }
    }
}
