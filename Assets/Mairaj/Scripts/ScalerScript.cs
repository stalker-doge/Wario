// Mairaj Muhammad -> 2415831
using System.Collections;
using UnityEngine;

public class ScalerScript : MonoBehaviour
{
    [Header("Scaling Settings")]
    public Vector3 maxScale = new Vector3(1.5f, 1.5f, 1.5f);
    public float duration = 1f;

    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
        StartCoroutine(ScaleLoop());
    }

    private IEnumerator ScaleLoop()
    {
        while (true)
        {
            // Scale up
            yield return StartCoroutine(ScaleObject(initialScale, maxScale, duration / 2f));
            // Scale down
            yield return StartCoroutine(ScaleObject(maxScale, initialScale, duration / 2f));
        }
    }

    private IEnumerator ScaleObject(Vector3 from, Vector3 to, float time)
    {
        float elapsed = 0f;
        while (elapsed < time)
        {
            transform.localScale = Vector3.Lerp(from, to, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = to;
    }
}
