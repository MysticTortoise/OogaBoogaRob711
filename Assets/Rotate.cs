using System;
using UnityEngine;
public class Rotate : MonoBehaviour
{
    private Transform player;
    private Transform cop;
    private Animator animator;
    private float absoluteDistanceFromPlayer;
    public float threshold;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cop = GetComponent<Transform>();
        player = FindAnyObjectByType<Player>().transform;
        animator = GetComponent<Animator>();
        cop.rotation = Quaternion.Euler(0, 180, 0);
    }

    // Update is called once per frame
        void Update()
    {
        absoluteDistanceFromPlayer = Math.Abs(cop.position.x - player.position.x);
        if (absoluteDistanceFromPlayer <= threshold)
        {
            animator.SetBool("inShootingRange", true);
        }
        if (absoluteDistanceFromPlayer >= threshold)
        {
            animator.SetBool("inShootingRange", false);
        }

    }
}