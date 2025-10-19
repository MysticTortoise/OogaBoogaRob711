using System;
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
    private float timer;
    private bool cooldown = false;
    private float currentTime;

        [Header("Hit Reaction")]
            public float knockbackForce = 8f;
            public float knockbackUp = 2f;
            public float flashTime = 0.2f;
            public GameObject deathParticle;
            private SpriteRenderer sr;
            private Color baseColor;
            private bool stunned;


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
        
        timer += Time.deltaTime;
        if (cooldown)
        {
            if (currentTime + 1 <= timer)
            {
                cooldown = false;
            }
        }
        distanceFromPlayer = enemy.position.x - player.position.x;
        absoluteDistanceFromPlayer = Math.Abs(distanceFromPlayer);
        if (absoluteDistanceFromPlayer < startMovingThreshold)
        {
            if (absoluteDistanceFromPlayer < stopMovingThreshold)
            {
                rb.linearVelocityX = 0;
                
            }
            else if (distanceFromPlayer > 0)
            {
                rb.linearVelocityX = -velocity;
                enemy.localScale = math.abs(enemy.localScale);
            }
            else if (distanceFromPlayer < 0)
            {
                rb.linearVelocityX = velocity;
                enemy.localScale = new Vector3(-Math.Abs(enemy.localScale.x), enemy.localScale.y, enemy.localScale.z);
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
            cooldown = true;
            currentTime = timer;
        }
    }

     private System.Collections.IEnumerator HitFeedback()
    {
        stunned = true;

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

        if (sr != null)
            sr.color = baseColor;

        stunned = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(startMovingThreshold * 2, 900));
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(stopMovingThreshold * 2, 900));
    }
    
}
