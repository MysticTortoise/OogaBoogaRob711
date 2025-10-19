using System;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class shootProjectile : MonoBehaviour
{
    private Transform projectile;
    private Transform enemy;
    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer render;
    private BoxCollider2D col;
    private float timer;
    public float speed;
    private Boolean runUpdate = false;

    public void Initialize(Transform enemyTransform)
    {
        Debug.Log("Script is running");
        enemy = enemyTransform;
        projectile = GetComponent<Transform>();
        player = FindAnyObjectByType<Player>().transform;
        rb = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        runUpdate = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (runUpdate && enemy != null)
        {
            timer += Time.deltaTime;
            if (timer <= 3)
            {
                projectile.position = new Vector3 (enemy.position.x, enemy.position.y + 2.871128f, enemy.position.z);
                rb.linearVelocityX = 0;
                render.enabled = false;
                col.enabled = false;
            }

            if (timer > 3)
            {
                render.enabled = true;
                col.enabled = true;
                if (timer < 3.05)
                {
                    if (player.position.x > enemy.position.x)
                    {
                        speed = Math.Abs(speed);
                    }
                    if (player.position.x < enemy.position.x)
                    {
                        speed = -Math.Abs(speed);
                    }
                }
                rb.linearVelocityX = speed;

            }
            if (timer >= 5)
            {
                timer = 0;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            timer = 0;
        }
    }
}
