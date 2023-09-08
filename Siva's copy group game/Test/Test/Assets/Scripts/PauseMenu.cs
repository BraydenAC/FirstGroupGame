//Pause Menu Code
//Written by Siva
//Modified by Brayden 8/2/2023
using System.Collections;
using System.Collections.Generic;

=======
//using CE;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject pauseMenuButton1;

    private bool isPaused = false;

    void Update()
    {
        // TODO This should probably use the more modern InputSystem, instead of the older Input. Makes it easier to port to different input systems like controllers
        //      Recommended tutorial: https://youtu.be/Yjee_e4fICc?si=JHVYhsPCJDyh4qk8

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        pauseMenuButton1.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        pauseMenuButton1.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // TODO This should use AudioListener.pause when the game is paused, to pause all audio
        //      For pause menu buttons and stuff that still needs sound when paused, set AudioSource.ignoreListenerPause = true
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
