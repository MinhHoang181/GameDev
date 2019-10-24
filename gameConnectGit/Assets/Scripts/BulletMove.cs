using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float bulletSpeed;
    public int bulletDamage;

    private Rigidbody2D rigidBody;
    private Transform player;
    private PlayerDefence playerDefence;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = transform.GetComponent<Rigidbody2D>();
        playerDefence = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDefence>();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(bulletDamage);
        }
        if (collision.gameObject.tag != "Enemies")
        {
            Destroy(gameObject);
        }   
    }
}
