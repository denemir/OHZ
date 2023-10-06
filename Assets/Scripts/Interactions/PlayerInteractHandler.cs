using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteractHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public float interactionRange;
    private Transform interactionZone; //boundary in front of the player in which determines if a player can interact with an interactable

    private List<Interactable> previousInteractablesInRange = new List<Interactable>();

    void Start()
    {
        interactionZone = new GameObject("InteractionZone").transform;
        interactionZone.SetParent(transform); //attach to player
        interactionZone.localPosition = new Vector3(0, 0, 1) * interactionRange; //push interaction zone in front of player
    }

    // Update is called once per frame
    void Update()
    {
        DetectCollision();
    }

    //interacting
    private bool doesInteractableHaveActiveInteraction(Interactable interactable)
    {
        return interactable.activeInteraction != null;
    }
    private bool isInteractableKeyDown(Interactable interactable) {
        return Input.GetKeyDown(interactable.activeInteraction.key);
    }

    //collision
    private void DetectCollision() //handles collision and removal of prompts
    {
        Collider[] hitColliders = Physics.OverlapSphere(interactionZone.position, interactionRange); //detect objects within zone
        List<Interactable> interactablesInRange = new List<Interactable>();                         

        //items within range of player's interaction zone
        foreach (Collider collider in hitColliders)
        {
            Interactable interactable = collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                interactable.playerEntersRange(GetComponent<Player>());
                //prompt
                if (isPlayerGUIHandlerActive())
                {
                    if (doesInteractableHaveActiveInteraction(interactable))
                        GetComponent<PlayerGUIHandler>().DisplayEventPrompt(interactable.activeInteraction);
                    else Debug.Log("Interactable does not have an active interaction event.");
                }
                else Debug.Log("Player GUI not active.");

                //get interactable key
                if (isInteractableKeyDown(interactable))
                {
                    interactable.Interact(interactable.activeInteraction); //if the key is down then interact with the currect active interaction
                }

                //adds to list of interactables within the range
                interactablesInRange.Add(interactable);
            }
        }

        // Check for interactables that were previously in range but are now out of range
        List<Interactable> interactablesToRemove = new List<Interactable>();

        foreach (Interactable interactable in previousInteractablesInRange)
        {
            if (!interactablesInRange.Contains(interactable))
            {
                interactable.playerLeavesRange(GetComponent<Player>());
                interactablesToRemove.Add(interactable);
            }
        }

        // Handle removal of prompts or other actions for interactables out of range
        if (isPlayerGUIHandlerActive())
        {
            foreach (Interactable interactable in interactablesToRemove)
            {
                GetComponent<PlayerGUIHandler>().RemovePrompt();
                //Debug.Log("Interactable out of range: " + interactable.name);
            }
        }

        // Store the current list of interactables in range for future comparison
        previousInteractablesInRange = interactablesInRange;
    }

    //gui
    private bool isPlayerGUIHandlerActive()
    {
        return GetComponent<PlayerGUIHandler>() != null;
    }

    //debug
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionZone.position, interactionRange);
    }

}
