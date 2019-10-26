using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    private float bulletSpeed;
    private int bulletDamage;

    private Rigidbody2D rigidBody;
    private GameObject whoShoot;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = transform.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rigidBody.velocity.x == 0)
        {
            if (whoShoot.transform.position.x < transform.position.x)
            {
                rigidBody.velocity = new Vector2(bulletSpeed, 0);
            }
            else
            {
                rigidBody.velocity = new Vector2(bulletSpeed * -1, 0);
            }
        }
        
    }

    // Lay gia tri speed va damage cua dan + GameObject cua doi tuong ban dan
    public void SetBulletInfo(float speed, int damage, GameObject owner)
    {
        bulletSpeed = speed;
        bulletDamage = damage;
        whoShoot = owner;
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
        if (collision.gameObject.tag != "Enemies" && collision.gameObject.tag != "Bullet")
        {
            Destroy(gameObject);
        }
    }
}
