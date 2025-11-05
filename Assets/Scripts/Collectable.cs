using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void YouWin()
    {
        FindAnyObjectByType<Fade>().DoFade(Color.white, true, 3, "Level Select");
        LevelSelectIntroAnim.WasInLevel = true;

        int thisLevel = int.Parse(SceneManager.GetActiveScene().name.Substring(5, 1));
        if (SaveData.levelsBeaten < thisLevel)
        {
            SaveData.levelsBeaten = thisLevel;
            SaveData.SaveGame();
        }
    }
}
