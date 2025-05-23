// Mairaj Muhammad -> 2415831
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System;

public class FillTheGapManager : MonoBehaviour
{
    // Singleton instance
    public static FillTheGapManager Instance { get; private set; }

    [SerializeField]
    private List<GameObject> dropZoneObjects; // List of DropZone objects

    [SerializeField]
    private List<GameObject> dragZoneObjects; // List of Draggable objects, needed for selecting 5 at random out of which x should have a drop slot

    [SerializeField]
    private List<RectTransform> dragZoneRects;

    [SerializeField]
    private FillTheGapVariant variant = FillTheGapVariant.mTwoSlots;

    [SerializeField]
    private NewFillTheGapVariant newVariant = NewFillTheGapVariant.mXTwoSlots;

    private int randomVariant;

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
        randomVariant= UnityEngine.Random.Range(0, 3);
        switch(randomVariant)
        {
            case 0:variant= FillTheGapVariant.mOneSlots;
                break;
            case 1: variant = FillTheGapVariant.mTwoSlots;
                break;
            case 2:
                variant = FillTheGapVariant.mThreeSlots;
                break;
        }
        Debug.Log("POLO");
        SelectRandomDropZonesAndUpdateColor();
    }

    public FillTheGapVariant GetVariant()
    {
        return variant;
    }

    public NewFillTheGapVariant GetNewVariant()
    {
        return newVariant;
    }

    private void SelectRandomDropZonesAndUpdateColor()
    {
        int numberToSelect = 0;
        if (newVariant == NewFillTheGapVariant.mXZeroSlots)
        {
            numberToSelect = VariantToCount(variant);
        } else
        {
            numberToSelect = NewVariantToCount(newVariant);
        }
            

        if (dropZoneObjects.Count < numberToSelect)
        {
            Debug.LogWarning("Not enough DropZone objects in the list.");
            return;
        }

        // Randomly select the required number of distinct drop zones
        List<GameObject> selectedDropZones = dropZoneObjects
            .OrderBy(x => UnityEngine.Random.value)
            .Take(numberToSelect)
            .ToList();

        if (newVariant != NewFillTheGapVariant.mXZeroSlots)
        {
            var matchingDragObjects = dragZoneObjects
            .Where(drag =>
            {
                var dragType = drag.GetComponent<DragDrop>()?.newAcceptedShapeType;
                return selectedDropZones.Any(dz =>
                    dz.GetComponent<DropZone>()?.GetNewAcceptedShapeType() == dragType);
            })
            .ToList();

            // Calculate how many more we need to add
            int remainingToAdd = (int)GetLastEnumValue<NewFillTheGapVariant>() - numberToSelect;

            // Find the remaining dragZoneObjects that are NOT in matchingDragObjects
            var remainingDragObjects = dragZoneObjects
                .Except(matchingDragObjects)
                .OrderBy(x => UnityEngine.Random.value) // Shuffle randomly
                .Take(remainingToAdd)       // Take as many as needed
                .ToList();

            // Combine both matching and random distractors
            var finalDragObjects = matchingDragObjects
                .Concat(remainingDragObjects)
                .ToList();

            // Activate only these 5 final drag objects and assign to dragZoneRects
            for (int i = 0; i < dragZoneRects.Count; i++)
            {
                GameObject drag = finalDragObjects[i];
                drag.SetActive(true);
                drag.GetComponent<RectTransform>().anchoredPosition = dragZoneRects[i].anchoredPosition;
                drag.GetComponent<DragDrop>().SetOriginalPosition(dragZoneRects[i].anchoredPosition);
            }
        }

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
            FillTheGapVariant.mOneSlots => 1,
            FillTheGapVariant.mTwoSlots => 2,
            FillTheGapVariant.mThreeSlots => 3,
            FillTheGapVariant.mFourSlots => 4,
            FillTheGapVariant.mFiveSlots => 5,
            _ => 2
        };
    }

    private int NewVariantToCount(NewFillTheGapVariant variant)
    {
        return variant switch
        {
            NewFillTheGapVariant.mXTwoSlots => 2,
            NewFillTheGapVariant.mXThreeSlots => 3,
            NewFillTheGapVariant.mXFourSlots => 4,
            NewFillTheGapVariant.mXFiveSlots => 5,
            _ => 2
        };
    }

    private void SetImageColor(GameObject dropZone)
    {
        Transform childTransform = dropZone.transform.GetChild(0);
        Image imageComponent = childTransform.GetComponent<Image>();

        if (imageComponent != null)
        {
            if (newVariant == NewFillTheGapVariant.mXZeroSlots)
            {
                imageComponent.color = Color.black;
            } else
            {
                Color currentColor = imageComponent.color;
                currentColor.a = 0f; // Set alpha to 0 (fully transparent)
                imageComponent.color = currentColor;
            }
        }
        else
        {
            Debug.LogWarning("No Image component found on the child of " + dropZone.name);
        }

    }
    public static TEnum GetLastEnumValue<TEnum>() where TEnum : Enum
    {
        var values = (TEnum[])Enum.GetValues(typeof(TEnum));
        return values[^1]; // ^1 is the last element (C# 8+)
    }

}

public enum FillTheGapVariant
{
    //mZeroSlots,
    mOneSlots,
    mTwoSlots,
    mThreeSlots,
    mFourSlots,
    mFiveSlots,
}

public enum NewFillTheGapVariant
{
    mXZeroSlots,
    mXOneSlots,
    mXTwoSlots,
    mXThreeSlots,
    mXFourSlots,
    mXFiveSlots
}