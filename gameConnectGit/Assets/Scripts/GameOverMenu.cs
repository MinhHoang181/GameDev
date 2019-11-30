using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameObject UI;

    public void PlayAgain()
    {
        UI.GetComponent<PauseGame>().Resume();
        SceneManager.LoadScene("Game");
    }

    public void BackToMenu()
    {
        UI.GetComponent<PauseGame>().Resume();
        SceneManager.LoadScene("StartMenu");
    }
}
