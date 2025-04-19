using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public ArrowController arrow;

    public float bulletSpeed = 10f;

    public int collisionCount = 0;
    public int collisionCountMax = 2;
    
    public int maxShots = 2;
    private int remainingShots;
    
    
    void Start()
    {
        remainingShots = maxShots;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.CompareTag("Goal"))
        {
            other.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
    
    public void Shoot()
    {
        if (remainingShots > 0)
        {
            // Do the shooting logic here (e.g., instantiate projectile)
            Debug.Log("Shoot!");
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            Vector2 dir = arrow.GetDirection();

            bullet.GetComponent<Rigidbody2D>().AddForce(dir * bulletSpeed, ForceMode2D.Impulse);
            remainingShots--;
        }
        else
        {
            Debug.Log("No shots left!");
        }
    }
}