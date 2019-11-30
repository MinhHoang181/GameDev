using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject statsMenu;

    public void Back()
    {
        mainMenu.SetActive(true);
        statsMenu.SetActive(false);
    }
}
