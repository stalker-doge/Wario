using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject collisionAnimationObject;
    public Transform shootPoint;
    public ArrowController arrow;

    public float bulletSpeed = 10f;

    public int collisionCount = 0;
    public int collisionCountMax = 4;

    public int bulletLimit = 0;

    private void Start()
    {
        bulletLimit = 0;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        //    Vector2 dir = arrow.GetDirection();

        //    bullet.GetComponent<Rigidbody2D>().AddForce(dir * bulletSpeed, ForceMode2D.Impulse);
        //}
        if (Input.GetMouseButtonUp(0) && !TimerManager.Instance.winloseState)
        {
            if (bulletLimit < 3)
            {
                bulletLimit += 1;
                SoundManager.Instance.ShootAudioClip();

                if (shootPoint != null)
                {
                    GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
                    Vector2 dir = arrow.GetDirection();

                    if (bullet != null)
                        bullet.GetComponent<Rigidbody2D>().AddForce(dir * bulletSpeed, ForceMode2D.Impulse);
                }

            }
            else
            {
               // SoundManager.Instance.CardMismatchAudioClip();
            }
            //ShootBullet();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            GameObject effect = Instantiate(collisionAnimationObject, transform.position, Quaternion.identity);
            Destroy(effect, 0.3f);
        }


        if (other.gameObject.CompareTag("Goal") && !TimerManager.Instance.winloseState)
        {
            SoundManager.Instance.MiniGameCompleteAudioClip();
            if(ScoreManager.Instance)
            {
                StartCoroutine(ScoreManager.Instance.GameComplete());
            }
            other.gameObject.SetActive(false);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject, 0.3f);
        }
    }

    public void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        Vector2 dir = arrow.GetDirection();

        bullet.GetComponent<Rigidbody2D>().AddForce(dir * bulletSpeed, ForceMode2D.Impulse);
    }
}