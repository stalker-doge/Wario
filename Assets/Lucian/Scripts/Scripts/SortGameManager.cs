using UnityEngine;
using TMPro;
using System.Linq;

public class SortGameManager : MonoBehaviour
{
    public static SortGameManager Instance { get; private set; }

    public SortZone zone;
    public TextMeshProUGUI feedbackText;

    private void Awake()
    {
        Instance = this;
    }

    public void CheckSortingAuto()
    {
        var children = zone.transform.Cast<Transform>()
            .Select(t => t.GetComponent<SortableObject>())
            .Where(obj => obj != null)
            .ToList();

        // Check if all objects are in the zone
        if (children.Count != SortableObject.allObjects.Count)
        {
            feedbackText.text = "";
            return;
        }

        // Define the correct order
        var correctOrder = children.OrderBy(obj => (int)obj.objectSize).ToList();

        bool isCorrect = true;
        for (int i = 0; i < children.Count; i++)
        {
            if (children[i].objectSize != correctOrder[i].objectSize)
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            feedbackText.text = "Correct!";
        }
        else
        {
            feedbackText.text = "Try Again!";
            // Send all images back to their original positions
            foreach (var obj in children)
            {
                obj.ResetToOriginal();
            }
            zone.ArrangeChildren();
        }
    }
} 