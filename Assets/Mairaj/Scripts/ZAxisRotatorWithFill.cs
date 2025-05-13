//Mairaj Muhammad ->2415831
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ZAxisRotatorWithFill : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject objectToRotate;
    [SerializeField] private Image timerFillImage;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationAmount = 45f; // Degrees per press
    [SerializeField] private float rotationDuration = 0.3f; // Duration of tween

    [Header("Fill Settings")]
    [SerializeField] private float fillIncrement = 0.1f; // Fill increase per press

    private float currentRotation = 0f;

    public void OnButtonPressed()
    {
        if (objectToRotate != null)
        {
            currentRotation += rotationAmount;
            objectToRotate.transform
                .DORotate(new Vector3(0f, 0f, currentRotation), rotationDuration)
                .SetEase(Ease.OutBack);
        }

        if (timerFillImage != null)
        {
            timerFillImage.fillAmount = Mathf.Clamp01(timerFillImage.fillAmount + fillIncrement);
        }
    }
}
