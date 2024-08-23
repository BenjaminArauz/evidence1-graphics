using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossController : MonoBehaviour
{
    public GameObject bulletPrefab;       
    public TextMeshProUGUI bulletCountTextMesh;
    public float bulletSpeed = 20f;       
    public float shootInterval = 10f;    
    public int numberBullets = 10;        
    public float spiralRadius = 2f;
    public int numberSpirals = 5;
    public int index = 0;
    public int bulletCounter = 0;

    public int maxHealth = 200;
    public int currentHealth;

    private float[] angles; 

    void Start()
    {
        currentHealth = maxHealth;
        
        angles = new float[numberSpirals];
        // Set different starting angles for each shot
        for (int i = 0; i < numberSpirals; i++)
        {
            angles[i] = i * (360f / numberSpirals);
        }

        // Start the main coroutine that handles shooting patterns
        StartCoroutine(HandleShootingPatterns());
    }

    void Update()
    {
        UpdateBulletCountText();
    }

    IEnumerator HandleShootingPatterns()
    {
        while (true)
        {
            float elapsedTime = 0;
            while (elapsedTime < 5f)
            {
                ShootBullets(index);
                elapsedTime += Time.deltaTime;
                yield return new WaitForSeconds(0.1f);
            }
            index++;
            if (index > 2)
            {
                index = 0;
            }
            yield return null; 
        }
    }

    void ShootBullets(int index)
    {        
        switch(index)
        {
            case 0:
                shootCircle();
                break;
            case 1:
                shootSpiral();
                break;
            case 2:
                shootChaotic();
                break;
        }
    }

    void shootChaotic()
    {
        float randomSpeed = bulletSpeed + Random.Range(-bulletSpeed, bulletSpeed);

        float angle = Random.Range(0f, 360f);
        
        float bulletDirX = transform.position.x + Mathf.Sin(angle * Mathf.Deg2Rad) * spiralRadius;
        float bulletDirZ = transform.position.z + Mathf.Cos(angle * Mathf.Deg2Rad) * spiralRadius;

        Vector3 bulletPosition = new Vector3(bulletDirX, transform.position.y, bulletDirZ);
        Vector3 bulletDir = (bulletPosition - transform.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletPosition, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = bulletDir * randomSpeed;

        bulletCounter++;
    }
    
    void shootSpiral()
    {
        for (int i = 0; i < numberSpirals; i++)
        {
            float bulletDirX = transform.position.x + Mathf.Sin(angles[i] * Mathf.Deg2Rad) * spiralRadius;
            float bulletDirZ = transform.position.z + Mathf.Cos(angles[i] * Mathf.Deg2Rad) * spiralRadius;

            Vector3 bulletPosition = new Vector3(bulletDirX, transform.position.y, bulletDirZ);
            Vector3 bulletDir = (bulletPosition - transform.position).normalized;
            
            GameObject bullet = Instantiate(bulletPrefab, bulletPosition, Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = bulletDir * bulletSpeed;
            
            bulletCounter++;

            angles[i] += 1f;
            if (angles[i] >= 360f)
            {
                angles[i] -= 360f;
            }
        }
    }
    
    
    void shootCircle()
    {
        float angleStep = 360f / numberBullets;
        float angle = 0f;

        for (int i = 0; i < numberBullets; i++)
        {
            float bulletDirX = transform.position.x + Mathf.Sin(angle * Mathf.Deg2Rad);
            float bulletDirZ = transform.position.z + Mathf.Cos(angle * Mathf.Deg2Rad);

            Vector3 bulletMoveVector = new Vector3(bulletDirX, transform.position.y, bulletDirZ);
            Vector3 bulletDir = (bulletMoveVector - transform.position).normalized;

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(bulletDir.x, 0, bulletDir.z) * bulletSpeed;

            angle += angleStep;
            
            bulletCounter++;
        }
        angle = 0f;
    }


    void UpdateBulletCountText()
    {
        if (bulletCountTextMesh != null)
        {
            bulletCountTextMesh.text = "Contador: " + bulletCounter;
        }
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Boss health: " + currentHealth);
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
