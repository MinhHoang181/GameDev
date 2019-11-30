using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public bool canFly = false;

    private Rigidbody2D rb;
    private Enemy enemy;

    private RaycastHit2D hitX;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        enemy = gameObject.GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool IsGrounded()
    {
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, 0.2f, LayerMask.GetMask("Ground"));
        if (hitDown.collider != null)
        {
            return true;
        }
        return false;
    }

    public void Move(Vector2 targetPos)
    {
        if (canFly)
        {
            rb.position = Vector2.MoveTowards(rb.position, targetPos, enemy.speed * Time.deltaTime);
        } else
        {
            if (targetPos.x > transform.position.x)
            {
                transform.Translate(Vector2.right * enemy.speed * Time.deltaTime);
                hitX = Physics2D.Raycast(transform.position, Vector2.right, 0.25f, LayerMask.GetMask("Ground"));
            } else if (targetPos.x < transform.position.x)
            {
                transform.Translate(Vector2.left * enemy.speed * Time.deltaTime);
                hitX = Physics2D.Raycast(transform.position, Vector2.left, 0.25f, LayerMask.GetMask("Ground"));
            }
            if (hitX.collider != null && IsGrounded())
            {
                rb.velocity = Vector2.up * 3;
            }
        }
    }

    public void Wandering(Vector2 randomPos)
    {
        if (canFly)
        {
            rb.position = Vector2.MoveTowards(rb.position, randomPos, enemy.speed * Time.deltaTime);
        } else
        {
            rb.position = Vector2.MoveTowards(rb.position, new Vector2(randomPos.x,rb.position.y), enemy.speed * Time.deltaTime);
            if (randomPos.x > transform.position.x)
            {
                hitX = Physics2D.Raycast(transform.position, Vector2.right, 0.25f, LayerMask.GetMask("Ground"));
            }
            else if (randomPos.x < transform.position.x)
            {
                hitX = Physics2D.Raycast(transform.position, Vector2.left, 0.25f, LayerMask.GetMask("Ground"));
            }
            if (hitX.collider != null && IsGrounded())
            {
                rb.velocity = Vector2.up * 3;
            }
        }
    }
}
