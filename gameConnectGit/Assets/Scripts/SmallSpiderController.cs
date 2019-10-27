using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallSpiderController : MonoBehaviour
{
    private Enemy enemy;
    private Animator animator;
    private Rigidbody2D rigidBody;
    private EnemyAI enemyAI;
    private EnemyShoot enemyShoot;

    float hurtTime;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        enemy = gameObject.GetComponent<Enemy>();
        enemyAI = gameObject.GetComponent<EnemyAI>();
        enemyShoot = gameObject.GetComponent<EnemyShoot>();
    }

    // Update is called once per frame
    void Update()
    {
        // nhan sat thuong
        if (enemy.IsDamaged() && !animator.GetBool("isHurting"))
        {
            animator.SetBool("isHurting", true);
            hurtTime = Time.time + 0.1f;
        }
        if (animator.GetBool("isHurting") && Time.time > hurtTime)
        {
            animator.SetBool("isHurting", false);
            enemy.IsDamaged(false);
        }

        // Chet
        if (enemy.CurrentHealth() == 0)
        {
            animator.SetBool("isDying", true);
            Destroy(gameObject.GetComponent<EnemyAI>());
            Destroy(gameObject.GetComponent < EnemyShoot>());
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Die") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                StartCoroutine(RemoveGameObject());
            }
        }

        // Di chuyen
        if (enemyAI.IsFollow() || enemyAI.IsWandering() && !enemyAI.IsIdle() || enemyAI.IsComingBack())
        {
            if (animator.GetBool("isJumping"))
            {
                animator.SetBool("isWalking", true);
            }
        } else
        {
            animator.SetBool("isWalking", false);
        }

        // Ban dan
        if (enemyShoot.IsShooting())
        {
            animator.SetBool("isShooting", true);
        }
        // Kiem tra khi nao animation Shoot chay xong thi isShooting thanh false
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            animator.SetBool("isShooting", false);
        }

        // Nhay
        if (!IsGrounded())
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("isWalking", false);
        } else
        {
            animator.SetBool("isJumping", false);
        }
    }

    // Xoa gameObject sau mot khoan thoi gian
    IEnumerator RemoveGameObject()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    // Kiem tra co tren mat dat ko
    bool IsGrounded()
    {
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, 0.25f, LayerMask.GetMask("Ground"));
        if (hitDown.collider != null)
        {
            return true;
        }
        return false;
    }
}
