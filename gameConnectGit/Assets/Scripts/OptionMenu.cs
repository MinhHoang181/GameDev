using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionMenu;

    public void Back()
    {
        mainMenu.SetActive(true);
        optionMenu.SetActive(false);
    }
}
