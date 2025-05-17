// Mairaj Muhammad -> 2415831
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class FillTheGapManager : MonoBehaviour
{
    // Singleton instance
    public static FillTheGapManager Instance { get; private set; }

    [SerializeField]
    private List<GameObject> dropZoneObjects; // List of DropZone objects

    [SerializeField]
    private FillTheGapVariant variant = FillTheGapVariant.mTwoSlots;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        SelectRandomDropZonesAndUpdateColor();
    }

    public FillTheGapVariant GetVariant()
    {
        return variant;
    }

    private void SelectRandomDropZonesAndUpdateColor()
    {
        int numberToSelect = VariantToCount(variant);

        if (dropZoneObjects.Count < numberToSelect)
        {
            Debug.LogWarning("Not enough DropZone objects in the list.");
            return;
        }

        // Randomly select the required number of distinct drop zones
        List<GameObject> selectedDropZones = dropZoneObjects
            .OrderBy(x => Random.value)
            .Take(numberToSelect)
            .ToList();

        // Set their color to black
        foreach (var dz in selectedDropZones)
        {
            SetImageColor(dz);
        }

        // Remove DropZone component from unselected objects
        foreach (GameObject dz in dropZoneObjects)
        {
            if (!selectedDropZones.Contains(dz))
            {
                DropZone dropZoneScript = dz.GetComponent<DropZone>();
                if (dropZoneScript != null)
                {
                    Destroy(dropZoneScript);
                }
            }
        }
    }

    private int VariantToCount(FillTheGapVariant variant)
    {
        return variant switch
        {
            FillTheGapVariant.mTwoSlots => 2,
            FillTheGapVariant.mThreeSlots => 3,
            FillTheGapVariant.mFourSlots => 4,
            FillTheGapVariant.mFiveSlots => dropZoneObjects.Count,
            _ => 2
        };
    }

    private void SetImageColor(GameObject dropZone)
    {
        Transform childTransform = dropZone.transform.GetChild(0);
        Image imageComponent = childTransform.GetComponent<Image>();

        if (imageComponent != null)
        {
            imageComponent.color = Color.black;
        }
        else
        {
            Debug.LogWarning("No Image component found on the child of " + dropZone.name);
        }
    }
}

public enum FillTheGapVariant
{
    mZeroSlots,
    mOneSlots,
    mTwoSlots,
    mThreeSlots,
    mFourSlots,
    mFiveSlots
}
