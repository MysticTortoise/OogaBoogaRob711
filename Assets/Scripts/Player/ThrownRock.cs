using System;
using System.Collections.Generic;
using UnityEngine;

public class ThrownRock : MonoBehaviour
{
    [SerializeField] private float gravityAmount;
    [SerializeField] private Vector2 bounds;
    [SerializeField] private ContactFilter2D mask;
    
    private Camera playerCamera;
    private SpriteRenderer spriteRenderer;

    private Vector2 velocity;
    private bool active;

    public bool IsActive()
    {
        return active;
    }
    
    private void Start()
    {
        playerCamera = FindAnyObjectByType<Camera>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Throw(Vector3 pos, Vector2 dir)
    {
        transform.position = pos;
        active = true;
        velocity = dir;
    }

    private void Update()
    {
        spriteRenderer.enabled = active;
        
        if (!active)
        {
            return;
        }

        velocity -= new Vector2(0, gravityAmount * Time.deltaTime);
        transform.position += (Vector3)velocity * Time.deltaTime;
        if (transform.position.y < -100)
        {
            active = false;
            return;
        }

        var colliders = new List<Collider2D>();
        int amount = Physics2D.OverlapBox(transform.position, bounds, 0, mask, colliders);
        if (amount > 0)
        {
            active = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, bounds);
    }
}
