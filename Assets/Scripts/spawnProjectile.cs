using Unity.Properties;
using UnityEngine;

public class spawnProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    private Transform spawner;
    private GameObject projectile;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawner = GetComponent<Transform>();
        projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        shootProjectile spawnedProjectile = projectile.GetComponent<shootProjectile>();
        spawnedProjectile.Initialize(spawner);
    }

    
}
