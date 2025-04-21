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
        DetectSwipeInput();

        // Optional: clamp X position if needed
        float clampedX = Mathf.Clamp(transform.position.x, -maxX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
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
            Debug.Log("Game Over");
        }
    }
}