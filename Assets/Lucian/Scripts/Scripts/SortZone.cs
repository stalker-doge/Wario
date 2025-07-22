using UnityEngine;
using UnityEngine.EventSystems;

public class SortZone : MonoBehaviour, IDropHandler
{
    public float spacing = 120f; // Space between objects
    public Renderer canvasRender;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop called!");
        var sortable = eventData.pointerDrag.GetComponent<SortableObject>();
        if (sortable != null)
        {
            sortable.transform.SetParent(transform);
            sortable.transform.SetAsLastSibling(); // Place at end of list
            // Position will be set by ArrangeChildren
            SortGameManager.Instance.CheckSortingAuto();
            ArrangeChildren();
        }
    }

    public void ArrangeChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i) as RectTransform;
            if (child != null)
            {
                
                child.anchoredPosition = new Vector2(-canvasRender.bounds.max.x + (i * spacing) +50, 0);
            }
        }
    }
} 