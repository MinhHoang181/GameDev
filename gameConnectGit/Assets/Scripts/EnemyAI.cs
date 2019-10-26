using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public float speed = 200f;
    public float nextWayPointDistance = 0.1f;
    public float endReachedDistance = 0.5f;

    Transform target;
    Rigidbody2D rb;
    Seeker seeker;
    Path path;

    int currentWayPoint = 0;
    bool reachedEndOfPath = false;
    bool isFollow = false;
    bool facingLeft = true;
    Vector3 localScale;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        localScale = transform.localScale;
        seeker = gameObject.GetComponent<Seeker>();
        
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (path == null)
        {
            return;
        }
        if (currentWayPoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }
        if (Vector2.Distance(rb.position, target.position) > endReachedDistance)
        {
            rb.position = Vector2.MoveTowards(rb.position, path.vectorPath[currentWayPoint], speed * Time.deltaTime);
            //rb.velocity = ((Vector2)path.vectorPath[currentWayPoint] - rb.position) * speed;
            isFollow = true;
        } else
        {
            isFollow = false;
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
        if (distance < nextWayPointDistance)
        {
            currentWayPoint++;
        }
    }

    private void LateUpdate()
    {
        CheckWhereToFace();
    }

    void CheckWhereToFace()
    {
        if (target.position.x > transform.position.x)
        {
            facingLeft = true;
        }
        else if (target.position.x < transform.position.x)
        {
            facingLeft = false;
        }
        if (facingLeft && localScale.x > 0 || !facingLeft && localScale.x < 0)
        {
            localScale.x *= -1;
        }
        transform.localScale = localScale;
    }

    public bool IsFollow()
    {
        return isFollow;
    }
}
