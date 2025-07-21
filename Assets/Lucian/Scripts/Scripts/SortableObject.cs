using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SortableObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public enum SizeType { Small, Medium, Large }
    public SizeType objectSize;

    private Vector3 startPosition;
    private Transform originalParent;
    private CanvasGroup canvasGroup;
    private Canvas canvas;

    public static List<SortableObject> allObjects = new List<SortableObject>();

    private Vector3 initialPosition;
    private Transform initialParent;

    private void Awake()
    {
        allObjects.Add(this);
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        // Store initial position and parent
        initialPosition = transform.position;
        initialParent = transform.parent;
    }

    private void OnDestroy()
    {
        allObjects.Remove(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        originalParent = transform.parent;
        canvasGroup.blocksRaycasts = false; // Allow raycasts to pass through while dragging
        transform.SetParent(canvas.transform); // Move to top of hierarchy for dragging
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Use eventData.position for UI dragging
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector2 localPoint
        );
        transform.localPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        // If not dropped in a zone, return to start
        if (transform.parent == originalParent || transform.parent == canvas.transform)
        {
            transform.position = startPosition;
            transform.SetParent(originalParent);
        }
    }

    public void ResetToOriginal()
    {
        transform.SetParent(initialParent);
        transform.position = initialPosition;
    }
} 