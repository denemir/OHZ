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
    }

    public List<Interaction> interactions;

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

}
