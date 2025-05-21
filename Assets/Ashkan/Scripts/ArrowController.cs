using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public int stepAngle = 10;          // step size in degrees
    public float swipeThreshold = 10f;  // how much to drag to trigger one step
    public int minAngle = 0;            // limit (left)
    public int maxAngle = 180;          // limit (right)

    private Vector2 lastPosition;
    private bool isDragging = false;
    private float accumulatedDelta = 0f; // to accumulate drag distance

    void Start()
    {
        // Make sure arrow starts at center (90 degrees)
        transform.rotation = Quaternion.Euler(0, 0, 90);
    }

    void Update()
    {
        
        if(TimerManager.Instance.winloseState)
            return;
        
        // 🖱 Mouse Input
        if (Input.GetMouseButtonDown(0))
        {
            lastPosition = Input.mousePosition;
            isDragging = true;
            accumulatedDelta = 0f;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging && Input.touchCount == 0)
        {
            Vector2 currentPosition = Input.mousePosition;
            float deltaX = currentPosition.x - lastPosition.x;
            HandleStepRotation(deltaX);
            lastPosition = currentPosition;
        }

        // 📱 Touch Input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastPosition = touch.position;
                accumulatedDelta = 0f;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                float deltaX = touch.position.x - lastPosition.x;
                HandleStepRotation(deltaX);
                lastPosition = touch.position;
            }
        }
    }

    void HandleStepRotation(float deltaX)
    {
        accumulatedDelta += deltaX;

        if (Mathf.Abs(accumulatedDelta) >= swipeThreshold)
        {
            int stepDirection = accumulatedDelta > 0 ? -1 : 1; // right swipe = rotate left
            float currentAngle = transform.eulerAngles.z;
            float newAngle = currentAngle + stepAngle * stepDirection;

            // Clamp to range between 0 and 180
            if (newAngle < 0) newAngle += 360; // normalize angle
            if (newAngle >= 360) newAngle -= 360;

            if (newAngle >= minAngle && newAngle <= maxAngle)
            {
                transform.rotation = Quaternion.Euler(0, 0, newAngle);
            }

            accumulatedDelta = 0f; // reset after each step
        }
    }

    public Vector2 GetDirection()
    {
        float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }
}
