using System;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    public float startMovingThreshold;
    public float stopMovingThreshold;
    public float velocity;
    private Transform player;
    private Transform enemy;
    private Rigidbody2D rb;
    private float distanceFromPlayer;
    private float absoluteDistanceFromPlayer;

    
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Transform>();
        player = FindAnyObjectByType<Player>().transform;
    }
    // Update is called once per frame
    void Update()
    {
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
}
