using System;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private Animator animator;
    private bool collected;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(collected){return;}
        Debug.Log(other);
        
        if (other.GetComponent<Player>())
        {
            animator.SetTrigger("Got");
            collected = true;
        }
    }
}
