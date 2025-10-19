using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : HittableBase
{
    public float startMovingThreshold;
    public float stopMovingThreshold;
    public float velocity;
    private Transform player;
    private Transform enemy;
    private Rigidbody2D rb;
    private float distanceFromPlayer;
    private float absoluteDistanceFromPlayer;
    public float invincibilityTime;
    private bool cooldown = false;

    [Header("Animator")]
        public Animator anim;

        [Header("Hit Reaction")]
            public float knockbackForce = 8f;
            public float knockbackUp = 2f;
            public float flashTime = 0.2f;
            public GameObject deathParticle;
            private SpriteRenderer sr;
            private Color baseColor;
            private bool stunned;
            [SerializeField] private float flashInterval;


    [SerializeField] private int health = 300;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Transform>();
        player = FindAnyObjectByType<Player>().transform;
        sr = GetComponent<SpriteRenderer>();
    if (sr == null)
    {
        var child = transform.Find("Sprite");
        if (child != null)
            sr = child.GetComponent<SpriteRenderer>();
    }
if (sr != null)
    baseColor = sr.color;

    }
    
    // Update is called once per frame
    void Update()
    {
        if (stunned) return;

        distanceFromPlayer = enemy.position.x - player.position.x;
        absoluteDistanceFromPlayer = Math.Abs(distanceFromPlayer);
        if (absoluteDistanceFromPlayer < startMovingThreshold)
        {
            if (distanceFromPlayer > 0)
            {
                enemy.localScale = math.abs(enemy.localScale);
            }
            if (distanceFromPlayer < 0)
            {
                 enemy.localScale = new Vector3(-Math.Abs(enemy.localScale.x), enemy.localScale.y, enemy.localScale.z);
            }
            if (absoluteDistanceFromPlayer < stopMovingThreshold)
            {
                rb.linearVelocityX = 0;
                anim.SetBool("move", false);
                
            }
            else if (distanceFromPlayer > 0)
            {
                rb.linearVelocityX = -velocity;
                anim.SetBool("move", true);
            }
            else if (distanceFromPlayer < 0)
            {
                rb.linearVelocityX = velocity;
                anim.SetBool("move", true);
            }
        }
    }

    public override void Hit(HitType type)
    {
        if (!cooldown)
        {
            if (type == HitType.Dash)
            {
                return;
            }

            health -= (int)type;

            if (health <= 0)
            {
                Instantiate(deathParticle, transform.position, Quaternion.identity);

                Destroy(gameObject);
            } else 
            {
                StartCoroutine(HitFeedback());
            }
        }
    }

     private IEnumerator HitFeedback()
    {
        stunned = true;
        cooldown = true;

        // Direction away from player
        float dir = 1f;
        if (player != null)
            dir = Mathf.Sign(transform.position.x - player.position.x);
        if (dir == 0) dir = 1f;

        // Apply knockback
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(dir * knockbackForce, knockbackUp), ForceMode2D.Impulse);

        // Flash red
        if (sr != null)
            sr.color = Color.red;

        yield return new WaitForSeconds(flashTime);

        stunned = false;

        float elapsed = 0;
        while (elapsed < invincibilityTime - flashTime)
        {
            sr.color = baseColor;
            yield return new WaitForSeconds(flashInterval);
            sr.color = Color.red;
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval * 2f;
        }
        sr.color = baseColor;
        cooldown = false;
    }
     
    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(startMovingThreshold * 2, 900));
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(stopMovingThreshold * 2, 900));
    }
    
}
