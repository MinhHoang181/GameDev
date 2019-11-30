using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExpBar : MonoBehaviour
{
    private Transform bar;

    private TextMeshPro textExp;
    private TextMeshPro level;

    private LevelSystem playerLevel;
    // Start is called before the first frame update
    void Start()
    {
        bar = transform.Find("Bar");

        textExp = transform.Find("TextExp").GetComponent<TextMeshPro>();
        level = transform.Find("Level").GetComponent<TextMeshPro>();

        playerLevel = GameObject.FindGameObjectWithTag("Player").GetComponent<LevelSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        textExp.SetText(playerLevel.currentExp.ToString() + "/" + playerLevel.expLeft.ToString() + " XP");
        level.SetText("Level " + playerLevel.level.ToString());

        float expPercent = (float)playerLevel.currentExp / (float)playerLevel.expLeft;
        setSize(expPercent);
    }

    private void setSize(float sizeNormalized)
    {
        bar.localScale = new Vector3(sizeNormalized, 1f);
    }
}
