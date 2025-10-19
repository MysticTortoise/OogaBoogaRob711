using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

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
    [SerializeField] private float noHoldJumpModifier = 1;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 18f;
    [SerializeField] private float dashDuration = 0.18f;
    [SerializeField] private float dashCooldown = 0.6f;
    [SerializeField] private float iFrameBuffer = 0.05f;
    [SerializeField] private float flashInterval;
    [SerializeField] private float dashHitboxExtra;

    [Header("Dash After Image")]
    public GameObject afterImagePrefab;
    public float afterImageLifetime = 0.3f;
    public float afterImageSpawnInterval = 0.05f;
    public Color afterImageColor = new Color(1f, 0f, 0f, 0.5f);
    public Sprite afterImageSprite; 

    [Header("Stats")]
    public int health = 1000;

    [Header("Hitbox")]
    public bool canTakeDamage = true;
    
    [Header("Camera")]
    [SerializeField] private float cameraAheadAmount;
    [SerializeField] private float cameraTweenAmount;
    [SerializeField] private float cameraYAmount;
    public float normalZoom;
    public float ZoomOut;
    public float ZoomIn;
    
    [Header("References")]
    public CameraVFX cameraVFX;

    [Header("Stick")]
    [SerializeField] private Vector2 stickboxOffset;
    [SerializeField] private Vector2 stickboxSize;
    [SerializeField] private float stickAttackTime;
    [SerializeField] private float stickCooldown;
    [Header("Rocks")]
    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private int maxRocks;
    [SerializeField] private float throwSpeed;
    [SerializeField] private float throwCooldown;

    // --- internals ---
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Transform cameraTransform;
    private Camera cameraComp;
    
    // Private Variables
    // --- Input Axis ---
    private float horizMoveInput;
    private float jumpAxis;

    private Vector2 aimPos;
    // Jump Variables
    private bool jumping => jumpAxis > jumpDeadzone;
    private bool bufferedJump;
    
    // Move Variables
    private bool facingRight;
    
    // Dash Variables
    private float dashTimer;
    private bool dashRight = true;
    private bool dashing => dashTimer < 0;
    private bool dashOnCooldown => dashTimer < dashCooldown;

    // Input Variables
    private float noInputTimer;
    private bool canInput => noInputTimer <= 0;
    
    // Attack Variables
    private List<ThrownRock> rocks = new List<ThrownRock>();
    private float stickCooldownTimer;
    private float stickAttackTimer;
    private float rockAttackTimer;

    // Statuses
    private bool grounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        cameraTransform = transform.parent.Find("Camera");
        cameraComp = cameraTransform.GetComponent<Camera>();
        
        for (int i = 0; i < maxRocks; i++)
        {
            rocks.Add(Instantiate(rockPrefab).GetComponent<ThrownRock>());
        }
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

    public void AimInput(InputAction.CallbackContext context)
    {
        aimPos = context.ReadValue<Vector2>() / new Vector2(Screen.width, Screen.height);
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
        StartCoroutine(SpawnAfterImages(dashDuration));
        StartCoroutine(IFrameFlash());
        cameraVFX.PunchZoom(ZoomIn);
    }

    public void Strike()
    {
        if (stickCooldownTimer > 0)
        {
            return;
        }

        animator.SetTrigger("StickAttack");
        stickCooldownTimer = stickCooldown;
        stickAttackTimer = stickAttackTime;
        
    }

    public void ThrowRock(InputAction.CallbackContext context)
    {
        if (!context.started || rockAttackTimer > 0)
        {
            return;
        }
        foreach (ThrownRock rock in rocks)
        {
            if (!rock.IsActive())
            {
                Vector3 target = cameraComp.ViewportToWorldPoint(aimPos);
                rock.Throw(transform.position, (target - transform.position).normalized * throwSpeed);
                rockAttackTimer = throwCooldown;
                return;
            }
        }
    }

    private void Update()
    {
        // Jump Handler
        if (bufferedJump && canInput)
        {
            rb.AddForceY(jumpForce, ForceMode2D.Impulse);
            bufferedJump = false;
        }
        
        // Left / Right checker
        if (horizMoveInput < 0)
        {
            facingRight = false;
        } else if (horizMoveInput > 0)
        {
            facingRight = true;
        }
        
        // Timers
        dashTimer += Time.deltaTime;
        noInputTimer -= Time.deltaTime;
        stickCooldownTimer -= Time.deltaTime;
        stickAttackTimer -= Time.deltaTime;
        rockAttackTimer -= Time.deltaTime;

        // Stick Check
        if (stickAttackTimer > 0)
        {
            var colliders = new List<Collider2D>();
            int boxes = Physics2D.OverlapBox(boxCollider.bounds.center + new Vector3(stickboxOffset.x * (facingRight? 1 : -1), stickboxOffset.y), stickboxSize, 0, new ContactFilter2D(), colliders);
            for (int i = 0; i < boxes; i++)
            {
                if (colliders[i].GetComponent<HittableBase>() is HittableBase hittable)
                {
                    hittable.Hit(HitType.Stick);
                }
            }
        }
        
        // Left/Right Movement
        if (canInput)
        {
            rb.AddForceX(horizMoveInput * acceleration * Time.deltaTime * rb.mass, ForceMode2D.Impulse);
            rb.linearVelocityX = Mathf.Clamp(rb.linearVelocityX, -moveSpeed, moveSpeed);

            rb.gravityScale = rb.linearVelocityY > 0 ? Mathf.Lerp(standardGravity * noHoldJumpModifier, standardGravity, jumpAxis) : standardGravity;
        }
    
        // Grounded Check
        grounded = Physics2D.OverlapBox((Vector2)boxCollider.bounds.center - new Vector2(0, boxCollider.bounds.size.y * 0.5f), new Vector2(boxCollider.bounds.size.x * 0.9f, 0.1f), 0, boxCollider.includeLayers);
        
        
        // Deceleration
        if (Mathf.Abs(horizMoveInput) < 0.01d)
        {
            rb.AddForceX(Mathf.Min(deceleration * Time.deltaTime, Mathf.Abs(rb.linearVelocityX)) * -Mathf.Sign(rb.linearVelocityX) * rb.mass, ForceMode2D.Impulse);
        }
        
        // Dash
        if (dashing)
        {
            rb.linearVelocityX = dashSpeed * (dashRight ? 1 : -1);
            canTakeDamage = false;

            List<Collider2D> colliders = new List<Collider2D>();
            Physics2D.OverlapBox(boxCollider.bounds.center,
                boxCollider.bounds.size + new Vector3(dashHitboxExtra, dashHitboxExtra),
                0, new ContactFilter2D(), colliders);

            foreach (Collider2D collider in colliders)
            {
                if (collider.GetComponent<HittableBase>() is HittableBase hittable)
                {
                    hittable.Hit(HitType.Dash);
                }
            }
        }

        // Visuals
        spriteRenderer.flipX = !facingRight;
        animator.SetFloat("MoveSpeed", rb.linearVelocityX);
        animator.SetBool("InAir", !grounded);
        
        
    }

    // flash
    private IEnumerator IFrameFlash()
    {
        float flashDuration = dashDuration + iFrameBuffer;
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

    private void FixedUpdate()
    {
        // Camera
        Vector3 goalPos = transform.position + new Vector3(cameraAheadAmount * (facingRight ? 1 : -1), cameraYAmount,-10);
        cameraTransform.position = MysticUtil.DampVector(
            cameraTransform.position, goalPos,
            cameraTweenAmount, Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canTakeDamage && other.CompareTag("Enemy"))
        {
            health -= 100;
            if (health <= 0) { /* death */ }

            // when die play: cameraVFX.StartScreenShake(.5f, 0.2f, cameraComp);
        }
        Debug.Log(health);
    }
    
        private IEnumerator SpawnAfterImages(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            GameObject ghost = Instantiate(afterImagePrefab, transform.position, transform.rotation);
            SpriteRenderer ghostSR = ghost.GetComponent<SpriteRenderer>();

            if (ghostSR != null)
            {
                // always use your chosen sprite
                if (afterImageSprite != null)
                    ghostSR.sprite = afterImageSprite;

                ghostSR.color = afterImageColor;
            }

            Destroy(ghost, afterImageLifetime);

            yield return new WaitForSeconds(afterImageSpawnInterval);
            elapsed += afterImageSpawnInterval;
        }
    }

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider2D>();
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((Vector2)boxCollider.bounds.center - new Vector2(0, boxCollider.bounds.size.y * 0.5f), new Vector2(boxCollider.bounds.size.x * 0.9f, 0.1f));

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + (Vector3)stickboxOffset, stickboxSize);
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(boxCollider.bounds.center,
            boxCollider.bounds.size + new Vector3(dashHitboxExtra, dashHitboxExtra));
    }
    #endif
}
