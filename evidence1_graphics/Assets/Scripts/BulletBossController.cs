using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBossController : MonoBehaviour
{
    public float xBoundary = 170f; // X-axis boundary
    public float zBoundary = 60f; // Z-axis boundary

    void Start()
    {

    }

    void Update()
    {
        CheckBoundaries();
    }

    void CheckBoundaries()
    {
        // Check if the bullet is outside the map boundaries
        if (transform.position.x > xBoundary || transform.position.x < -xBoundary ||
            transform.position.z > zBoundary || transform.position.z < -zBoundary)
        {
            BossController boss = GameObject.Find("Boss").GetComponent<BossController>();
            if (boss != null)
            {
                boss.bulletCounter--;
            }
            
            Destroy(gameObject); // Destroy the bullet if it's out of bounds
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SpaceShip"))
        {
            SpaceShipController ship = other.gameObject.GetComponent<SpaceShipController>();
            ship.takeDamage(10); 

            BossController boss = GameObject.Find("Boss").GetComponent<BossController>();
            boss.bulletCounter--;
            
            Destroy(gameObject);
        }
        else if (!(other.gameObject.CompareTag("Boss")) && !(other.gameObject.CompareTag("BulletBoss")))
        {
            Destroy(other.gameObject);
            Destroy(gameObject); // Destroy the bullet if it collides with anything else
        }
    }

}
