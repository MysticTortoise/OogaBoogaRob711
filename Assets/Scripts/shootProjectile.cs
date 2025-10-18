using System;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class shootProjectile : MonoBehaviour
{
    private Transform bullet;
    private Transform enemy;
    private Transform player;
    private float timer;
    public float speed;

    void Start()
    {
        bullet = GetComponent<Transform>();
        enemy = FindAnyObjectByType<enemyMovement>().transform;
        player = FindAnyObjectByType<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer <= 3)
        {
            bullet.localPosition = new Vector3(0, 0, 0);
        }

        if (timer > 3)
        {
            if (timer < 3.1)
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
            bullet.position = new Vector3(bullet.position.x + speed, bullet.position.y, bullet.position.z);

        }
        if (timer >= 5)
        {
            timer = 0;
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Collided");
        timer = 0;
        
    }
}
