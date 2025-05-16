using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfBall : MonoBehaviour
{


    [SerializeField]
    private GameObject golfBallPrefab;
    [SerializeField]
    private Transform shootPoint;

    [SerializeField]
    private ArrowController arrow;
    public float bulletSpeed = 10f;

    public int collisionCount = 0;
    public int collisionCountMax = 2;
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            //SoundManager.Instance.ShootAudioClip();

            GameObject bullet = Instantiate(golfBallPrefab, shootPoint.position, Quaternion.identity);
            Vector2 dir = arrow.GetDirection();

            bullet.GetComponent<Rigidbody2D>().velocity = dir * bulletSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //SoundManager.Instance.ProjectileBounceAudioClip();
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
                StartCoroutine(scoreManager.GameComplete());
            }
            else
            {
                Debug.LogError("ScoreManager not found in the scene.");
            }
        }
    }
}
