using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuHandler : MonoBehaviour
{
    //vars
    public Canvas pauseMenuCanvas;
    public bool isCurrentlyPaused = false;
    public Player player;

    //displays
    public WeaponInventoryDisplay displaySlot1;
    public WeaponInventoryDisplay displaySlot2;
    public WeaponInventoryDisplay displaySlot3;

    // Start is called before the first frame update
    void Start()
    {
        //pauseMenuCanvas = GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        //check if pause key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UpdateDisplays();
            TogglePause();
        }
            

        if (pauseMenuCanvas.isActiveAndEnabled)
            isCurrentlyPaused = true;
        else isCurrentlyPaused = false;
    }

    //pausing functions
    public void TogglePause()
    {
        bool isPaused = pauseMenuCanvas.isActiveAndEnabled;

        if(!isPaused)
        {
            Time.timeScale = 0; //pause
            pauseMenuCanvas.enabled = true;
            isCurrentlyPaused = true;
        }
        else
        {
            Time.timeScale = 1; //resume
            pauseMenuCanvas.enabled = false;
            isCurrentlyPaused = false;
        }
    }

    //gui
    public void Resume()
    {
        TogglePause();
    }

    public void UpdateDisplays()
    {
        if (player.GetPlayerInventory().weapons[0] != null && displaySlot1 != null)
            displaySlot1.setWeapon(player.GetPlayerInventory().weapons[0]);

        if (player.GetPlayerInventory().weapons[1] != null && displaySlot2 != null)
            displaySlot2.setWeapon(player.GetPlayerInventory().weapons[1]);

        if (player.GetPlayerInventory().weapons.Length == 3 && player.GetPlayerInventory().weapons[2] != null && displaySlot3 != null)
            displaySlot3.setWeapon(player.GetPlayerInventory().weapons[2]);
    }

    //getters and setters
    public bool currentlyPaused()
    {
        return isCurrentlyPaused;
    }
}
