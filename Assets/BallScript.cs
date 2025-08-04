using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallScript : MonoBehaviour
{
    private Camera mainCam;
    private bool hasLeftScreen = false;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (!hasLeftScreen && !IsVisibleFrom(mainCam))
        {
            hasLeftScreen = true;
            StartCoroutine(RespawnAfterExit());
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Goal"))
        {
            if (!TimerManager.Instance.LosePage.activeSelf)
            {
                SoundManager.Instance?.CardMatchAudioClip();
                StartCoroutine(ScoreManager.Instance?.GameComplete());

                if (SceneManager.GetActiveScene().name ==  "GolfGame")
                {
                    SoundManager.Instance.GolfGoalAudioClip();
                }

                if (SceneManager.GetActiveScene().name ==  "BasketballGame")
                {
                    mainCam.GetComponent<CameraShake>().switchh = true;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") && DragController.Instance.shoot)
        {
            StartCoroutine(RespawnWithDelay());
            DragController.Instance.shoot = false;
            
            if (SceneManager.GetActiveScene().name ==  "BasketballGame")
            {
                SoundManager.Instance.BasketballBounceAudioClip();
            }
        }
    }

    // Check if object is visible by camera
    bool IsVisibleFrom(Camera cam)
    {
        Vector3 viewPos = cam.WorldToViewportPoint(transform.position);
        return viewPos.x >= 0 && viewPos.x <= 1 &&
               viewPos.y >= 0 && viewPos.y <= 1 &&
               viewPos.z >= 0;
    }

    IEnumerator RespawnAfterExit()
    {
        GetComponent<TrailRenderer>().enabled = false;
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<Rigidbody2D>().Sleep();
        gameObject.transform.position = DragController.Instance.spawnPoint.position;
        hasLeftScreen = false;
        GetComponent<TrailRenderer>().enabled = true;

    }

    IEnumerator RespawnWithDelay()
    {
        GetComponent<TrailRenderer>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<Rigidbody2D>().Sleep();
        gameObject.transform.position = DragController.Instance.spawnPoint.position;
        hasLeftScreen = false;
        GetComponent<TrailRenderer>().enabled = true;

    }

}