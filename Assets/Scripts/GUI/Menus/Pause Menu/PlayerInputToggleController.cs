using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInputToggleController : MonoBehaviour
{
    public Toggle km;
    public Toggle controller;

    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        km.onValueChanged.AddListener(switchToKM);
        controller.onValueChanged.AddListener(switchToController);
    }

    private void switchToKM(bool isOn)
    {
        if(isOn)
        {
            player.inputState = Player.InputState.KandM;
        }
    }

    private void switchToController(bool isOn)
    {
        if (isOn)
        {
            player.inputState = Player.InputState.Controller;
        }
    }
}
