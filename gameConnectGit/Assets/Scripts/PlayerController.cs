using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Animator animator;
    private Player player;

    private bool facingRight = true;
    private Vector3 localScale;

    // Start is called before the first frame update
    void Start()
    { 
        rigidBody = transform.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponentInChildren<Animator>();
        player = gameObject.GetComponent<Player>();
        localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // Nhay
        if (Input.GetKeyDown(KeyCode.W) && IsGrounded() && !animator.GetBool("isAttacking") && !animator.GetBool("isShielding") && !animator.GetBool("isDead"))
        {
            rigidBody.velocity = Vector2.up * player.jumpSpeed;
            animator.SetBool("isJumping", true);
        }

        // Danh
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!animator.GetBool("isAttacking"))
            {
                animator.SetBool("isAttacking", true);
            }
        }
            // Kiem tra khi nao ket thuc animmation Attack thi set isAttack thanh false lai
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_1") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f || animator.GetBool("isFalling"))
        {
            animator.SetBool("isAttacking", false);
        }

        // Nhao lon
        if (animator.GetBool("isRunning") && Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetBool("isRolling", true);
        }
        else
        {
            animator.SetBool("isRolling", false);
        }

        // Bao ve
        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.SetBool("isShielding", true);
        } else if (Input.GetKeyUp(KeyCode.S))
        {
            animator.SetBool("isShielding", false);
        }
    }

    private void FixedUpdate()
    {
        // Di chuyen
        float moveInput = Input.GetAxis("Horizontal");
        if (moveInput != 0 && IsGrounded())
        {
            animator.SetBool("isRunning", true);
        } else
        {
            animator.SetBool("isRunning", false);
        }

        SetAnimationState();

        if (!animator.GetBool("isAttacking") && !animator.GetBool("isRolling") && !animator.GetBool("isShielding") && !animator.GetBool("isDead"))
        {
            rigidBody.velocity = new Vector2(moveInput * player.speed, rigidBody.velocity.y);
        }
        
    }

    private void LateUpdate()
    {
        CheckWhereToFace();
    }

    // Kiem tra Character co tren mat dat ko
    bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 0.2f;
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }

    // Set trang thai animation nao hoat dong
    void SetAnimationState()
    {
        // Set Jump va Fall
        if (rigidBody.velocity.y == 0)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }
        if (rigidBody.velocity.y < 0 && !IsGrounded())
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }

        // Chet
        if (player.CurrentHealth() == 0 && !animator.GetBool("isDead"))
        {
            animator.SetBool("isDead", true);
        }

    }


    // Kiem tra nhan vat dang nhin ve huong nao thi animation quay ve huong do
    void CheckWhereToFace()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            facingRight = true;
        } else if (Input.GetAxis("Horizontal") < 0)
        {
            facingRight = false;
        }
        if (facingRight && localScale.x < 0 || !facingRight && localScale.x > 0)
        {
            localScale.x *= -1;
        }
        if (!animator.GetBool("isDead"))
        {
            transform.localScale = localScale;
        }
        
    }
}
