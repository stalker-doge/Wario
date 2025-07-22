using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashBoundaryManager : MonoBehaviour
{
    [SerializeField]
    private Image flashImage;

    [SerializeField]
    private Image flashCorrectImage;

    [SerializeField]
    private int flashCount = 3;

    [SerializeField]
    private float totalTime = 0.5f;

    [SerializeField]
    private float opacityAplhaValue = 0.2f;

    public static System.Action OnFlashRequested = null;

    public static System.Action OnFlashCorrectRequested = null;

    private void Awake()
    {
        OnFlashRequested += FlashingActivityCallback;
        OnFlashCorrectRequested += FlashingCorrectActivityCallback;
    }

    private void FlashingActivityCallback()
    {
        StartCoroutine(FlashCoroutine());
    }
    private void FlashingCorrectActivityCallback()
    {
        StartCoroutine(FlashCorrectCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        float singleFlashDuration = totalTime / flashCount;
        float halfFlash = singleFlashDuration / 2f;

        Color color = flashImage.color;

        for (int i = 0; i < flashCount; i++)
        {
            // Fade in to alpha 0.2 (50 out of 255)
            float t = 0;
            while (t < halfFlash)
            {
                t += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, opacityAplhaValue, t / halfFlash);
                flashImage.color = new Color(color.r, color.g, color.b, alpha);
                yield return null;
            }

            // Fade out to alpha 0
            t = 0;
            while (t < halfFlash)
            {
                t += Time.deltaTime;
                float alpha = Mathf.Lerp(opacityAplhaValue, 0f, t / halfFlash);
                flashImage.color = new Color(color.r, color.g, color.b, alpha);
                yield return null;
            }
        }

        // Ensure alpha is 0 at the end
        flashImage.color = new Color(color.r, color.g, color.b, 0f);
    }

    private IEnumerator FlashCorrectCoroutine()
    {
        float singleFlashDuration = totalTime / flashCount;
        float halfFlash = singleFlashDuration / 2f;

        Color color = flashCorrectImage.color;

        for (int i = 0; i < flashCount; i++)
        {
            // Fade in to alpha 0.2 (50 out of 255)
            float t = 0;
            while (t < halfFlash)
            {
                t += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, opacityAplhaValue, t / halfFlash);
                flashCorrectImage.color = new Color(color.r, color.g, color.b, alpha);
                yield return null;
            }

            // Fade out to alpha 0
            t = 0;
            while (t < halfFlash)
            {
                t += Time.deltaTime;
                float alpha = Mathf.Lerp(opacityAplhaValue, 0f, t / halfFlash);
                flashCorrectImage.color = new Color(color.r, color.g, color.b, alpha);
                yield return null;
            }
        }

        // Ensure alpha is 0 at the end
        flashCorrectImage.color = new Color(color.r, color.g, color.b, 0f);
    }

    private void OnDestroy()
    {
        OnFlashRequested -= FlashingActivityCallback;
        OnFlashCorrectRequested -= FlashingCorrectActivityCallback;
    }
}
