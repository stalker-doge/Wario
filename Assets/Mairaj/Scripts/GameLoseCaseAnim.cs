using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameLoseCaseAnim : MonoBehaviour
{
    [SerializeField]
    private Sprite[] blackScreenWipeAnimationSprites;
    [SerializeField]
    private Sprite[] heartBrokenAnimationSprites;
    [SerializeField]
    private Image blackScreenImage;
    [SerializeField]
    private Image heartBrokenImage;
    [SerializeField]
    private Image animCharactersFromBottom;
    [SerializeField]
    private Image loseTextFromBottom;

    private RectTransform canvasRect;

    private void ResetDefaults()
    {
        // Scale win text to zero initially
        loseTextFromBottom.transform.localScale = Vector3.zero;

        blackScreenImage.gameObject.SetActive(false);
        heartBrokenImage.gameObject.SetActive(false);

        // Move animCharacters off-screen (bottom of canvas)
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        var characterRT = animCharactersFromBottom.rectTransform;
        characterRT.anchoredPosition = new Vector2(0, -canvasRect.rect.height);
    }

    private void PlayAnimation()
    {
        AnimateLoseText();
        AnimateCharacterRise();
        StartCoroutine(PlayBlackScreenAnimation());
        StartCoroutine(PlayHeartBrokenAnimation());
    }

    private void OnEnable()
    {
        ResetDefaults();
        PlayAnimation();
        SoundManager.Instance.GameLoseSadAudioClip(); // Changed from Win to Lose audio
    }

    private void AnimateLoseText()
    {
        // Scale up with OutBack ease
        loseTextFromBottom.transform
            .DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack);

        // Rotate 360 degrees after 0.25s delay (around Z-axis)
        loseTextFromBottom.transform
            .DORotate(new Vector3(0, 0, 360), 0.25f, RotateMode.FastBeyond360)
            .SetDelay(0.25f)
            .SetEase(Ease.OutCubic);
    }

    private void AnimateCharacterRise()
    {
        animCharactersFromBottom.rectTransform
            .DOAnchorPosY(0, 0.5f)
            .SetEase(Ease.OutCubic);
    }

    private IEnumerator PlayBlackScreenAnimation()
    {
        blackScreenImage.gameObject.SetActive(true);
        float frameRate = 0.05f; // 20 FPS

        foreach (var sprite in blackScreenWipeAnimationSprites)
        {
            blackScreenImage.sprite = sprite;
            yield return new WaitForSeconds(frameRate);
        }

        yield return new WaitForSeconds(1f);
        blackScreenImage.gameObject.SetActive(false);
    }

    private IEnumerator PlayHeartBrokenAnimation()
    {
        heartBrokenImage.gameObject.SetActive(true);
        float frameRate = 0.05f; // 20 FPS

        foreach (var sprite in heartBrokenAnimationSprites)
        {
            heartBrokenImage.sprite = sprite;
            yield return new WaitForSeconds(frameRate);
        }

        heartBrokenImage.gameObject.SetActive(false);
    }
}
