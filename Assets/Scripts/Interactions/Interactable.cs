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

        //keycode
        public KeyCode getKeyCode() { return key; }
        public void setKeyCode(KeyCode keyCode) { key = keyCode; }

    }

    public List<Interaction> interactions;
    public Interaction activeInteraction;

    public void Start()
    {
        interactions = new List<Interaction>();
        activeInteraction = null;
    }

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

}
