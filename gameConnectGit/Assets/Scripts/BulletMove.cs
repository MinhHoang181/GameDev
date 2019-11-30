using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    private float bulletSpeed;
    private int bulletDamage;

    private Rigidbody2D rigidBody;
    private GameObject whoShoot;
    private GameObject bullet;

    private Vector3 target;
    private Animator animator;
    private Player player;
    private PlayerDefence playerDefence;

    private float temp;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = transform.GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerDefence = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDefence>();
        animator = gameObject.GetComponentInChildren<Animator>();
        temp = player.speed;
    }

    void Update()
    {
        if (bullet.name == "BeeBullet")
        {
            MoveForward();
        } else if (bullet.name == "SpiderWeb")
        {
            MoveToTarget();

            // Chay aniamtion khi toi vi tri
            if (transform.position == target)
            {
                animator.SetBool("isHit", true);
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("SpiderWeb") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                Destroy(gameObject);
            }
        }
    }

    // Di thang
    void MoveForward()
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

    // Ban ve huong muc tieu
    void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, bulletSpeed * Time.deltaTime);
    }

    // Lay gia tri speed va damage cua dan + GameObject cua doi tuong ban dan
    public void SetBulletInfo(float speed, int damage, GameObject owner, GameObject bulletSpawn)
    {
        bulletSpeed = speed;
        bulletDamage = damage;
        whoShoot = owner;
        bullet = bulletSpawn;
    }

    // Xoa khi ra khoi tam nhin
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    // Xu li khi va cham
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!playerDefence.IsDefence() || bullet.name == "BeeBullet")
            {
                collision.gameObject.GetComponent<Player>().TakeDamage(bulletDamage);
                collision.gameObject.GetComponent<PlayerController>().IsHurt();
            }
            if (bullet.name == "SpiderWeb")
            {
                collision.gameObject.GetComponent<Player>().EffectSlow(0.5f,1f);
            }
        }
        if (collision.gameObject.tag != "Enemies" && collision.gameObject.tag != "Bullet" && collision.gameObject.tag != "Map")
        {
            if (bullet.name == "BeeBullet")
            {
                Destroy(gameObject);
            }
        }
    }
}

