using System;
using UnityEngine;

public class MazeDragPlayer : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 targetPosition;
    private bool isDragging = false;
    private bool isTouchingWall = false;

    public float moveSpeed = 10f;
    public float rotationSpeed = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        Vector3 newPosition = Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        Vector3 direction = newPosition - rb.position;

        // Rotate only if not touching wall
        if (direction.magnitude > 0.01f && !isTouchingWall)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        rb.MovePosition(newPosition);

       // rb.velocity = Vector3.ClampMagnitude(rb.velocity, moveSpeed);
    }

    // Detect collision with walls
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = true;
        }
        if (collision.gameObject.CompareTag("MazeWall"))
        {
            moveSpeed = 3;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("MazeWall"))
        {
            moveSpeed = 8;
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("MazeGoal"))
        {
            Debug.Log("Maze Done");
            // You can also add effects, sounds, or next level logic here
            //SoundManager.Instance.MiniGameCompleteAudioClip();
            ScoreManager.Instance?.GameComplete();
        }
    
    }
}