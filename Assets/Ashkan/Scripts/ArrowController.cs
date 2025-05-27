using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float moveSpeed = 100f;       // Speed of rotation in degrees per second
    public float minAngle = 0f;           // Minimum allowed angle (left limit)
    public float maxAngle = 180f;         // Maximum allowed angle (right limit)

    public GameObject threeRemaining;    // UI for 3 remaining
    public GameObject twoRemaining;      // UI for 2 remaining
    public GameObject oneRemaining;      // UI for 1 remaining

    private Vector2 lastPosition;
    private bool isDragging = false;
    [SerializeField] private int shootCount = 0; // Counts how many times player has shot

    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, 90);

        // Make sure all indicators are active at the beginning
        threeRemaining?.SetActive(true);
        twoRemaining?.SetActive(true);
        oneRemaining?.SetActive(true);
    }

    void Update()
    {
        if (TimerManager.Instance.winloseState)
            return;

        // 🖱 Mouse Input
        if (Input.GetMouseButtonDown(0))
        {
            lastPosition = Input.mousePosition;
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            HandleShot();
        }

        if (isDragging && Input.touchCount == 0)
        {
            Vector2 currentPosition = Input.mousePosition;
            float deltaX = currentPosition.x - lastPosition.x;
            RotateArrow(deltaX);
            lastPosition = currentPosition;
        }

        // 📱 Touch Input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                float deltaX = touch.position.x - lastPosition.x;
                RotateArrow(deltaX);
                lastPosition = touch.position;
            }
        }
    }

    void RotateArrow(float deltaX)
    {
        float rotationAmount = -deltaX * moveSpeed * Time.deltaTime;
        float currentAngle = transform.eulerAngles.z;

        // Normalize current angle to range [0, 360)
        currentAngle = (currentAngle + 360f) % 360f;

        float newAngle = currentAngle + rotationAmount;

        // Clamp new angle to [minAngle, maxAngle]
        newAngle = Mathf.Clamp(newAngle, minAngle, maxAngle);

        transform.rotation = Quaternion.Euler(0, 0, newAngle);
    }

    public Vector2 GetDirection()
    {
        float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }

    void HandleShot()
    {
        shootCount++;
        if (shootCount == 1 && threeRemaining != null)
            threeRemaining.SetActive(false);
        else if (shootCount == 2 && twoRemaining != null)
            twoRemaining.SetActive(false);
        else if (shootCount == 3 && oneRemaining != null)
            oneRemaining.SetActive(false);
    }
}
