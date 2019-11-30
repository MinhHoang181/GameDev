using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefence : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isDefence = false;
    void Start()
    {
        gameObject.transform.Find("Shield").GetComponent<BoxCollider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            gameObject.transform.Find("Shield").GetComponent<BoxCollider2D>().enabled = true;
            isDefence = true;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            gameObject.transform.Find("Shield").GetComponent<BoxCollider2D>().enabled = false;
            isDefence = false;
        }
    }

    public bool IsDefence()
    {
        return isDefence;
    }
}
