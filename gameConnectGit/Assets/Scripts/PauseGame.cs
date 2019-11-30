using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PauseGame : MonoBehaviour
{
    public static bool gameIsPause = false;

    public GameObject pauseGame;
    public GameObject pauseMenu;
    public GameObject inputWindow;
    public GameObject gameOver;
    // Update is called once per frame
    private void Start()
    {
        gameIsPause = false;
        Time.timeScale = 1f;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOver.activeInHierarchy && !inputWindow.activeInHierarchy)
        {
            if (gameIsPause)
            {
                Resume();
            } else
            {
                Pause(pauseMenu);
            }
        }
    }

    public void Pause(GameObject scene)
    {
        pauseGame.SetActive(true);
        scene.SetActive(true);
        Time.timeScale = 0f;
        gameIsPause = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        gameIsPause = false;
        pauseGame.SetActive(false);
    }

}
