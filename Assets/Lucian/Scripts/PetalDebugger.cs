using UnityEngine;
using UnityEngine.UI;

public class PetalDebugger : MonoBehaviour
{
    [ContextMenu("Debug All Petals")]
    public void DebugAllPetals()
    {
        Petal[] petals = FindObjectsOfType<Petal>();
        Debug.Log($"=== PETAL DEBUG: Found {petals.Length} petals ===");
        
        for (int i = 0; i < petals.Length; i++)
        {
            Petal petal = petals[i];
            GameObject go = petal.gameObject;
            
            Debug.Log($"Petal {i}: {go.name}");
            Debug.Log($"  Active: {go.activeInHierarchy}");
            Debug.Log($"  Position: {go.transform.position}");
            Debug.Log($"  Layer: {go.layer}");
            
            Image image = go.GetComponent<Image>();
            if (image != null)
            {
                Debug.Log($"  Has Image: YES, Raycast Target: {image.raycastTarget}");
                Debug.Log($"  Image Color: {image.color}");
            }
            else
            {
                Debug.Log($"  Has Image: NO - THIS IS THE PROBLEM!");
            }
            
            Button button = go.GetComponent<Button>();
            Debug.Log($"  Has Button: {button != null}");
            
            Canvas parentCanvas = go.GetComponentInParent<Canvas>();
            Debug.Log($"  Parent Canvas: {(parentCanvas != null ? parentCanvas.name : "NONE")}");
        }
    }

    void Start()
    {
        // Auto-debug after a short delay to let spawning complete
        Invoke("DebugAllPetals", 0.5f);
    }
}
