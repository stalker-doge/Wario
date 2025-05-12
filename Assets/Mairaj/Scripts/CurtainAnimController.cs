using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CurtainAnimController : MonoBehaviour
{
    [SerializeField]
    private GameObject leftImage;
    [SerializeField]
    private GameObject rightImage;

    private bool isAtCenter = false;

    private void Awake()
    {
        Activate(false);
    }
    private void Activate(bool isActive)
    {
        leftImage.SetActive(isActive);
        rightImage.SetActive(isActive);
    }

    public void AnimateTowardsCenter(float animTimer, System.Action CompletionCallback)
    {
        Activate(true);
        RectTransform leftRect = leftImage.GetComponent<RectTransform>();
        RectTransform rightRect = rightImage.GetComponent<RectTransform>();

        leftRect.DOAnchorPosX(-200f, animTimer).SetEase(Ease.Linear);
        rightRect.DOAnchorPosX(400f, animTimer).SetEase(Ease.Linear).OnComplete(() => { 
            CompletionCallback?.Invoke();
            isAtCenter = true;
        });
    }

    public void AnimateAwayFromCenter(float animTimer, System.Action CompletionCallback)
    {
        if (!isAtCenter)
        {
            return;
        }

        RectTransform leftRect = leftImage.GetComponent<RectTransform>();
        RectTransform rightRect = rightImage.GetComponent<RectTransform>();

        leftRect.DOAnchorPosX(-1350f, animTimer).SetEase(Ease.Linear);
        rightRect.DOAnchorPosX(1570f, animTimer).SetEase(Ease.Linear).OnComplete(() =>
        {
            CompletionCallback?.Invoke();
            Destroy(gameObject);
        });
    }
}
