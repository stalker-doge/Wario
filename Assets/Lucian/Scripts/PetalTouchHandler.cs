using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class PetalTouchHandler : MonoBehaviour
{
    private PointerEventData pointerData;
    private List<RaycastResult> raycastResults;
    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;

    void Start()
    {
        // Try to find raycaster on the same canvas first
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            raycaster = canvas.GetComponent<GraphicRaycaster>();
        }
        
        // Fallback to finding any raycaster in scene
        if (raycaster == null)
        {
            raycaster = FindObjectOfType<GraphicRaycaster>();
        }

        eventSystem = FindObjectOfType<EventSystem>();
        raycastResults = new List<RaycastResult>();

        // Debug checks
        if (raycaster == null)
        {
            Debug.LogError("No GraphicRaycaster found! Make sure your Canvas has a GraphicRaycaster component.");
        }
        if (eventSystem == null)
        {
            Debug.LogError("No EventSystem found! Make sure you have an EventSystem in your scene.");
        }
    }

    void Update()
    {
        if (raycaster == null || eventSystem == null) return;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            pointerData = new PointerEventData(eventSystem)
            {
                position = Input.GetTouch(0).position
            };

            raycastResults.Clear();
            raycaster.Raycast(pointerData, raycastResults);

            Debug.Log($"Raycast hit {raycastResults.Count} objects at position {Input.GetTouch(0).position}");

            foreach (var result in raycastResults)
            {
                Debug.Log($"Hit object: {result.gameObject.name}");
                Petal petal = result.gameObject.GetComponent<Petal>();
                if (petal != null)
                {
                    Debug.Log($"Picking petal: {result.gameObject.name}");
                    petal.Pick();
                    break;
                }
            }
        }
    }
}