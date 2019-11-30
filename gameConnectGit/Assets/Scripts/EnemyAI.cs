using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
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

    bool isWandering = true;
    Vector2 posBeforeWandering;
    Vector2 randomPos;

    Transform target;
    Transform playerTarget;
    Rigidbody2D rb;
    Seeker seeker;
    Path path;
    EnemyMove enemyMove;
    Transform textName;
    Transform textLevel;
    bool changeFacingText = false;

    int currentWayPoint = 0;

    bool isFollow = false;
    bool canShoot = false;
    bool isIdle = false;
    bool facingLeft = true;

    Vector3 localScale;
    float distanceToTarget;


    // Start is called before the first frame update
    void Start()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        target = playerTarget;
        rb = gameObject.GetComponent<Rigidbody2D>();
        localScale = transform.localScale;
        seeker = gameObject.GetComponent<Seeker>();
        enemyMove = gameObject.GetComponent<EnemyMove>();
        textName = transform.Find("Name");
        textLevel = transform.Find("Level");

        InvokeRepeating("UpdatePath", 0f, 0.5f);
        posBeforeWandering = transform.position;
        spawnPosition = transform.position;

        endReachedDistance += Random.Range(-0.25f, 0.25f) * endReachedDistance;
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
                enemyMove.Move(path.vectorPath[currentWayPoint]);
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
                if (Vector2.Distance(rb.position, spawnPosition) > 0.5f && !isWandering)
                {
                    isComingBack = true;
                }
                else
                {
                    isComingBack = false;
                    gameObject.GetComponent<Collider2D>().isTrigger = false;
                }
            }
            isFollow = false;
            canShoot = false;
            if (!isWandering)
            {
                posBeforeWandering = rb.position;
                randomPos = rb.position;
            }
            if (!isComingBack && canWandering)
            {
                isWandering = true;
            } 
        }

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

        if (facingLeft == false && changeFacingText == false)
        {
            textName.localScale = new Vector3(textName.localScale.x * -1, textName.localScale.y);
            textLevel.localScale = new Vector3(textLevel.localScale.x * -1, textLevel.localScale.y);
            changeFacingText = true;
        }
        else if (facingLeft == true && changeFacingText == true)
        {
            textName.localScale = new Vector3(textName.localScale.x * -1, textName.localScale.y);
            textLevel.localScale = new Vector3(textLevel.localScale.x * -1, textLevel.localScale.y);
            changeFacingText = false;
        }
    }

    public bool IsFollow()
    {
        return isFollow;
    }

    public bool IsWandering()
    {
        return isWandering;
    }

    public bool IsComingBack()
    {
        return isComingBack;
    }

    public bool IsIdle()
    {
        return isIdle;
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
                // Neu khoang canh toa do enemy voi toa do random <= 0.01f va enemy biet bay HOAC enemy khong biet bay va khoang canh toa do x giua enemy va random <= 0.01f
            if (Vector2.Distance(transform.position, randomPos) <= 0.01f && enemyMove.canFly || !enemyMove.canFly && (transform.position.x - randomPos.x) < 0.01f)
            {
                if (timeWaitWandering <= 0)
                {
                    timeWaitWandering = Time.time + timePerWandering;
                    isIdle = true;
                }                
                if (Time.time > timeWaitWandering)
                {
                    randomPos = new Vector3(posBeforeWandering.x + Random.Range(minX, maxX), posBeforeWandering.y + Random.Range(minY, maxY));
                    timeWaitWandering = 0;
                    isIdle = false;
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
            enemyMove.Wandering(randomPos);
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
        if (enemyMove.canFly)
        {
            gameObject.GetComponent<Collider2D>().isTrigger = true;
        }
        enemyMove.Move(spawnPosition);

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && isWandering && enemyMove.canFly)
        {
            randomPos = new Vector3(posBeforeWandering.x + Random.Range(minX, maxX), posBeforeWandering.y + Random.Range(minY, maxY));
            enemyMove.Move(randomPos);
        }
    }
}
