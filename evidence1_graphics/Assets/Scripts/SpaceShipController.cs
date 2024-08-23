using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipController : MonoBehaviour
{
    public float speed = 40.0f;
    public float turnSpeed = 50.0f;
    public float horizontalInput;
    public float forwardInput;
    public KeyCode shootKey;

    public int maxHealth = 100;
    public int currentHealth;

    public GameObject bulletPrefab; // Assign the bullet prefab in the Inspector
    public float bulletSpeed = 20f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
        transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);

        if (Input.GetKeyDown(shootKey))
        {
            shoot();
        }
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Ship health: " + currentHealth);
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void shoot()
    {
        Debug.Log("Shoot function called");
        Vector3 spawnPosition = transform.position + transform.forward * 2f; // Bullet spawns slightly ahead of the ship
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, transform.rotation);
        
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * bulletSpeed;
    }
}
