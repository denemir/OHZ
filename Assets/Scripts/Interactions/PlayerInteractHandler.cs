using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteractHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public float interactionRange;
    private Transform interactionZone; //boundary in front of the player in which determines if a player can interact with an interactable

    //key detection
    private bool oldKeyDown;

    private List<Interactable> previousInteractablesInRange = new List<Interactable>();

    void Start()
    {
        interactionZone = new GameObject("InteractionZone").transform;
        interactionZone.SetParent(transform); //attach to player
        interactionZone.localPosition = new Vector3(0, 0, 1) * interactionRange; //push interaction zone in front of player

        oldKeyDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        //DetectCollision();
    }

    private void FixedUpdate()
    {
        DetectCollision();
    }

    //interacting
    private bool doesInteractableHaveActiveInteraction(Interactable interactable)
    {
        return interactable.activeInteraction != null;
    }
    private bool doesInteractableHaveActiveInteractionWithAlt(Interactable interactable)
    {
        return interactable.activeInteraction.altAction != null;
    }
    private bool isInteractableKeyPressed(Interactable interactable)
    {
        return Input.GetKeyDown(interactable.activeInteraction.key);
    }
    private bool isInteractableAltKeyPressed(Interactable interactable)
    {
        return Input.GetKeyDown(interactable.activeInteraction.altKey);
    }
    private bool isInteractableKeyDown(Interactable interactable)
    {
        return Input.GetKey(interactable.activeInteraction.key);
    }
    private bool isInteractableAltKeyDown(Interactable interactable)
    {
        return Input.GetKey(interactable.activeInteraction.altKey);
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

                switch(interactable.activeInteraction.holdKeyDown)
                {
                    case false:
                        if (!CheckKeyPress(interactable))
                            if (!CheckAltKeyPress(interactable))
                                interactable.ResetTimer(interactable.activeInteraction);
                        break;
                    case true:
                        if (!CheckKeyHold(interactable))
                            if (!CheckAltKeyHold(interactable))
                                interactable.ResetTimer(interactable.activeInteraction);
                        break;
                }

                    //adds to list of interactables within the range
                    interactablesInRange.Add(interactable);

                //update old key
                oldKeyDown = isInteractableKeyDown(interactable);
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

    ////debug
    //public void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(interactionZone.position, interactionRange);
    //}

    //checks
    private bool CheckKeyPress(Interactable interactable)
    {
        //get interactable key
        if (isInteractableKeyPressed(interactable)/* && !oldKeyDown*/)
        {
            interactable.Interact(interactable.activeInteraction); //if the key is down then interact with the current active interaction
            return true;
        }
        return false;
    }
    private bool CheckAltKeyPress(Interactable interactable)
    {
        //get alt key
        if (isInteractableAltKeyPressed(interactable) /*&& !oldKeyDown*/ && doesInteractableHaveActiveInteractionWithAlt(interactable))
        {
            interactable.InteractAlternate(interactable.activeInteraction); //if the key is down then interact with the current active alternate interaction
            return true;
        }
        return false;
    }
    private bool CheckKeyHold(Interactable interactable)
    {
        //is the key pressed and the time is less than threshold?
        if (isInteractableKeyDown(interactable) && interactable.activeInteraction.currentHoldTime < interactable.activeInteraction.holdTime)
        {
            interactable.IncrementHoldTimer(interactable.activeInteraction);
            return true;
        }
        //else interactable.ResetTimer(interactable.activeInteraction);

        if (isInteractableKeyDown(interactable) && interactable.activeInteraction.currentHoldTime >= interactable.activeInteraction.holdTime)
        {
            interactable.Interact(interactable.activeInteraction); //if the key is down then interact with the current active interaction
            return true;
        }
        return false;
    }
    private bool CheckAltKeyHold(Interactable interactable)
    {
        if (isInteractableAltKeyDown(interactable) && interactable.activeInteraction.currentHoldTime < interactable.activeInteraction.holdTime)
        {
            interactable.IncrementHoldTimer(interactable.activeInteraction);
            return true;
        }
        //else interactable.ResetTimer(interactable.activeInteraction);

        if (isInteractableAltKeyDown(interactable) && interactable.activeInteraction.currentHoldTime >= interactable.activeInteraction.holdTime)
        {
            interactable.InteractAlternate(interactable.activeInteraction); //if the key is down then interact with the current active interaction
            return true;
        }
        return false;
    }



}
