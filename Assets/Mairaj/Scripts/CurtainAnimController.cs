//Mairaj Muhammad ->2415831
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

        KillAnimations(leftRect, rightRect);

        isAtCenter = true;

        leftRect.DOAnchorPosX(-200f, animTimer).SetEase(Ease.Linear);
        rightRect.DOAnchorPosX(600f, animTimer).SetEase(Ease.Linear).OnComplete(() => {
            CompletionCallback?.Invoke();
        });
    }

    private void KillAnimations(RectTransform leftRect, RectTransform rightRect)
    {
        leftRect.DOKill();
        rightRect.DOKill();
    }

    public void ResetAnims()
    {
        isAtCenter=false;
    }

    public void AnimateAwayFromCenter(float animTimer, System.Action CompletionCallback)
    {
        if (!isAtCenter)
        {
            //Debug.Log("XYZ Return");
            return;
        }

        RectTransform leftRect = leftImage.GetComponent<RectTransform>();
        RectTransform rightRect = rightImage.GetComponent<RectTransform>();

        KillAnimations(leftRect, rightRect);

        isAtCenter = false;

        leftRect.DOAnchorPosX(-1350f, animTimer).SetEase(Ease.Linear);
        rightRect.DOAnchorPosX(1570f, animTimer).SetEase(Ease.Linear).OnComplete(() =>
        {
            CompletionCallback?.Invoke();
            Destroy(gameObject.transform.parent.gameObject);
        });
    }
}
