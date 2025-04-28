using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class FillTheGapManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> dropZoneObjects;  // List of DropZone objects

    private void Start()
    {
        // Select two random DropZone objects, set their color to black, and remove DropZone component from unused ones
        SelectRandomDropZonesAndUpdateColor();
    }

    private void SelectRandomDropZonesAndUpdateColor()
    {
        if (dropZoneObjects.Count < 2)
        {
            Debug.LogWarning("Not enough DropZone objects in the list.");
            return;
        }

        // Randomly select two different DropZone objects from the list
        int randomIndex1 = Random.Range(0, dropZoneObjects.Count);
        int randomIndex2 = randomIndex1;

        while (randomIndex2 == randomIndex1)  // Ensure the second index is different from the first
        {
            randomIndex2 = Random.Range(0, dropZoneObjects.Count);
        }

        // Get the selected DropZone objects
        GameObject dropZone1 = dropZoneObjects[randomIndex1];
        GameObject dropZone2 = dropZoneObjects[randomIndex2];

        // Set the color of the child image components of the selected DropZone objects to black
        SetImageColor(dropZone1);
        SetImageColor(dropZone2);

        // Remove DropZone component from the unselected items
        RemoveDropZoneComponent(dropZone1, dropZone2);
    }

    private void SetImageColor(GameObject dropZone)
    {
        // Find the child Image component and modify its color to black
        Transform childTransform = dropZone.transform.GetChild(0); // Assuming the child holding the Image is the first child
        Image imageComponent = childTransform.GetComponent<Image>();

        if (imageComponent != null)
        {
            // Set the color of the image to black
            imageComponent.color = Color.black;
        }
        else
        {
            Debug.LogWarning("No Image component found on the child of " + dropZone.name);
        }
    }

    private void RemoveDropZoneComponent(GameObject dropZone1, GameObject dropZone2)
    {
        // Loop through all dropZoneObjects and remove the DropZone component if not selected
        foreach (GameObject dropZone in dropZoneObjects)
        {
            if (dropZone != dropZone1 && dropZone != dropZone2)
            {
                DropZone dropZoneScript = dropZone.GetComponent<DropZone>();
                if (dropZoneScript != null)
                {
                    Destroy(dropZoneScript); // Remove the DropZone component from the unselected drop zones
                }
            }
        }
    }
}
