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
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        //    Vector2 dir = arrow.GetDirection();

        //    bullet.GetComponent<Rigidbody2D>().AddForce(dir * bulletSpeed, ForceMode2D.Impulse);
        //}
        if (Input.GetMouseButtonUp(0))
        {
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            Vector2 dir = arrow.GetDirection();

            bullet.GetComponent<Rigidbody2D>().AddForce(dir * bulletSpeed, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        collisionCount++;
        if ( collisionCount == collisionCountMax)
        {
            gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("Goal"))
        {
            other.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}