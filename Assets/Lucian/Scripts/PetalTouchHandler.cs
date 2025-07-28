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
        raycaster = FindObjectOfType<GraphicRaycaster>();
        eventSystem = FindObjectOfType<EventSystem>();
        raycastResults = new List<RaycastResult>();
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            pointerData = new PointerEventData(eventSystem)
            {
                position = Input.GetTouch(0).position
            };

            raycastResults.Clear();
            raycaster.Raycast(pointerData, raycastResults);

            foreach (var result in raycastResults)
            {
                Petal petal = result.gameObject.GetComponent<Petal>();
                if (petal != null)
                {
                    petal.Pick();
                    break;
                }
            }
        }
    }
}