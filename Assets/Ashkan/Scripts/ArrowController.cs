using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float rotationSpeed = 0.3f;
    private Vector2 lastPosition;
    private bool isDragging = false;

    void Update()
    {
        // 🖱 Mouse Controls (PC)
        if (Input.GetMouseButtonDown(0))
        {
            lastPosition = Input.mousePosition;
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging && Input.touchCount == 0)
        {
            Vector2 currentPosition = Input.mousePosition;
            float deltaX = currentPosition.x - lastPosition.x;
            transform.Rotate(0, 0, -deltaX * rotationSpeed);
            lastPosition = currentPosition;
        }

        // 📱 Touch Controls (Mobile)
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
                transform.Rotate(0, 0, -deltaX * rotationSpeed);
                lastPosition = touch.position;
            }
        }
    }

    public Vector2 GetDirection()
    {
        float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }
}
