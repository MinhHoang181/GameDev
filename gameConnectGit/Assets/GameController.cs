using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject bee;
    public GameObject spider;
    public GameObject[] spawnPos;
    GameObject[] enemies;
    List<GameObject> list = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        list.Add(bee);
        list.Add(spider);
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemies");
        if (enemies.Length < 5)
        {
            Instantiate(list[Random.Range(0, list.Count)], spawnPos[Random.Range(0,spawnPos.Length)].transform.position, Quaternion.identity);
        }
    }
}
