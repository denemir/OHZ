using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public class Interaction
    {
        //vars
        public string prompt;
        public KeyCode key;
        public KeyCode altKey;
        public UnityEvent action;
        public UnityEvent altAction;
        public Player player;
        public bool holdKeyDown; //if true, key must be held rather than just pressed.
        public float holdTime;

        //updates
        public bool isInteracting = false;
        public float currentHoldTime = 0.0f;

        //keycode
        public KeyCode getKeyCode() { return key; }
        public void setKeyCode(KeyCode keyCode) { key = keyCode; }

    }

    //setting interactions
    public List<Interaction> interactions;
    public Interaction activeInteraction;

    //player context
    private List<Player> playersInRange;
    private List<bool> playersInteracting;
    private Player interactingPlayer;

    public void Start()
    {
        interactions = new List<Interaction>();
        playersInRange = new List<Player>();
        activeInteraction = null;
    }

    public void FixedUpdate()
    {
        
    }

    //interacting
    public void Interact()
    {
        foreach(Interaction interaction in interactions)
        {
            if(Input.GetKeyDown(interaction.key))
            {
                interaction.action.Invoke();
                return;
            }
        }
    }
    public void Interact(Interaction interaction)
    {
        interaction.action.Invoke();
        ResetTimer(interaction);
    }
    public void Interact(Interaction interaction, Player player)
    {
        interaction.action.Invoke();
        interaction.player = player;
        ResetTimer(interaction);
    }
    public void InteractAlternate(Interaction interaction)
    {
        interaction.altAction.Invoke();
    }
    private void IsAnyoneInteracting()
    {

    }
    public void IncrementHoldTimer(Interaction interaction)
    {
        interaction.currentHoldTime += 0.05f;
    }
    public void ResetTimer(Interaction interaction)
    {
        interaction.currentHoldTime = 0;
    }

    //player context
    public void playerEntersRange(Player player)
    {
        if (!playersInRange.Contains(player))
        {
            playersInRange.Add(player);
            //Debug.Log("Player has entered range.");
        }
    }
    public void playerLeavesRange(Player player)
    {
        if (playersInRange.Contains(player))
        {
            playersInRange.Remove(player);
            //Debug.Log("Player has left range.");
        }
        else Debug.Log("Player is not found within range.");
    }
    public List<Player> getPlayersInRange()
    {
        return playersInRange;
    }

}
