using UnityEngine;

public class MazeDragPlayer : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 targetPosition;
    private bool isDragging = false;
    private bool isTouchingWall = false;

    public float moveSpeed = 10f;
    public float rotationSpeed = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // this blocks Unity's physics from rotating the object
        targetPosition = transform.position;
    }

    void OnMouseDown()
    {
        isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            targetPosition = mousePos;
        }
    }

    void FixedUpdate()
    {
        Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        Vector2 direction = newPosition - rb.position;

        // Rotate only if not touching wall
        if (direction.magnitude > 0.01f && !isTouchingWall)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        rb.MovePosition(newPosition);
    }

    // Detect collision with walls
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = false;
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MazeGoal"))
        {
            Debug.Log("Maze Done");
            // You can also add effects, sounds, or next level logic here
        }
    }
}