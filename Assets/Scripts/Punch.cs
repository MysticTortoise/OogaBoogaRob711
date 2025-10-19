using System;
using UnityEngine;

public class Punch : MonoBehaviour
{
    private Transform player;
    private Transform civilian;
    private Animator animator;
    private float absoluteDistanceFromPlayer;
    public float threshold;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        civilian = GetComponent<Transform>();
        player = FindAnyObjectByType<Player>().transform;
        animator = GetComponent<Animator>();
        transform.rotation *= Quaternion.Euler(0, 180, 0);
    }

    // Update is called once per frame
    void Update()
    {

        absoluteDistanceFromPlayer = Math.Abs(civilian.position.x - player.position.x);
        if (absoluteDistanceFromPlayer <= threshold)
        {
            animator.SetBool("inPunchingDistance", true);
        }
        if (absoluteDistanceFromPlayer >= threshold)
        {
            animator.SetBool("inPunchingDistance", false);
        }
        
    }
}
