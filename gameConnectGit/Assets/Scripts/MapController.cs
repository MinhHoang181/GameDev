using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [Header("Background Object:")]
    public GameObject[] background;
    [Header("Ground Object:")]
    public GameObject groundBegin;
    public GameObject groundEnd;
    public GameObject[] ground;
    [Header("Enemies:")]
    public int num;
    public int level;
    public GameObject[] enemies;
    public float timeRespawn;
    public LayerMask whatIsEnemies;
    private float countTime;
    [Header("Point map:")]
    public Transform startMap;
    public Transform endMap;

    private Transform lastGround;
    private GameObject nextGround;
    private Vector3 startPoint;
    private Vector3 endPoint;

    // Start is called before the first frame update
    void Start()
    {
        // Create ground
        lastGround = groundBegin.transform;
        startPoint = lastGround.Find("StartPoint").position;
        endPoint = lastGround.Find("EndPoint").position;
        while (groundEnd.transform.Find("StartPoint").position.x > endPoint.x)
        {
            float distanceEnd = endPoint.x - lastGround.position.x;
            if (groundEnd.transform.Find("StartPoint").position.x - endPoint.x < 1.367f)
            {
                nextGround = Instantiate(ground[0], lastGround.position, Quaternion.identity);
            } else
            {
                nextGround = Instantiate(ground[Random.Range(0, ground.Length)], lastGround.position, Quaternion.identity);
            }
            nextGround.transform.parent = transform.Find("Middleground");
            lastGround = nextGround.transform;
            startPoint = lastGround.Find("StartPoint").position;
            float distanceStart = lastGround.position.x - startPoint.x;
            Vector3 position = new Vector3(lastGround.position.x + distanceStart + distanceEnd, lastGround.position.y);
            lastGround.position = position;
            endPoint = lastGround.Find("EndPoint").position;
        }

        countTime = timeRespawn;
        // Spawn enemies
        for (int i = 0; i < num; i++)
        {
            GameObject enemy = Instantiate(enemies[0], new Vector3(Random.Range(startMap.position.x + 0.5f, endMap.position.x - 0.5f), -0.6f), Quaternion.identity);
            enemy.GetComponent<Enemy>().level = level;
        }
    }

    // Update is called once per frame
    void Update()
    {
        countTime -= Time.deltaTime;
        if (countTime < 0)
        {
            Collider2D[] spawn = Physics2D.OverlapCircleAll(transform.position, 3f, whatIsEnemies);
            if (spawn.Length < 1)
            {
                for (int i = 0; i < num - spawn.Length; i++)
                {
                    GameObject enemy = Instantiate(enemies[0], new Vector3(Random.Range(startMap.position.x + 0.5f, endMap.position.x - 0.5f), -0.6f), Quaternion.identity);
                    enemy.GetComponent<Enemy>().level = level;
                }
            }
            countTime = timeRespawn;
        }
    }
}
