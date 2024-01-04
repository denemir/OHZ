using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PerkMachines : MonoBehaviour
{
    //perk machine stats
    public int cost; //cost to purchase perk
    public Perk perk;

    //interaction vars
    private Player interactingPlayer;
    private Interactable interactable;
    private bool isInitialized = false;
    public KeyCode interactKey;
    public KeyCode altKey;
    private string interactButton;
    private string altInteractButton;
    public UnityEvent purchasePerkForSelf;
    public UnityEvent purchasePerkForOthers;
    public UnityEvent acceptPerk;


    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Interactable>() != null)
        {
            interactable = GetComponent<Interactable>();
        }
        else Debug.Log("Interactable component not found on Perk Machine. Please attach Interactable script to PerkMachine Prefab.");

        if (GetComponent<Interactable>().interactions != null)
        {
            InitializeInteractions();
            isInitialized = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //object initialization
    private void InitializeInteractions()
    {
        interactButton = "Interact";
        altInteractButton = "AltInteract";

        interactable.interactions.Add(new Interactable.Interaction
        {
            prompt = "Hold " + interactKey + " to purchase a " + perk.perkName + " for " + cost + " points, or hold " + altKey + " to purchase a " + perk.perkName + " for others",
            key = interactKey,
            altKey = altKey,
            button = interactButton,
            altButton = altInteractButton,
            action = purchasePerkForSelf,
            altAction = purchasePerkForOthers,
            hasAltEvent = true,
            holdKeyDown = true,
            holdTime = 1.0f
        }); //prompt player to purchase perk for themselves or others

        interactable.interactions.Add(new Interactable.Interaction
        {
            prompt = "Hold " + interactKey + " to take " + perk.perkName,
            key = interactKey,
            button = interactButton,
            action = acceptPerk,
            holdKeyDown = true,
            holdTime = 1.0f
        }); //prompt player to accept perk

        //set interaction to be active
        interactable.activeInteraction = interactable.interactions[0];
    }

    //mid-interaction
    public void PurchasePerk()
    {

    }
    private void PerkMachineJingle()
    {

    }

    //post-interaction
    private void ResetMachine()
    {
        interactingPlayer = null;
        interactable.activeInteraction = interactable.interactions[0];
    }

    //checks
    private bool DoesPlayerAlreadyHavePerk()
    {
        return false;
    }
    private bool DoesPlayerHaveEnoughPoints()
    {
        return false;
    }
}
