using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float bulletSpeed;

    private Rigidbody2D rigidBody;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = transform.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (player.position.x > transform.position.x)
        {
            rigidBody.velocity = new Vector2(bulletSpeed, 0);
        } else
        {
            rigidBody.velocity = new Vector2(bulletSpeed * -1, 0);
        }
        
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
