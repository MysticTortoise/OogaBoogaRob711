using System;
using System.Collections.Generic;
using UnityEngine;

public class President : BreakableObject
{
    [SerializeField] private int hits;
    [SerializeField] private GameObject explodePrefab;
    [SerializeField] private ContactFilter2D contactFilter;

    private Rigidbody2D rb;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Hit(HitType type)
    {
        if (timer > 0)
        {
            return;
        }
        base.Hit(type);
        
        hits--;
        Debug.Log(hits);

        if (hits <= 0)
        {
            Destroy(gameObject);
            GameObject explorde = Instantiate(explodePrefab, transform.position, Quaternion.identity);
        }
    }

    private void Update()
    {
        base.Update();
        if (Math.Abs(transform.position.y - startPos.y) > 50)
        {
            transform.position = startPos;
        }

        if (Physics2D.OverlapPoint(transform.position, contactFilter, new List<Collider2D>()) > 0 || rb.IsSleeping())
        {
            transform.position = startPos;
        }
    }
}
