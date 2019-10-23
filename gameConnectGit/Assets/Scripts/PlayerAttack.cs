using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public LayerMask whatIsEnemies;
    public Transform attackPos;
    public float attackRange;
    public int damage;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Danh
        if (!animator.GetBool("isAttacking"))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<BeeController>().TakeDamage(damage);
                }
            }
        }     
    }

    // Ve tam danh
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
