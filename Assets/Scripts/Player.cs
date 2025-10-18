using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jump")]
    public float jumpForce = 12f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;

    [Header("Dash")]
    public float dashSpeed = 18f;
    public float dashDuration = 0.18f;
    public float dashCooldown = 0.6f;
    public float iFrameBuffer = 0.05f;

    [Header("Stats")]
    public int health = 1000;

    [Header("Hitbox")]
    public bool canTakeDamage = true;

    // --- internals ---
    private Rigidbody2D rb;
    private SpriteRenderer sr;          // <-- NEW
    private Vector2 moveInput;
    private bool isGrounded;
    private bool facingRight = true;
    private bool isDashing = false;
    private bool dashOnCooldown = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();  // <-- NEW: get the sprite renderer
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        moveInput = new Vector2(moveX, 0f).normalized;

        if (moveX < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            facingRight = false;
        }
        else if (moveX > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            facingRight = true;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isDashing)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && !dashOnCooldown)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    void FixedUpdate()
    {
        if (isDashing) return;
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        dashOnCooldown = true;
        canTakeDamage = false;

        float dir = facingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(dir * dashSpeed, 0f);

        // Start flashing effect
        StartCoroutine(IFrameFlash());

        // dash time
        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        yield return new WaitForSeconds(iFrameBuffer);
        canTakeDamage = true;

        // cooldown
        yield return new WaitForSeconds(dashCooldown);
        dashOnCooldown = false;
    }

    // flash
    private IEnumerator IFrameFlash()
    {
        float flashDuration = dashDuration + iFrameBuffer;
        float flashInterval = 0.1f; // how fast it flickers
        float elapsed = 0f;

        Color normalColor = sr.color;
        Color transparentColor = sr.color;
        transparentColor.a = 0.3f;

        while (elapsed < flashDuration)
        {
            sr.color = transparentColor;
            yield return new WaitForSeconds(flashInterval);
            sr.color = normalColor;
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval * 2f;
        }

        //set colors back
        sr.color = normalColor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if (canTakeDamage && other.CompareTag("Enemy"))
        // {
        //     health -= 100;
        //     if (health <= 0) { /* death */ }
        // }
    }
}
