using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject panelPause;
    [SerializeField] GameObject panelMainMenu;
    PauseManager pauseManager;
    bool isPaused;
    public Action<bool> OnPauseButtonPressed;


    void Awake()
    {
        pauseManager= GetComponent<PauseManager>();
    }

    public void PauseButtonDown()
    {
        if (isPaused)
        { 
            UnPause();
            return;
        }
        isPaused= true;
        OnPauseButtonPressed?.Invoke(isPaused);
        
        pauseManager.PauseGame();
        panelPause.SetActive(true);

    }

    public void UnPause()
    {
        pauseManager.UnPauseGame();
        panelPause.SetActive(false);
        isPaused = false;

        OnPauseButtonPressed?.Invoke(isPaused);
    }

    public void GoToMainMenu()
    {
        panelPause.SetActive(false);
        UnPause();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
