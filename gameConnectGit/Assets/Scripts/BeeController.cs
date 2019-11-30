using System.Collections;
using UnityEngine;

public class BeeController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigidBody;
    private EnemyShoot enemyShoot;
    private Enemy enemy;

    private float hurtTime;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
        rigidBody = transform.GetComponent<Rigidbody2D>();
        enemyShoot = gameObject.GetComponent<EnemyShoot>();
        enemy = gameObject.GetComponent<Enemy>();
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
            animator.SetBool("isFalling", true);
            rigidBody.velocity = Vector2.down * 1.5f;
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
    }

    // Khi cham mat dat thi set trang thai chet
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && animator.GetBool("isFalling"))
        {
            animator.SetBool("isFalling", false);
            animator.SetBool("isDying", true);
            rigidBody.simulated = false;
            Destroy(gameObject.GetComponent<EnemyAI>());
            Destroy(gameObject.GetComponent<EnemyShoot>());
            StartCoroutine(RemoveGameObject());
        }
    }

    // Xoa gameObject sau mot khoan thoi gian
    IEnumerator RemoveGameObject()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
