// Mairaj Muhammad -> 2415831
using UnityEngine;
using DG.Tweening;
using System;

public class CurtainAnimController : MonoBehaviour
{
    [Header("Curtain Images")]
    [SerializeField] private GameObject leftImage;
    [SerializeField] private GameObject rightImage;

    private bool isAtCenter = false;

    // Singleton instance
    public static CurtainAnimController Instance { get; private set; }

    // Public callback for external destruction trigger
    public static Action DestroyParentCallback = null;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        Activate(false);
        DestroyParentCallback += DestroyParent;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;

        DestroyParentCallback -= DestroyParent;
    }

    private void Activate(bool isActive)
    {
        leftImage.SetActive(isActive);
        rightImage.SetActive(isActive);
    }

    public void AnimateTowardsCenter(float animTimer, Action CompletionCallback)
    {
        Activate(true);

        RectTransform leftRect = leftImage.GetComponent<RectTransform>();
        RectTransform rightRect = rightImage.GetComponent<RectTransform>();

        KillAnimations(leftRect, rightRect);

        isAtCenter = true;

        leftRect.DOAnchorPosX(-780f, animTimer).SetEase(Ease.Linear);
        rightRect.DOAnchorPosX(850f, animTimer).SetEase(Ease.Linear)
            .OnComplete(() => CompletionCallback?.Invoke());
    }

    public void AnimateAwayFromCenter(float animTimer, Action CompletionCallback)
    {
        if (!isAtCenter)
            return;

        RectTransform leftRect = leftImage.GetComponent<RectTransform>();
        RectTransform rightRect = rightImage.GetComponent<RectTransform>();

        KillAnimations(leftRect, rightRect);

        isAtCenter = false;

        leftRect.DOAnchorPosX(-2000f, animTimer).SetEase(Ease.Linear);
        rightRect.DOAnchorPosX(2000f, animTimer).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                CompletionCallback?.Invoke();
                Destroy(gameObject.transform.parent.gameObject);
            });
    }

    public void ResetAnims()
    {
        isAtCenter = false;
    }

    private void KillAnimations(RectTransform leftRect, RectTransform rightRect)
    {
        leftRect.DOKill();
        rightRect.DOKill();
    }

    private void DestroyParent()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}
