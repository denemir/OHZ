using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    //door stats
    public int cost; //cost to open door

    //interaction vars
    private Interactable interactable;
    private bool isInitialized = false;
    private string interactButton;
    private Player interactingPlayer;
    public UnityEvent openDoor;

    //player states
    private Dictionary<Player, bool> playerStates = new Dictionary<Player, bool>();

    //keybinds
    public KeyCode interactKey;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Interactable>() != null)
        {
            interactable = GetComponent<Interactable>();
        }
        else Debug.Log("Interactable component not found on Door. Please attach Interactable script to Door Prefab.");

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
            if(interactable.getPlayersInRange().Count > 0) //interactable has players within range
            {
                Player temp;
                temp = arePlayersInteracting();

                //check if player has enough points (bitchass might be broke)
                if (temp != null && DoesInteractingPlayerHaveEnough(temp))
                    interactingPlayer = temp;
            }
        }
    }

    //object initialization
    private void InitializeInteractions()
    {
        interactButton = "Interact";

        interactable.interactions.Add(new Interactable.Interaction
        {
            prompt = "Hold " + interactKey + " to open the door for " + cost + " points",
            key = interactKey,
            button = interactButton,
            action = openDoor,
            holdKeyDown = true,
            holdTime = 1.0f
        }); //only interaction, prompt player to open the door

        //set interaction to be active
        interactable.activeInteraction = interactable.interactions[0];
    }

    //interactions
    public void OpenDoor()
    {
        OpenDoorAnimation();
        DeactivateDoor();
    }
    private void DeactivateDoor()
    {
        gameObject.SetActive(false);
    }
    //player states
    private int DeterminePlayerStates()
    {
        //reset values
        playerStates.Clear();

        //bool hasPlayerPurchasedWeapon = false;
        foreach (Player player in interactable.getPlayersInRange())
        {
            //Determine if player has weapon in their inventory already
            bool hasEnough = DoesInteractingPlayerHaveEnough(player);
            playerStates[player] = hasEnough;
        }
        return 0;
    } //determine if player is within range and has specified weapon
    private Player arePlayersInteracting()
    {
        foreach (Player player in interactable.getPlayersInRange())
        {
            if (Input.GetKey(interactKey) || Input.GetButton(interactButton))
            {
                return player;
            }
        }
        return null;
    } //determine if any players are interacting with the interactable

    //animation
    private void OpenDoorAnimation()
    {

    } //triggers animation

    //checks
    private bool DoesInteractingPlayerHaveEnough(Player player)
    {
        if (player.points >= cost)
            return true;
        return false;
    } //does player have enough to open the door
}
