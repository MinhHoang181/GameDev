using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header("Enemies:")]
    public GameObject[] enemies;
    [Space]
    [Header("Spawn Position:")]
    public GameObject[] spawnPos;
    private GameObject[] spawn;
    [Space]
    [Header("Map:")]
    public GameObject startMap;
    public GameObject[] jungleMap;
    public GameObject[] caveMap;
    [Space]
    [Header("UI")]
    public GameObject UI;
    public GameObject inputWindow;
    public GameObject gameOver;
    [Space]
    [Header("Info Player")]
    public TextMeshPro namePlayer;
    public TextMeshPro levelPlayer;
    public TextMeshProUGUI scorePlayer;

    private Animator playerAnimator;
    private int score;
    private int num = 10;
    private Vector3 start;
    private Vector3 end;
    private Transform lastMap;
    private GameObject nextMap;
    private float timeWaitBegin;
    private bool startGame = false;
    private bool endGame = false;
    private PauseGame pauseGame;
    // Start is called before the first frame update
    void Start()
    {
        // Create map right
        start = startMap.transform.Find("StartMap").position;
        end = startMap.transform.Find("EndMap").position;
        lastMap = startMap.transform;
        for (int i = 0; i < num + 1; i++)
        {
            float distanceStart = lastMap.position.x - start.x;
            float distanceEnd = end.x - lastMap.position.x;
            Vector3 position = new Vector3(lastMap.position.x + distanceStart + distanceEnd - 0f, lastMap.position.y, 0f);
            if (i == num)
            {
                nextMap = Instantiate(jungleMap[0], position, Quaternion.identity);
            } else
            {
                nextMap = Instantiate(jungleMap[Random.Range(1, jungleMap.Length)], position, Quaternion.identity);
                nextMap.GetComponent<MapController>().level += i;
                nextMap.GetComponent<MapController>().num += (int)(i / 2);
                nextMap.GetComponent<MapController>().timeRespawn -= 5 * i;
            }
            lastMap = nextMap.transform;
            start = nextMap.transform.Find("StartMap").transform.position;
            end = nextMap.transform.Find("EndMap").transform.position;
        }

        // Create map left
        start = startMap.transform.Find("StartMap").position;
        end = startMap.transform.Find("EndMap").position;
        lastMap = startMap.transform;
        for (int i = 0; i < num + 1; i++)
        {
            float distanceStart = lastMap.position.x - start.x;
            float distanceEnd = end.x - lastMap.position.x;
            Vector3 position = new Vector3(lastMap.position.x - distanceStart - distanceEnd + 0.1f, lastMap.position.y, 0f);
            if (i == 0)
            {
                nextMap = Instantiate(caveMap[0], position, Quaternion.identity);
            }
            else if (i == num)
            {
                nextMap = Instantiate(caveMap[1], position, Quaternion.identity);
            }
            else
            {
                nextMap = Instantiate(caveMap[Random.Range(2, caveMap.Length)], position, Quaternion.identity);
                nextMap.GetComponent<MapController>().level += i;
                nextMap.GetComponent<MapController>().num += i * 2;
                nextMap.GetComponent<MapController>().timeRespawn -= 6 * i;
            }
            lastMap = nextMap.transform;
            start = nextMap.transform.Find("StartMap").transform.position;
            end = nextMap.transform.Find("EndMap").transform.position;
        }

        // Update A* Gridgraph
        var gg = AstarPath.active.data.gridGraph;
        var width = gg.width * (2* num + 1);
        var depth = gg.depth;
        var nodeSize = gg.nodeSize;

        //gg.center = new Vector3((num) * 4, 0, 0);

        gg.SetDimensions(width, depth, nodeSize);

        

        pauseGame = UI.GetComponent<PauseGame>();
        timeWaitBegin = 0.5f;

        // Player
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>();
        score = 0;
        scorePlayer.SetText(score.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        /*
        // Create enemy
        spawn = GameObject.FindGameObjectsWithTag("Enemies");
        if (spawn.Length < 5)
        {
            Instantiate(enemies[Random.Range(0, enemies.Length)], spawnPos[Random.Range(0,spawnPos.Length)].transform.position, Quaternion.identity);
        }
        */
        
        if (timeWaitBegin > 0)
        {
            timeWaitBegin -= Time.deltaTime;
        } else
        {
            if (!startGame)
            {
                pauseGame.Pause(inputWindow);
                startGame = true;

                AstarPath.active.Scan();
            }
        }
        

        // Check gameover
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Death") && playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            if (!endGame)
            {
                GameOver();
                endGame = true;
            }
        }
    }

    private void GameOver()
    {
        pauseGame.Pause(gameOver);

        

        // Create
        HighscoreRow highscoreRow = new HighscoreRow { name = namePlayer.text, level = levelPlayer.text, score = score };

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

    public void SetScore(int point)
    {
        score += point;
        scorePlayer.SetText(score.ToString());
    }
}
