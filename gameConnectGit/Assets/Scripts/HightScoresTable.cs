using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HightScoresTable : MonoBehaviour
{
    private Transform row;
    private List<HighscoreRow> highscoreRowList;
    private List<Transform> highscoreRowTransformList;
    private void Awake()
    {
        row = transform.Find("HightScoreRow");

        highscoreRowTransformList = new List<Transform>();

        // load list
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscore highscore = JsonUtility.FromJson<Highscore>(jsonString);

        // Sap xep
        for (int i = 0; i < highscore.highscoreRowsList.Count; i++)
        {
            for (int j = i; j < highscore.highscoreRowsList.Count; j++)
            {
                if (highscore.highscoreRowsList[j].score > highscore.highscoreRowsList[i].score)
                {
                    HighscoreRow tmp = highscore.highscoreRowsList[i];
                    highscore.highscoreRowsList[i] = highscore.highscoreRowsList[j];
                    highscore.highscoreRowsList[j] = tmp;
                }
            }
        }

        int numRows = Mathf.Min(6, highscore.highscoreRowsList.Count);
        int count = 0;
        foreach (HighscoreRow hightScoreRow in highscore.highscoreRowsList)
        {
            if (count < numRows)
            {
                CreateHighScoreRowTransform(hightScoreRow, transform, highscoreRowTransformList);
                count++;
            } else
            {
                break;
            }
        }

        // Remove json
        if (highscore.highscoreRowsList.Count > numRows)
        {
            for (int i = numRows; i < highscore.highscoreRowsList.Count; i++)
            {
                highscore.highscoreRowsList.Remove(highscore.highscoreRowsList[i]);
            }
        }
        string json = JsonUtility.ToJson(highscore);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    private void CreateHighScoreRowTransform(HighscoreRow highscoreRow, Transform container, List<Transform> transformsList)
    {
        Transform rowTransform = Instantiate(row, container);
        RectTransform rowRectTransform = rowTransform.GetComponent<RectTransform>();
        rowRectTransform.anchoredPosition = new Vector2(0, 250 - 100 * transformsList.Count);
        rowTransform.gameObject.SetActive(true);

        rowTransform.Find("Background").gameObject.SetActive(transformsList.Count % 2 == 0);

        // Pos
        int rank = transformsList.Count + 1;
        string rankString;
        switch (rank)
        {
            default: rankString = rank + "TH"; break;

            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }
        rowTransform.Find("Pos").GetComponent<TextMeshProUGUI>().SetText(rankString);

        // Name
        rowTransform.Find("Name").GetComponent<TextMeshProUGUI>().SetText(highscoreRow.name);

        // Level
        rowTransform.Find("Level").GetComponent<TextMeshProUGUI>().SetText(highscoreRow.level);

        // Score
        rowTransform.Find("Score").GetComponent<TextMeshProUGUI>().SetText(highscoreRow.score.ToString());

        transformsList.Add(rowTransform);
    }

    public void AddHighscoreRow(string name, string level, int score)
    {
        // Create
        HighscoreRow highscoreRow = new HighscoreRow { name = name, level = level, score = score };

        // Load save
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscore highscore = JsonUtility.FromJson<Highscore>(jsonString);

        // Add new highscore to list
        highscore.highscoreRowsList.Add(highscoreRow);

        // Save update
        string json = JsonUtility.ToJson(highscore);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }
}

public class Highscore
{
    public List<HighscoreRow> highscoreRowsList;
}

[System.Serializable]
public class HighscoreRow
{
    public string name;
    public string level;
    public int score;
}

