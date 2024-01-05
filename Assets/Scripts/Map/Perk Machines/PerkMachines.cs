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

    //perk up for grabs
    public bool isPerkUpForGrabs;

    //player states
    private Dictionary<Player, KeyCode> playerStates = new Dictionary<Player, KeyCode>();

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
        if(isInitialized)
        {
            if (interactable.getPlayersInRange().Count > 0) //interactable has players within range
            {
                Player temp;
                temp = ArePlayersInteracting();

                //check if player has enough playerStats.points 
                if (temp != null && DoesPlayerHaveEnoughPoints(temp))
                    if(!isPerkUpForGrabs && DoesPlayerAlreadyHavePerk(temp))
                        interactingPlayer = temp;
                else if(isPerkUpForGrabs)
                        interactingPlayer = temp;
            }
        }
    }

    //object initialization
    private void InitializeInteractions()
    {
        interactButton = "Interact";
        altInteractButton = "AltInteract";

        interactable.interactions.Add(new Interactable.Interaction
        {
            prompt = "Hold " + interactKey + " to purchase a " + perk.perkName + " for " + cost + " playerStats.points, or hold " + altKey + " to purchase a " + perk.perkName + " for others",
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

    //pre-interaction
    private void PerkMachineJingle()
    {

    }

    //mid-interaction
    public void PurchaseAndTakePerk()
    {
        DeductPointsFromPlayer(interactingPlayer);
    } //for the player who is purchasing the perk for themselves
    public void PurchasePerk()
    {

    } //for the player who is purchasing the perk
    public void TakePerk()
    {

    } //actually giving the perk to the player and having them drink it
    private void PerkMachineBuyJingle()
    {

    }
    private void DeductPointsFromPlayer(Player player)
    {
        player.playerStats.points -= cost;
    }

    //post-interaction
    private void ResetMachine()
    {
        interactingPlayer = null;
        isPerkUpForGrabs = false;
        interactable.activeInteraction = interactable.interactions[0];
    }

    //checks
    private bool DoesPlayerAlreadyHavePerk(Player player)
    {
        return false;
    }
    private bool DoesPlayerHaveEnoughPoints(Player player)
    {
        return player.playerStats.points >= cost;
    }
    private Player ArePlayersInteracting()
    {
        foreach (Player player in interactable.getPlayersInRange())
        {
            switch (player.inputState)
            {
                case Player.InputState.KandM:
                    if (Input.GetKey(interactKey))
                    {
                        interactingPlayer = player;

                        return player;
                    }
                    else if (!isPerkUpForGrabs && Input.GetKey(altKey)) //is the perk up for grabs already? if not, then set it to be up for grabs
                    {
                        interactingPlayer = player;
                        playerStates[player] = altKey;
                        isPerkUpForGrabs = true;
                        return player;
                    }
                    break;

                case Player.InputState.Controller:
                    if (Input.GetButton(interactButton))
                    {
                        interactingPlayer = player;

                        return player;
                    }
                    else if (!isPerkUpForGrabs && Input.GetButton(altInteractButton))
                    {
                        interactingPlayer = player;
                        playerStates[player] = altKey;
                        return player;
                    }
                    break;
            }

        }
        return null;
    }
}
