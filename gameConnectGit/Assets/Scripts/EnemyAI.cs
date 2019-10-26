using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public float speed = 1f;
    public float nextWayPointDistance = 0.1f;

    public float startReachedDistance = 2f;
    public float endReachedDistance = 0.5f;

    public bool canComeBackSpawnPosition = false;
    bool isComingBack = false;
    Vector2 spawnPosition;

    public bool canWandering = false;
    public float timePerWandering = 2f;
    float timeWaitWandering;

    public float minX = -1f;
    public float maxX = 1f;
    public float minY = -0.5f;
    public float maxY = 0.5f;

    bool isWandering;
    Vector2 posBeforeWandering;
    Vector2 randomPos;

    Transform target;
    Rigidbody2D rb;
    Seeker seeker;
    Path path;

    int currentWayPoint = 0;

    bool isFollow = false;
    bool canShoot = false;
    bool facingLeft = true;

    Vector3 localScale;
    float distanceToTarget;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        localScale = transform.localScale;
        seeker = gameObject.GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
        posBeforeWandering = transform.position;
        spawnPosition = transform.position;
        randomPos = new Vector3(posBeforeWandering.x + Random.Range(minX, maxX), posBeforeWandering.y + Random.Range(minY, maxY));
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && distanceToTarget <= startReachedDistance)
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
            return;
        }

        distanceToTarget = Vector2.Distance(rb.position, target.position);

        // di chuyen den diem kha thi
        if (distanceToTarget <= startReachedDistance)
        {
            if (distanceToTarget > endReachedDistance)
            {
                rb.position = Vector2.MoveTowards(rb.position, path.vectorPath[currentWayPoint], speed * Time.deltaTime);
                isFollow = true;
                canShoot = false;
            } else
            {
                canShoot = true;
            }
            isComingBack = false;
            isWandering = false;
        } else
        {

            if (canComeBackSpawnPosition)
            {
                if (Vector2.Distance(rb.position, spawnPosition) > 0 && !isWandering)
                {
                    isComingBack = true;
                }
                else
                {
                    isComingBack = false;
                }
            }
            isFollow = false;
            canShoot = false;
            if (!isWandering)
            {
                posBeforeWandering = rb.position;
                randomPos = rb.position;
            }
            if (!isComingBack)
            {
                isWandering = true;
            } 
        }

        // Di chuyen ve spawn
        if (canComeBackSpawnPosition && isComingBack)
        {
            ComeBackSpawnPosition();
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
        if (canWandering && !isComingBack)
        {
            Wandering();
        }    
    }

    // Quay mat ve phia Player khi khong wandering va khong quay ve spawn
    void CheckWhereToFace()
    {
        if (target.position.x < transform.position.x && !isWandering && !isComingBack)
        {
            facingLeft = true;
        }
        else if (target.position.x > transform.position.x && !isWandering && !isComingBack)
        {
            facingLeft = false;
        }
        if (facingLeft && localScale.x < 0 || !facingLeft && localScale.x > 0)
        {
            localScale.x *= -1;
        }
        transform.localScale = localScale;
    }

    public bool IsFollow()
    {
        return isFollow;
    }

    public bool CanShoot()
    {
        return canShoot;
    }

    //Ve ban kinh
    private void OnDrawGizmosSelected()
    {
        // Khoang cach follow xa nhat
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, startReachedDistance);

        // Khoang cach follow gan nhat
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, endReachedDistance);

        // Khu vuc wander
        if (isWandering)
        {
            // Khi dang wandering, ve khu vuc wander
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector2(posBeforeWandering.x + minX, posBeforeWandering.y + maxY), new Vector2(posBeforeWandering.x + maxX, posBeforeWandering.y + maxY));
            Gizmos.DrawLine(new Vector2(posBeforeWandering.x + minX, posBeforeWandering.y + minY), new Vector2(posBeforeWandering.x + maxX, posBeforeWandering.y + minY));
            Gizmos.DrawLine(new Vector2(posBeforeWandering.x + minX, posBeforeWandering.y + maxY), new Vector2(posBeforeWandering.x + minX, posBeforeWandering.y + minY));
            Gizmos.DrawLine(new Vector2(posBeforeWandering.x + maxX, posBeforeWandering.y + maxY), new Vector2(posBeforeWandering.x + maxX, posBeforeWandering.y + minY));
        } else
        {
            // Khi khong wandering, ve khu vuc wander di chuyen theo nhan vat
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector2(transform.position.x + minX, transform.position.y + maxY), new Vector2(transform.position.x + maxX, transform.position.y + maxY));
            Gizmos.DrawLine(new Vector2(transform.position.x + minX, transform.position.y + minY), new Vector2(transform.position.x + maxX, transform.position.y + minY));
            Gizmos.DrawLine(new Vector2(transform.position.x + minX, transform.position.y + maxY), new Vector2(transform.position.x + minX, transform.position.y + minY));
            Gizmos.DrawLine(new Vector2(transform.position.x + maxX, transform.position.y + maxY), new Vector2(transform.position.x + maxX, transform.position.y + minY));
        }
        
    }

    void Wandering()
    {
        if (isWandering)
        {
            //Tao vi tri ngau nhien
            if (Vector2.Distance(transform.position, randomPos) <= 0.01)
            {
                if (timeWaitWandering <= 0)
                {
                    timeWaitWandering = Time.time + timePerWandering;
                }                
                if (Time.time > timeWaitWandering)
                {
                    randomPos = new Vector3(posBeforeWandering.x + Random.Range(minX, maxX), posBeforeWandering.y + Random.Range(minY, maxY));
                    timeWaitWandering = 0;
                }   
            }
            // Quay mat ve phia vi tri dinh di toi
            if (transform.position.x < randomPos.x)
            {
                facingLeft = false;
            } else if (transform.position.x > randomPos.x)
            {
                facingLeft = true;
            }
            rb.position = Vector2.MoveTowards(rb.position, randomPos, speed * Time.deltaTime);
        }
    }

    void ComeBackSpawnPosition()
    {
        if (transform.position.x < spawnPosition.x)
        {
            facingLeft = false;
        }
        else if (transform.position.x > spawnPosition.x)
        {
            facingLeft = true;
        }
        rb.position = Vector2.MoveTowards(rb.position, spawnPosition, speed * Time.deltaTime);
    }

}
