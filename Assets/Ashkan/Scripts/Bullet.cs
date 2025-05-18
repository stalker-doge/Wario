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
    public int collisionCountMax = 4;
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
            SoundManager.Instance.ShootAudioClip();

            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            Vector2 dir = arrow.GetDirection();

            bullet.GetComponent<Rigidbody2D>().AddForce(dir * bulletSpeed, ForceMode2D.Impulse);
            //ShootBullet();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        SoundManager.Instance.ProjectileBounceAudioClip();
        collisionCount++;
        if (collisionCount == collisionCountMax)
        {
            gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("Goal"))
        {
            other.gameObject.SetActive(false);
            gameObject.SetActive(false);
            //calls the game complete method from the score manager
            ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager != null)
            {
                scoreManager.GameComplete();
            }
            else
            {
                Debug.LogError("ScoreManager not found in the scene.");
            }
        }
    }

    public void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        Vector2 dir = arrow.GetDirection();

        bullet.GetComponent<Rigidbody2D>().AddForce(dir * bulletSpeed, ForceMode2D.Impulse);
    }
}