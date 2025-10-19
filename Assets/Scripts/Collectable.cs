using System;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private Animator animator;
    private bool collected = false;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(collected){return;}
        
        if (other.GetComponent<Player>() is Player player)
        {
            animator.SetTrigger("Got");
            collected = true;
            player.DoWin(this);
        }
    }
}
