using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    public static System.Action BalloonPoppedCallback = null;
    public static List<Balloon> allBalloons = new List<Balloon>();

    [SerializeField]
    private BalloonType type;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 20f;

    [Header("Boundary Padding")]
    [SerializeField] private float leftPadding = 200f;
    [SerializeField] private float rightPadding = 200f;
    [SerializeField] private float topPadding = 250f;
    [SerializeField] private float bottomPadding = 600f;

    private RectTransform rect;
    private RectTransform canvasRect;
    private Vector2 direction;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        direction = Random.insideUnitCircle.normalized;

        allBalloons.Add(this);
    }

    private void OnDestroy()
    {
        allBalloons.Remove(this);
    }

    private void Start()
    {
        // Nothing needed for now
    }

    private void Update()
    {
        MoveBalloon();
    }

    private void LateUpdate()
    {
        RepelFromOthers();
    }

    private void MoveBalloon()
    {
        Vector2 pos = rect.anchoredPosition;
        pos += direction * moveSpeed * Time.deltaTime;

        float halfWidth = rect.rect.width / 2f;
        float halfHeight = rect.rect.height / 2f;

        float minX = -canvasRect.rect.width / 2 + halfWidth + leftPadding;
        float maxX = canvasRect.rect.width / 2 - halfWidth - rightPadding;

        float minY = -canvasRect.rect.height / 2 + halfHeight + topPadding;
        float maxY = canvasRect.rect.height / 2 - halfHeight - bottomPadding;

        // Bounce off boundaries
        if (pos.x <= minX || pos.x >= maxX) direction.x *= -1;
        if (pos.y <= minY || pos.y >= maxY) direction.y *= -1;

        // Clamp to padded area
        rect.anchoredPosition = new Vector2(
            Mathf.Clamp(pos.x, minX, maxX),
            Mathf.Clamp(pos.y, minY, maxY)
        );
    }

    private void RepelFromOthers()
    {
        foreach (Balloon other in allBalloons)
        {
            if (other == this) continue;

            Vector2 diff = rect.anchoredPosition - other.rect.anchoredPosition;
            float distance = diff.magnitude;
            float minDist = rect.rect.width * 2.0f; // ~10% extra space

            if (distance < minDist && distance > 0.01f)
            {
                Vector2 repel = diff.normalized * (minDist - distance) * 0.5f;
                rect.anchoredPosition += repel;
            }
        }
    }

    public void OnBalloonClicked()
    {
        if (type == BalloonType.Red)
        {
            SoundManager.Instance.CardMismatchAudioClip();

            Sequence shakeSequence = DOTween.Sequence();
            shakeSequence.Append(transform.DOShakePosition(0.1f, new Vector3(20f, 0f, 0f), 10, 90, false, true)
                .SetLoops(UnityEngine.Random.Range(3, 3)));

            shakeSequence.OnComplete(() =>
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    Handheld.Vibrate();
                }
            });
        }
        else
        {
            BalloonPoppedCallback?.Invoke();
            SoundManager.Instance.BalloonPopAudioClip();
            Destroy(gameObject);
        }
    }

    public enum BalloonType
    {
        Red,
        Yellow,
        Blue
    }
}
