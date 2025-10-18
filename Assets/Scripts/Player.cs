using System;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] [Range(0, 1)] private float jumpDeadzone = 0.02f;
    [SerializeField] private float standardGravity = 3;
    [SerializeField] private float holdJumpGravityModifier = 1;

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
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    
    private float horizMoveInput;
    private float jumpAxis;
    private bool jumping => jumpAxis > jumpDeadzone;
    private bool facingRight;
    private bool bufferedJump = false;

    private float dashTimer = 0;
    private bool dashRight = true;
    private bool dashing => dashTimer < 0;
    private bool dashOnCooldown => dashTimer < dashCooldown;

    private float noInputTimer = 0;
    private bool canInput => noInputTimer <= 0;


    private bool grounded = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        horizMoveInput = context.ReadValue<float>();
    }

    public void JumpInput(InputAction.CallbackContext context)
    {
        float val = context.ReadValue<float>();
        
        if (val == 0)
        {
            bufferedJump = false;
        } else if(val > jumpDeadzone && !jumping && grounded)
        {
            bufferedJump = true;
        }
        jumpAxis = val;
    }

    public void DodgeRoll()
    {
        if (dashOnCooldown || !canInput)
        {
            return;
        }
        
        noInputTimer = dashDuration;
        dashTimer = -dashDuration;
        dashRight = facingRight;
    }

    private void Update()
    {
        if (bufferedJump && canInput)
        {
            rb.AddForceY(jumpForce);
            bufferedJump = false;
        }
        
        if (horizMoveInput < 0)
        {
            facingRight = false;
        } else if (horizMoveInput > 0)
        {
            facingRight = true;
        }
        
        dashTimer += Time.deltaTime;
        noInputTimer -= Time.deltaTime;
        
        if (canInput)
        {
            rb.AddForceX(horizMoveInput * acceleration * Time.deltaTime * rb.mass, ForceMode2D.Impulse);
            rb.linearVelocityX = Mathf.Clamp(rb.linearVelocityX, -moveSpeed, moveSpeed);

            if (rb.linearVelocityY > 0)
            {
                rb.gravityScale = Mathf.Lerp(standardGravity, standardGravity * holdJumpGravityModifier, jumpAxis);
            }
            else
            {
                rb.gravityScale = standardGravity;
            }
        }

        grounded = Physics2D.OverlapBox((Vector2)boxCollider.bounds.center - new Vector2(0, boxCollider.bounds.size.y * 0.5f), new Vector2(boxCollider.bounds.size.x * 0.9f, 0.1f), 0, boxCollider.includeLayers);
        
        
        if (Mathf.Abs(horizMoveInput) < 0.01d)
        {
            rb.AddForceX(Mathf.Min(deceleration * Time.deltaTime, Mathf.Abs(rb.linearVelocityX)) * -Mathf.Sign(rb.linearVelocityX) * rb.mass, ForceMode2D.Impulse);
        }
        
        if (dashing)
        {
            rb.linearVelocityX = dashSpeed * (dashRight ? 1 : -1);
            canTakeDamage = false;
        }


        spriteRenderer.flipX = !facingRight;
    }

    void FixedUpdate()
    {
        //if (isDashing) return;
        //rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }
/*
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
    }*/

    // flash
    private IEnumerator IFrameFlash()
    {
        float flashDuration = dashDuration + iFrameBuffer;
        float flashInterval = 0.1f; // how fast it flickers
        float elapsed = 0f;

        Color normalColor = spriteRenderer.color;
        Color transparentColor = spriteRenderer.color;
        transparentColor.a = 0.3f;

        while (elapsed < flashDuration)
        {
            spriteRenderer.color = transparentColor;
            yield return new WaitForSeconds(flashInterval);
            spriteRenderer.color = normalColor;
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval * 2f;
        }

        //set colors back
        spriteRenderer.color = normalColor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if (canTakeDamage && other.CompareTag("Enemy"))
        // {
        //     health -= 100;
        //     if (health <= 0) { /* death */ }
        // }
    }

    private void OnDrawGizmosSelected()
    {
        if (boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider2D>();
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)boxCollider.bounds.center - new Vector2(0, boxCollider.bounds.size.y * 0.5f), new Vector2(boxCollider.bounds.size.x * 0.9f, 0.1f));
    }
}
