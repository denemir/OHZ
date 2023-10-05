using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteractHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public float interactionRange = 3f;
    private Transform interactionZone; //boundary in front of the player in which determines if a player can interact with an interactable

    void Start()
    {
        interactionZone = new GameObject("InteractionZone").transform;
        interactionZone.SetParent(transform); //attach to player
        interactionZone.localPosition = Vector3.forward * interactionRange; //push interaction zone in front of player
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(interactionZone.position, interactionRange); //detect objects within zone
        
        foreach(Collider collider in hitColliders)
        {
            Interactable interactable = collider.GetComponent<Interactable>();

            if(interactable != null)
            {
                //prompt
                if(isPlayerGUIHandlerActive())
                {
                    if (doesInteractableHaveActiveInteraction(interactable))
                        GetComponent<PlayerGUIHandler>().DisplayEventPrompt(interactable.activeInteraction);
                }

                //get interactable key
                if (isInteractableKeyDown(interactable))
                {
                    interactable.Interact(interactable.activeInteraction); //if the key is down then interact with the currect active interaction
                }
            }
        }
    }

    private bool doesInteractableHaveActiveInteraction(Interactable interactable)
    {
        return interactable.activeInteraction != null;
    }

    private bool isPlayerGUIHandlerActive()
    {
        return GetComponent<PlayerGUIHandler>() != null;
    }

    private bool isInteractableKeyDown(Interactable interactable) {
        return Input.GetKeyDown(interactable.activeInteraction.key);
    }

}
