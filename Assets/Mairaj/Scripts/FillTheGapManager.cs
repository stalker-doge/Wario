// Mairaj Muhammad -> 2415831
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System;

public class FillTheGapManager : MiniGameManagerBase
{
    public static FillTheGapManager Instance { get; private set; }

    [SerializeField]
    private List<GameObject> dropZoneObjects;

    [SerializeField]
    private List<GameObject> dragZoneObjects;

    [SerializeField]
    private List<RectTransform> dragZoneRects;

    [SerializeField]
    private FillTheGapVariant variant = FillTheGapVariant.mTwoSlots;

    [SerializeField]
    private NewFillTheGapVariant newVariant = NewFillTheGapVariant.mXTwoSlots;

    private int randomVariant;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        InitializeGame();
    }

    public override void InitializeGame()
    {
        randomVariant = UnityEngine.Random.Range(0, 2);
        switch (randomVariant)
        {
            case 0:
                variant = FillTheGapVariant.mOneSlots;
                if (newVariant != NewFillTheGapVariant.mXZeroSlots)
                    newVariant = NewFillTheGapVariant.mXOneSlots;
                break;
            case 1:
                variant = FillTheGapVariant.mTwoSlots;
                if (newVariant != NewFillTheGapVariant.mXZeroSlots)
                    newVariant = NewFillTheGapVariant.mXTwoSlots;
                break;
            case 2:
                variant = FillTheGapVariant.mThreeSlots;
                if (newVariant != NewFillTheGapVariant.mXZeroSlots)
                    newVariant = NewFillTheGapVariant.mXThreeSlots;
                break;
            case 3:
                variant = FillTheGapVariant.mFourSlots;
                if (newVariant != NewFillTheGapVariant.mXZeroSlots)
                    newVariant = NewFillTheGapVariant.mXFourSlots;
                break;
            case 4:
                variant = FillTheGapVariant.mFiveSlots;
                if (newVariant != NewFillTheGapVariant.mXZeroSlots)
                    newVariant = NewFillTheGapVariant.mXFiveSlots;
                break;
        }

        SelectRandomDropZonesAndUpdateColor();
    }

    public override void EndGame()
    {
        if (ScoreManager.Instance)
        {
            StartCoroutine(ScoreManager.Instance.GameComplete());
        }
    }

    protected override void RegisterCallbacks()
    {
        
    }

    protected override void UnregisterCallbacks()
    {
        
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
        int numberToSelect = newVariant == NewFillTheGapVariant.mXZeroSlots
            ? VariantToCount(variant)
            : NewVariantToCount(newVariant);

        if (dropZoneObjects.Count < numberToSelect)
        {
            Debug.LogWarning("Not enough DropZone objects in the list.");
            return;
        }

        List<GameObject> selectedDropZones;
        List<NewAcceptedShapeType?> selectedTypes;
        do
        {
            selectedDropZones = dropZoneObjects
                .OrderBy(x => UnityEngine.Random.value)
                .Take(numberToSelect)
                .ToList();

            selectedTypes = selectedDropZones
                .Select(dz => dz.GetComponent<DropZone>()?.GetNewAcceptedShapeType())
                .ToList();

        } while (selectedTypes.Contains(NewAcceptedShapeType.mShape10) &&
                 selectedTypes.Contains(NewAcceptedShapeType.mShape16));

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

            int remainingToAdd = (int)GetLastEnumValue<NewFillTheGapVariant>() - numberToSelect;

            var remainingDragObjects = dragZoneObjects
                .Except(matchingDragObjects)
                .OrderBy(x => UnityEngine.Random.value)
                .ToList();

            bool has10 = matchingDragObjects.Any(d => d.GetComponent<DragDrop>()?.newAcceptedShapeType == NewAcceptedShapeType.mShape10);
            bool has16 = matchingDragObjects.Any(d => d.GetComponent<DragDrop>()?.newAcceptedShapeType == NewAcceptedShapeType.mShape16);

            if (has10)
                remainingDragObjects = remainingDragObjects
                    .Where(d => d.GetComponent<DragDrop>()?.newAcceptedShapeType != NewAcceptedShapeType.mShape16)
                    .ToList();
            else if (has16)
                remainingDragObjects = remainingDragObjects
                    .Where(d => d.GetComponent<DragDrop>()?.newAcceptedShapeType != NewAcceptedShapeType.mShape10)
                    .ToList();

            var distractors = remainingDragObjects.Take(remainingToAdd).ToList();
            var finalDragObjects = matchingDragObjects.Concat(distractors).ToList();

            for (int i = 0; i < dragZoneRects.Count; i++)
            {
                GameObject drag = finalDragObjects[i];
                drag.SetActive(true);
                drag.GetComponent<RectTransform>().anchoredPosition = dragZoneRects[i].anchoredPosition;
                drag.GetComponent<DragDrop>().SetOriginalPosition(dragZoneRects[i].anchoredPosition);
            }
        }

        foreach (var dz in selectedDropZones)
        {
            SetImageColor(dz);
        }

        foreach (GameObject dz in dropZoneObjects)
        {
            if (!selectedDropZones.Contains(dz))
            {
                DropZone dropZoneScript = dz.GetComponent<DropZone>();
                dz.GetComponent<Image>().enabled = false;
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
            NewFillTheGapVariant.mXOneSlots => 1,
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
            }
            else
            {
                Color currentColor = imageComponent.color;
                currentColor.a = 0f;
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
        return values[^1];
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
        UnregisterCallbacks();
    }
}

public enum FillTheGapVariant
{
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
