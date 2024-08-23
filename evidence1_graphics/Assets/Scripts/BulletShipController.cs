using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShipController : MonoBehaviour
{
    public float xBoundary = 150f; // X-axis boundary
    public float zBoundary = 60f; // Z-axis boundary

    void Start()
    {

    }

    void Update()
    {
        checkBoundaries();
    }

    void checkBoundaries()
    {
        // Check if the bullet is outside the map boundaries
        if (transform.position.x > xBoundary || transform.position.x < -xBoundary ||
            transform.position.z > zBoundary || transform.position.z < -zBoundary)
        {
            Destroy(gameObject); // Destroy the bullet if it's out of bounds
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boss"))
        {
            BossController boss = other.gameObject.GetComponent<BossController>();
            
            boss.takeDamage(20); 
            Destroy(gameObject);
        }
        else if (!(other.gameObject.CompareTag("SpaceShip")))
        {
            Destroy(gameObject); // Destroy the bullet if it collides with anything else
        }
    }
}
