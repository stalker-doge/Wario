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
        if(!TimerManager.Instance.winloseState)
            dragging = true;
    }

    void OnMouseDrag()
    {
        if (!TimerManager.Instance.winloseState)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos;   
        }
    }

    void OnMouseUp()
    {
        dragging = false;

        if (isOverPlaceholder)
        {
            if (!TimerManager.Instance.LosePage.activeSelf)
            {
                if (isCorrect)
                {
                    Debug.Log("HEYYYY");
                    // Snap to placeholder if correct
                    transform.position = placeholderTransform.position;
                    SoundManager.Instance?.CardMatchAudioClip();
                    StartCoroutine(ScoreManager.Instance?.GameComplete());
                }
                else
                {
                    // Wrong answer dropped on placeholder, return to original
                    transform.position = startPosition;
                    SoundManager.Instance?.CardMismatchAudioClip();
                    FlashBoundaryManager.OnFlashRequested?.Invoke();

                }
            }
            else
            {
                transform.position = startPosition;
                SoundManager.Instance?.CardMismatchAudioClip();
                FlashBoundaryManager.OnFlashRequested?.Invoke();
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