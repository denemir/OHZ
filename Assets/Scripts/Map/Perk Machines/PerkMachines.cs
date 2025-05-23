using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PerkMachines : MonoBehaviour
{
    private class PlayerPerkStates
    {
        public KeyCode keyPressed;
        public bool doesPlayerHavePerk;

        public PlayerPerkStates(KeyCode key, bool doesPlayerHavePerk)
        {
            keyPressed = key;
            this.doesPlayerHavePerk = doesPlayerHavePerk;
        }
    }

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
    private Dictionary<Player, PlayerPerkStates> playerStates = new Dictionary<Player, PlayerPerkStates>();

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
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isInitialized)
        {
            if (interactable.getPlayersInRange().Count > 0) //interactable has players within range
            {
                DeterminePlayerStates();

                Player temp;
                temp = ArePlayersInteracting();

                //check if player has enough playerStats.points 
                if (temp != null && DoesPlayerHaveEnoughPoints(temp))
                    if (!isPerkUpForGrabs && DoesPlayerAlreadyHavePerk(temp))
                        interactingPlayer = temp;
                    else if (isPerkUpForGrabs)
                        interactingPlayer = temp;
            }
        }
        else
        {
            InitializeInteractions();
        }

        //Updating states
        UpdateInteractionStates();
    }

    //object initialization
    private void InitializeInteractions()
    {
        interactButton = "Interact";
        altInteractButton = "AltInteract";

        interactable.interactions.Add(new Interactable.Interaction
        {
            prompt = "Hold " + interactKey + " to purchase a " + perk.perkName + " for " + cost + " points, or hold " + altKey + " to purchase a " + perk.perkName + " for other players",
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

        interactable.interactions.Add(new Interactable.Interaction
        {
            prompt = "Hold " + interactKey + " to purchase a " + perk.perkName + " for other players",
            key = interactKey,
            button = interactButton,
            action = purchasePerkForOthers,
            holdKeyDown = true,
            holdTime = 1.0f
        }); //prompt player to purchase perk for others

        interactable.interactions.Add(new Interactable.Interaction
        {
            prompt = ""
        });

        //set interaction to be active
        interactable.activeInteraction = interactable.interactions[0];
        isInitialized = true;
    }

    //pre-interaction
    private void PerkMachineJingle()
    {

    }
    private int DeterminePlayerStates()
    {
        //reset values
        playerStates.Clear();

        foreach (Player player in interactable.getPlayersInRange())
        {
            // Add the player to the dictionary if it's not already present
            if (!playerStates.ContainsKey(player))
            {
                // Create a new PlayerPerkStates object and add it to the dictionary
                playerStates.Add(player, new PlayerPerkStates(/*player.inputState == Player.InputState.KandM ? interactKey : altKey*/0, player.GetPlayerPerks().DoesPlayerHavePerk(perk)));
            }

        }
        return 0;
    } //determine if player is within range and has specified perk
    private void UpdateInteractionStates()
    {
        //determine player prompts
        foreach (Player player in playerStates.Keys) //change per player
        {
            switch (playerStates[player].doesPlayerHavePerk)
            {
                case true:
                    if (isPerkUpForGrabs)
                    {
                        interactable.activeInteraction = interactable.interactions[3];
                    }
                    else interactable.activeInteraction = interactable.interactions[2]; //set purchase perk for other prompt to be active
                    break;
                case false:
                    if (isPerkUpForGrabs)
                    {
                        interactable.activeInteraction = interactable.interactions[1];
                    }
                    else interactable.activeInteraction = interactable.interactions[0]; //set purchase perk prompt to be active
                    break;
            }
        }
    } //update which prompt shows

    //mid-interaction
    public void PurchaseAndTakePerk()
    {
        DeductPointsFromPlayer(interactingPlayer);
        TakePerk(interactingPlayer);
        ResetMachine();
    } //for the player who is purchasing the perk for themselves
    public void PurchasePerk()
    {
        DeductPointsFromPlayer(interactingPlayer);
        isPerkUpForGrabs = true;
        interactable.activeInteraction = interactable.interactions[1];
    } //for the player who is purchasing the perk
    public void TakePerk(Player player)
    {
        player.GetPlayerPerks().AddPerk(perk);
        ResetMachine();
    } //actually giving the perk to the player and having them drink it
    public void AcceptPerk()
    {
        TakePerk(interactingPlayer);
    } //just taking the perk and drinking it thru interactable
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
                        playerStates[player].keyPressed = altKey;
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
                        playerStates[player].keyPressed = altKey;
                        return player;
                    }
                    break;
            }

        }
        return null;
    }
}
