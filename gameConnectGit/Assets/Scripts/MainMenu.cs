using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionMenu;
    public GameObject statsMenu;

    private void Start()
    {
        Time.timeScale = 1f;
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void Option()
    {
        mainMenu.SetActive(false);
        optionMenu.SetActive(true);
    }

    public void Stats()
    {
        mainMenu.SetActive(false);
        statsMenu.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
