using System;
using UnityEngine;

public class BreakableObject : HittableBase
{
    [SerializeField] private Vector2 hitForce;

    private float timer;

    private void Update()
    {
        timer -= Time.deltaTime;
    }

    public override void Hit(HitType type)
    {
        if (type == HitType.Dash || timer > 0)
        {
            return;
        }

        timer = .5f;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Collider2D collider = GetComponent<Collider2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.excludeLayers = new LayerMask();
        rb.AddForceAtPosition(
            new Vector2(
                    Mathf.Sign(FindAnyObjectByType<Player>().transform.position.x - transform.position.x) * -hitForce.x * rb.mass,
                    hitForce.y * rb.mass
                ),
            new Vector2(collider.bounds.center.x, collider.bounds.max.y),
            ForceMode2D.Impulse
            );
    }
}
