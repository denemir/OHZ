using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public class Interaction
    {
        public string prompt;
        public KeyCode key;
        public UnityEvent action;
        public bool holdKeyDown; //if true, key must be held rather than just pressed.

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

    public void Start()
    {
        interactions = new List<Interaction>();
        playersInRange = new List<Player>();
        activeInteraction = null;
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
