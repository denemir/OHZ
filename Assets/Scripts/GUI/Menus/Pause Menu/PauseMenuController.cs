using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuHandler : MonoBehaviour
{
    public Canvas pauseMenuCanvas;

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
            TogglePause();
    }

    void TogglePause()
    {
        bool isPaused = pauseMenuCanvas.isActiveAndEnabled;

        if(!isPaused)
        {
            Time.timeScale = 0; //pause
            pauseMenuCanvas.enabled = true;
        }
        else
        {
            Time.timeScale = 1; //resume
            pauseMenuCanvas.enabled = false;
        }
    }

    public void Resume()
    {
        bool isPaused = pauseMenuCanvas.isActiveAndEnabled;

        if (isPaused)
        {
            Time.timeScale = 1; //resume
            pauseMenuCanvas.enabled = false;
        }
    }
}
