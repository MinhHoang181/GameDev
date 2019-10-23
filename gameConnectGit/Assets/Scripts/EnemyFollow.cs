using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float speed;
    public float distanceFollow;

    private Transform target;
    private Vector3 localScale;
    private bool facingLeft = true;
    private bool isFollow;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, target.position) > distanceFollow)
        {
            isFollow = true;
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        } else
        {
            isFollow = false;
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
