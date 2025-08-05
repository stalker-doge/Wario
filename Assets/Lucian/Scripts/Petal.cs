using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class Petal : MonoBehaviour, IPointerClickHandler
{
    private bool isPicked = false;

    void Start()
    {
        // Ensure the petal is properly set up for UI raycasting
        Image image = GetComponent<Image>();
        if (image == null)
        {
            image = gameObject.AddComponent<Image>();
            Debug.LogWarning($"Added missing Image component to {gameObject.name}");
        }
        
        // Make sure raycast target is enabled
        image.raycastTarget = true;
        
        // Ensure it has some color (transparent is fine but alpha shouldn't be 0)
        if (image.color.a <= 0)
        {
            Color color = image.color;
            color.a = 0.01f; // Nearly transparent but still raycastable
            image.color = color;
        }

        Debug.Log($"Petal {gameObject.name} setup complete - Raycast Target: {image.raycastTarget}");
    }

    public void Pick()
    {
        if (isPicked) return;

        Debug.Log($"Picking petal: {gameObject.name}");
        isPicked = true;
        PetalGameManager.Instance.PetalPicked();
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"OnPointerClick called on {gameObject.name}");
        Pick();
    }
}