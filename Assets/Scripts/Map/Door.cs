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
    private Player interactingPlayer;
    private UnityEvent openDoor;

    //player states
    private Dictionary<Player, bool> playerStates = new Dictionary<Player, bool>();

    //keybinds
    private KeyCode interactKey;

    // Start is called before the first frame update
    void Start()
    {
        InitializeInteractions();
    }

    // Update is called once per frame
    void Update()
    {
        if(isInitialized)
        {
            if(interactable.getPlayersInRange().Count > 0) //interactable has players within range
            {

            }
        }
    }

    //object initialization
    private void InitializeInteractions()
    {
        interactable.interactions.Add(new Interactable.Interaction
        {
            prompt = "Hold " + interactKey + " to open the door for " + cost + " points",
            key = interactKey,
            action = openDoor
        }); //only interaction, prompt player to open the door

        //set interaction to be active
        interactable.activeInteraction = interactable.interactions[0];
    }

    //interactions
    public void OpenDoor()
    {
        
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
