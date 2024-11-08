using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playermovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingpower = 25f;
    private bool IsfacingRight = true;
    private bool canJump = false;

    private float chargeTime = 1f;
    private float chargeBuildup = 20f;
    private float chargeCooldown = 2.3f;
    private bool isRushing;
    private bool isBuildingUp;
    private bool canRush = true;
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    private Animator animator;

    PlayerLife playerLife;

    [SerializeField] public CapsuleCollider2D attackCollider;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundlayor;
    [SerializeField] private TrailRenderer tr;
    // Update is called once per frame

    private void Start()
    {
        attackCollider = GetComponentInChildren<CapsuleCollider2D>();
        playerLife = FindObjectOfType<PlayerLife>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (isDashing)
        {
            return;
        }
        if (isRushing)
        {
            return;
        }



        horizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space)  && canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingpower);
        }
        if (Input.GetKey(KeyCode.C))
        {
            isBuildingUp = true;
            chargeBuildup += Time.deltaTime * 2.5f;
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            StartCoroutine(Rush());
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        Flip();
        
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        if (isRushing)
        {
            return;
        }

        Vector2 movement = new Vector2(horizontal * speed, rb.velocity.y);
        rb.velocity = movement;
        float absoluteSpeed = Mathf.Abs(horizontal * speed);
        animator.SetFloat("Speed", absoluteSpeed);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundlayor);
    }

    private void Flip()
    {
        if (IsfacingRight && horizontal < 0f || !IsfacingRight && horizontal > 0f)
        {
            IsfacingRight = !IsfacingRight;
            Vector3 localscale = transform.localScale;
                localscale.x *= -1f;
            transform.localScale = localscale;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (isRushing)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerLife.playerHP -= 10;
        }
        if (collision.collider.gameObject.CompareTag("Trap"))
        {
            playerLife.playerHP -= 100;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("JumpPowerup"))
        {
            canJump = true;
            Debug.Log("Press Space to Jump");
        }
        if (collision.CompareTag("LevelEndDoor"))
        {
            EndLevel();
        }
    }
    private IEnumerator Rush()
    {
        isBuildingUp = false;
        canRush = false;
        isRushing = true;
        tr.emitting = true;
        animator.SetBool("Charge", isRushing);
        attackCollider.enabled = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0.7f;
        if (IsfacingRight)
        {
            rb.velocity = new Vector2(chargeBuildup * 1f, 0f);
        }
        else if (!IsfacingRight)
        {
            rb.velocity = new Vector2(chargeBuildup * -1f, 0f);
        }
        yield return new WaitForSeconds(chargeTime);
        isRushing = false;
        tr.emitting = false;
        attackCollider.enabled = false;
        animator.SetBool("Charge", isRushing);
        rb.gravityScale = originalGravity;
        speed = 1f;
        yield return new WaitForSeconds(chargeCooldown);
        speed = 8f;
        canRush = true;
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        tr.emitting = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 10f;
        animator.SetBool("Dash", isDashing);
        if (IsfacingRight)
        {
            rb.velocity = new Vector2(dashingPower * 1, 0f);
        }
        else if (!IsfacingRight)
        {
            rb.velocity = new Vector2(dashingPower * -1, 0f);
        }
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        isDashing = false;
        rb.gravityScale = originalGravity;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    private void EndLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
