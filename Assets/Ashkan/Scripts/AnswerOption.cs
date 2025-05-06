using UnityEngine;

public class AnswerOption : MonoBehaviour
{
    public int value;
    public bool isCorrect;

    private Vector3 startPosition;
    private bool dragging = false;
    private bool isOverPlaceholder = false;
    private Transform placeholderTransform;

    void Start()
    {
        startPosition = transform.position;
    }

    void OnMouseDown()
    {
        dragging = true;
    }

    void OnMouseDrag()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;
    }

    void OnMouseUp()
    {
        dragging = false;

        if (isOverPlaceholder)
        {
            if (isCorrect)
            {
                // Snap to placeholder if correct
                transform.position = placeholderTransform.position;
                ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
                if (scoreManager != null)
                {
                    scoreManager.GameComplete();
                }
                // Optional: disable further dragging if needed
            }
            else
            {
                // Wrong answer dropped on placeholder, return to original
                transform.position = startPosition;
            }
        }
        else
        {
            // Released somewhere else, return to original position
            transform.position = startPosition;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("AnswerPlaceholder"))
        {
            isOverPlaceholder = true;
            placeholderTransform = other.transform;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("AnswerPlaceholder"))
        {
            isOverPlaceholder = false;
            placeholderTransform = null;
        }
    }
}