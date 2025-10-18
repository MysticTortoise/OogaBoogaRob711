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
    private float timer;
    public float speed;

    void Start()
    {
        projectile = GetComponent<Transform>();
        enemy = FindAnyObjectByType<enemyMovement>().transform;
        player = FindAnyObjectByType<Player>().transform;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer <= 3)
        {
            projectile.localPosition = new Vector3(0, 0, 0);
        }

        if (timer > 3)
        {
            if (timer < 3.1)
            {
                if (player.position.x > enemy.position.x)
                {
                    speed = -Math.Abs(speed);
                }
                if (player.position.x < enemy.position.x)
                {
                    speed = Math.Abs(speed);
                }
            }
            rb.linearVelocityX = speed;

        }
        if (timer >= 5)
        {
            timer = 0;
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            timer = 0;
            Debug.Log("Collided");
        }
    }
}
