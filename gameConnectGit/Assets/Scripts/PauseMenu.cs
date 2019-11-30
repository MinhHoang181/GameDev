using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject UI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume()
    {
        UI.GetComponent<PauseGame>().Resume();
    }

    public void Menu()
    {
        UI.GetComponent<PauseGame>().Resume();
        SceneManager.LoadScene("StartMenu");      
    }

    public void Quit()
    {
        Application.Quit();
    }
}
