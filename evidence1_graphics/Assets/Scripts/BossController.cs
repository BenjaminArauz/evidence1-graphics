using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BossController : MonoBehaviour
{
    // Bullet properties
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;       

    // UI properties       
    public TextMeshProUGUI bulletCountTextMesh;
    public int bulletCounter = 0;

    // Shoot in circle properties
    public float shootInterval = 10f;    
    public int numberBullets = 10;

    // Shoot in spiral properties     
    public float spiralRadius = 2f;
    public int numberSpirals = 10;
    
    // Boss properties
    public int index = 1;
    public int maxHealth = 200;
    public int currentHealth;

    private float[] angles; 

    void Start()
    {
        currentHealth = maxHealth;
        
        angles = new float[numberSpirals];
        for (int i = 0; i < numberSpirals; i++)
        {
            angles[i] = i * (360f / numberSpirals);
        }

        // Start the main coroutine that handles shooting patterns
        StartCoroutine(HandleShootingPatterns());
        //handle();
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
        float [] anglesCopy = angles;

        for (int i = 0; i < numberSpirals; i++)
        {
            float bulletDirX = Mathf.Sin(anglesCopy[i] * Mathf.Deg2Rad);
            float bulletDirZ = Mathf.Cos(anglesCopy[i] * Mathf.Deg2Rad);

            Vector3 bulletDirection = new Vector3(bulletDirX, 0, bulletDirZ).normalized;
            Vector3 bulletPosition = transform.position + new Vector3(0f, 1f, 0f);
            GameObject bullet = Instantiate(bulletPrefab, bulletPosition, Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = bulletDirection * bulletSpeed;

            bulletCounter++;
            
            anglesCopy[i] += 1f;
            if (anglesCopy[i] >= 360f)
            {
                anglesCopy[i] -= 360f;
            }
        }
    }
    
    
    void shootCircle()
    {
        float angleStep = 360f / numberBullets; // Divide 360 grados por el número de balas
        float angle = 0f; // Ángulo inicial

        for (int i = 0; i < numberBullets; i++)
        {
            float bulletDirX = Mathf.Sin(angle * Mathf.Deg2Rad);
            float bulletDirZ = Mathf.Cos(angle * Mathf.Deg2Rad);

            Vector3 bulletDirection = new Vector3(bulletDirX, 0, bulletDirZ).normalized;
            GameObject bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = bulletDirection * bulletSpeed;

            bulletCounter++;
            angle += angleStep;
        }
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
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene(2);
        }
    }
}
