using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputName : MonoBehaviour
{
    private TMP_InputField inputField;
    public TextMeshPro playerName;
    public GameObject UI;
    // Start is called before the first frame update
    void Start()
    {
        inputField = transform.Find("InputField").GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OK();
        }
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
        UI.GetComponent<PauseGame>().Resume();
    }

    public void OK()
    {
        if (inputField.text != "")
        {
            playerName.SetText(inputField.text);
        }
        gameObject.SetActive(false);
        UI.GetComponent<PauseGame>().Resume();
    }
}
