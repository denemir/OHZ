using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteractHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public float interactionRange;
    private Transform interactionZone; //boundary in front of the player in which determines if a player can interact with an interactable
    public LayerMask interactionLayer;

    //key detection
    private bool oldKeyDown;
    private bool oldButtonDown;

    private List<Interactable> previousInteractablesInRange = new List<Interactable>();

    void Start()
    {
        interactionZone = new GameObject("InteractionZone").transform;
        interactionZone.SetParent(transform); //attach to player
        interactionZone.localPosition = new Vector3(0, 0.5f, 0.5f) * interactionRange; //push interaction zone in front of player

        oldKeyDown = false;
    }

    private void FixedUpdate()
    {
        DetectCollision();
    }

    //interacting
    private bool DoesInteractableHaveActiveInteraction(Interactable interactable)
    {
        return interactable.activeInteraction != null;
    }
    private bool DoesInteractableHaveActiveInteractionWithAlt(Interactable interactable)
    {
        return interactable.activeInteraction.altAction != null;
    }
    private bool IsInteractableKeyPressed(Interactable interactable)
    {
        switch (GetComponent<Player>().inputState)
        {
            case Player.InputState.KandM:
                return Input.GetKeyDown(interactable.activeInteraction.key);
            case Player.InputState.Controller:
                return Input.GetButtonDown("Interact");
        }
        return false;
    }
    private bool IsInteractableAltKeyPressed(Interactable interactable)
    {
        switch (GetComponent<Player>().inputState)
        {
            case Player.InputState.KandM:
                return Input.GetKeyDown(interactable.activeInteraction.altKey);
            case Player.InputState.Controller:
                return Input.GetKeyDown(interactable.activeInteraction.altButton);
        }
        return false;
    }
    private bool IsInteractableKeyDown(Interactable interactable)
    {
        switch (GetComponent<Player>().inputState)
        {
            case Player.InputState.KandM:
                return Input.GetKey(interactable.activeInteraction.key);
            case Player.InputState.Controller:
                return Input.GetButton(interactable.activeInteraction.button);
        }
        return false;
    }
    private bool IsInteractableAltKeyDown(Interactable interactable)
    {
        switch (GetComponent<Player>().inputState)
        {
            case Player.InputState.KandM:
                return Input.GetKey(interactable.activeInteraction.altKey);
            case Player.InputState.Controller:
                //Debug.Log(interactable.activeInteraction.altButton);
                if (Input.GetButton(interactable.activeInteraction.altButton))
                    return Input.GetButton(interactable.activeInteraction.altButton);
                return false;
        }
        return false;
    }

    //collision
    private void DetectCollision() //handles collision and removal of prompts  /////////////////////////////////////////////////////// this could be optimized lil fella
    {
        Collider[] hitColliders = Physics.OverlapSphere(interactionZone.position, interactionRange, interactionLayer); //detect objects within zone
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
                    if (DoesInteractableHaveActiveInteraction(interactable))
                        GetComponent<PlayerGUIHandler>().DisplayEventPrompt(interactable.activeInteraction);
                    else
                    {
                        Debug.Log("Interactable does not have an active interaction event.");
                        break;
                    }
                }
                else Debug.Log("Player GUI not active.");

                switch (interactable.activeInteraction.holdKeyDown)
                {
                    case false:
                        if (!CheckKeyPress(interactable))
                            if (!CheckAltKeyPress(interactable))
                                interactable.ResetTimer(interactable.activeInteraction);
                        break;
                    case true:
                        if (!CheckKeyHold(interactable))
                            if (interactable.activeInteraction.hasAltEvent && !CheckAltKeyHold(interactable))
                                interactable.ResetTimer(interactable.activeInteraction);
                        break;
                }

                //adds to list of interactables within the range
                interactablesInRange.Add(interactable);

                //update old key
                oldKeyDown = IsInteractableKeyDown(interactable);
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
        if (interactionZone != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(interactionZone.position, interactionRange);
        }

    }

    //checks
    private bool CheckKeyPress(Interactable interactable)
    {
        //get interactable key
        if (IsInteractableKeyPressed(interactable)/* && !oldKeyDown*/)
        {
            interactable.Interact(interactable.activeInteraction); //if the key is down then interact with the current active interaction
            return true;
        }
        return false;
    }
    private bool CheckAltKeyPress(Interactable interactable)
    {
        //get alt key
        if (IsInteractableAltKeyPressed(interactable) /*&& !oldKeyDown*/ && DoesInteractableHaveActiveInteractionWithAlt(interactable))
        {
            interactable.InteractAlternate(interactable.activeInteraction); //if the key is down then interact with the current active alternate interaction
            return true;
        }
        return false;
    }
    private bool CheckKeyHold(Interactable interactable)
    {
        //is the key pressed and the time is less than threshold?
        if (IsInteractableKeyDown(interactable) && interactable.activeInteraction.currentHoldTime < interactable.activeInteraction.holdTime)
        {
            interactable.IncrementHoldTimer(interactable.activeInteraction);
            return true;
        }
        //else interactable.ResetTimer(interactable.activeInteraction);

        if (IsInteractableKeyDown(interactable) && interactable.activeInteraction.currentHoldTime >= interactable.activeInteraction.holdTime)
        {
            interactable.Interact(interactable.activeInteraction); //if the key is down then interact with the current active interaction
            return true;
        }
        return false;
    }
    private bool CheckAltKeyHold(Interactable interactable)
    {
        switch (GetComponent<Player>().inputState)
        {
            case Player.InputState.KandM:
                if (IsInteractableAltKeyDown(interactable) && interactable.activeInteraction.currentHoldTime < interactable.activeInteraction.holdTime)
                {
                    interactable.IncrementHoldTimer(interactable.activeInteraction);
                    return true;
                }
                //else interactable.ResetTimer(interactable.activeInteraction);

                if (IsInteractableAltKeyDown(interactable) && interactable.activeInteraction.currentHoldTime >= interactable.activeInteraction.holdTime)
                {
                    interactable.InteractAlternate(interactable.activeInteraction); //if the key is down then interact with the current active interaction
                    return true;
                }
                break;
            case Player.InputState.Controller:
                if (IsInteractableAltKeyDown(interactable) && interactable.activeInteraction.currentHoldTime < interactable.activeInteraction.holdTime)
                {
                    interactable.IncrementHoldTimer(interactable.activeInteraction);
                    return true;
                }
                //else interactable.ResetTimer(interactable.activeInteraction);

                if (IsInteractableAltKeyDown(interactable) && interactable.activeInteraction.currentHoldTime >= interactable.activeInteraction.holdTime)
                {
                    interactable.InteractAlternate(interactable.activeInteraction); //if the key is down then interact with the current active interaction
                    return true;
                }
                break;
        }
        return false;
    }



}
