using System;
using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private Transform sourceObj;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() is Player player)
        {
            player.TakeDamage(damage, sourceObj.position);
        }
    }
}
