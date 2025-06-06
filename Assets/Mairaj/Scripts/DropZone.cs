//Mairaj Muhammad ->2415831
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public ShapeType acceptedShapeType; // Assign in Inspector
    private static int correctMatches = 0;  // Counter to track correct matches

    public delegate void GameEnded(); // Delegate for notifying game end
    public static event GameEnded OnGameEnded; // Event to notify game manager

    private FillTheGapVariant variant;

    private NewFillTheGapVariant newVariant;

    [SerializeField]
    private NewAcceptedShapeType newAcceptedShapeType;

    private void Awake()
    {
        correctMatches = 0; // Initialize the correct matches counter
        Invoke("InitializeVariantAfterDelay", 0.3f);
    }

    public NewAcceptedShapeType GetNewAcceptedShapeType()
    {
        return newAcceptedShapeType;
    }

    private void InitializeVariantAfterDelay()
    {
        newVariant = FillTheGapManager.Instance.GetNewVariant();
        if (newVariant == NewFillTheGapVariant.mXZeroSlots)
        {
            variant = FillTheGapManager.Instance.GetVariant();
        }
    }
    

    public void OnDrop(PointerEventData eventData)
    {
        DragDrop dragDrop = eventData.pointerDrag.GetComponent<DragDrop>();

        if (dragDrop != null)
        {
            // Debug.Log("XYZ Found Shape Overlap " + dragDrop.shapeType + " " + acceptedShapeType);

            if (newVariant == NewFillTheGapVariant.mXZeroSlots)
            {
                if (dragDrop.shapeType == acceptedShapeType)
                {
                    // Correct Shape - Update position and parent

                    // Update the parent of the DragDrop object to be the same as the parent of the DropZone
                    dragDrop.transform.SetParent(transform.parent); // Set the parent to the parent of this DropZone

                    // Update position to match the DropZone
                    dragDrop.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

                    // Optionally, adjust any other properties of the DragDrop after the drop (e.g., lock its position)
                    dragDrop.GetComponent<DragDrop>().enabled = false; // Disable dragging after successful drop (if required)

                    // Increment correct match counter
                    correctMatches++;
                    // Check if all matches have been made
                    if (correctMatches >= (int)variant + 1)
                    {
                        // Correct Shape - Update position and parent

                        // Update the parent of the DragDrop object to be the same as the parent of the DropZone
                        dragDrop.transform.SetParent(transform.parent); // Set the parent to the parent of this DropZone

                        // Update position to match the DropZone
                        dragDrop.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

                        // Optionally, adjust any other properties of the DragDrop after the drop (e.g., lock its position)
                        dragDrop.GetComponent<DragDrop>().enabled = false; // Disable dragging after successful drop (if required)

                        // Increment correct match counter
                        correctMatches++;

                        // Check if all matches have been made
                        if (correctMatches >= (int)variant)
                        {
                            correctMatches = 0; // Reset the counter for the next game
                            // Game has ended, notify FindTheGapManager
                            OnGameEnded?.Invoke();
                            if (ScoreManager.Instance)
                            {
                                if (!TimerManager.Instance.LosePage.activeSelf)
                                {
                                    StartCoroutine(ScoreManager.Instance.GameComplete());
                                }
                            }
                            SoundManager.Instance?.CardMatchAudioClip();
                        }
                    }
                }
                else
                {
                    // Wrong Shape - Reset to original position
                    dragDrop.ResetPosition();
                    FlashBoundaryManager.OnFlashRequested?.Invoke();
                }
            } else
            {
                if (dragDrop.newAcceptedShapeType == newAcceptedShapeType)
                {
                    // Correct Shape - Update position and parent

                    // Update the parent of the DragDrop object to be the same as the parent of the DropZone
                    dragDrop.transform.SetParent(transform.parent); // Set the parent to the parent of this DropZone

                    // Update position to match the DropZone
                    dragDrop.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

                    // Optionally, adjust any other properties of the DragDrop after the drop (e.g., lock its position)
                    dragDrop.GetComponent<DragDrop>().enabled = false; // Disable dragging after successful drop (if required)

                    // Increment correct match counter
                    correctMatches++;

                    // Check if all matches have been made
                    if (correctMatches >= (int)newVariant)
                    {
                        correctMatches = 0; // Reset the counter for the next game
                        // Game has ended, notify FindTheGapManager
                        OnGameEnded?.Invoke();
                        if (ScoreManager.Instance)
                        {
                            if (!TimerManager.Instance.LosePage.activeSelf)
                            {
                                StartCoroutine(ScoreManager.Instance.GameComplete());
                            }
                        }
                        SoundManager.Instance?.CardMatchAudioClip();
                    }
                }
                else
                {
                    // Wrong Shape - Reset to original position
                    dragDrop.ResetPosition();
                    FlashBoundaryManager.OnFlashRequested?.Invoke();
                }
            }
            
        }
    }
}
