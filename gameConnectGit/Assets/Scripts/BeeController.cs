using System.Collections;
using UnityEngine;

public class BeeController : MonoBehaviour
{
    [Header("Enemies stats:")]
    public int maxHealth;
    private int currentHealth;

    [Space]
    [Header("Reference:")]
    public Animator animator;
    public LayerMask groundLayer;

    private Rigidbody2D rigidBody;
    private EnemyShoot enemyShoot;

    private float hurtTime;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = transform.GetComponent<Rigidbody2D>();
        enemyShoot = gameObject.GetComponent<EnemyShoot>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // bi thuong
        if (animator.GetBool("isHurting") && Time.time > hurtTime)
        {
            animator.SetBool("isHurting", false);
        }

        // Chet
        if (currentHealth == 0)
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

    // nhan sat thuong
    public void TakeDamage(int damage)
    {
        StartCoroutine(WaitAttackThenTakeDamaged(damage));
    }
    // cho mot khoan thoi gian roi moi chay animation
    IEnumerator WaitAttackThenTakeDamaged(int damage)
    {
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("isHurting", true);
        hurtTime = Time.time + 0.1f;
        currentHealth -= damage;
    }

    // Khi cham mat dat thi set trang thai chet
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && animator.GetBool("isFalling"))
        {
            animator.SetBool("isFalling", false);
            animator.SetBool("isDying", true);
            rigidBody.simulated = false;
            Destroy(gameObject.GetComponent<EnemyFollow>());
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
