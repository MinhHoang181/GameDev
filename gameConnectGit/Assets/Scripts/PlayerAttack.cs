using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public LayerMask whatIsEnemies;
    public Transform attackPos;
    public float attackRange;

    private Animator animator;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
        player = gameObject.GetComponent<Player>();
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
                    enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(player.damage);
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
