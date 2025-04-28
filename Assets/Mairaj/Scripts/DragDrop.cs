using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ShapeType shapeType; // Assign in Inspector

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        Debug.Log("XYZ1 " + !eventData.pointerEnter);
        Debug.Log("XYZ2 " + eventData.pointerEnter?.GetComponent<DropZone>() == null);
        if (!eventData.pointerEnter || eventData.pointerEnter?.GetComponent<DropZone>() == null)
        {
            // If not dropped on a valid slot, return to original position
            Debug.Log("XYZ ResetPosition If");
            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        Debug.Log("XYZ Reset Position Called");
        rectTransform.anchoredPosition = originalPosition;
    }
}
public enum ShapeType
{
    LBlock,
    ReverseLBlock,
    TBlock,
    YellowSquareBlock,
    ZBlock
}
