using UnityEngine;

public class BallController : MonoBehaviour
{
    public float forceMultiplier = 10f;
    public float maxX = 1.5f;

    private Vector2 startTouchPos;
    private Vector2 endTouchPos;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(!TimerManager.Instance.winloseState)
            DetectSwipeInput();

    }

    void DetectSwipeInput()
    {
        // Touch input (mobile)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                startTouchPos = touch.position;

            if (touch.phase == TouchPhase.Ended)
            {
                endTouchPos = touch.position;
                ApplySwipeForce(endTouchPos - startTouchPos);
            }
        }

        // Mouse input (for editor)
        if (Input.GetMouseButtonDown(0))
            startTouchPos = Input.mousePosition;

        if (Input.GetMouseButtonUp(0))
        {
            endTouchPos = Input.mousePosition;
            ApplySwipeForce(endTouchPos - startTouchPos);
        }
    }

    void ApplySwipeForce(Vector2 swipe)
    {
        if (swipe.magnitude > 50f) // prevent weak swipes
        {
            swipe.Normalize();
            Vector2 force = swipe * forceMultiplier;
            rb.AddForce(force, ForceMode2D.Impulse); // Add impulse force
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            //SoundManager.Instance.MiniGameCompleteAudioClip();
            //ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
            //if (scoreManager != null)
            //{
            //    StartCoroutine(scoreManager.GameComplete());
            //}

            StartCoroutine(ScoreManager.Instance?.GameComplete());
        }
    }
}