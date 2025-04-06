using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public GameObject menu;
    public GameObject quit;

    public bool on;
    public bool off;

    void Start()
    {
        menu.SetActive(false);
        off = true;
        on = false;
    }

    void Update()
    {
        if (off && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            menu.SetActive(true);
            off = false;
            on = true;
        }

        else if (on && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1;
            menu.SetActive(false);
            off = true;
            on = false;
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
