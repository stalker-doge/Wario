// Mairaj Muhammad -> 2415831
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameWinCaseAnim : MonoBehaviour
{
    [SerializeField]
    private Sprite[] sparkleSprites;
    [SerializeField]
    private Image sparkleImage;
    [SerializeField]
    private Image animCharactersFromBottom;
    [SerializeField]
    private Image winTextFromBottom;

    private RectTransform canvasRect;

    private void ResetDefaults()
    {
        // Scale win text to zero initially
        winTextFromBottom.transform.localScale = Vector3.zero;

        sparkleImage.gameObject.SetActive(false);

        // Move animCharacters off-screen (bottom of canvas)
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        var characterRT = animCharactersFromBottom.rectTransform;
        characterRT.anchoredPosition = new Vector2(0, -canvasRect.rect.height);
    }

    private void PlayAnimation()
    {
        AnimateWinText();
        AnimateCharacterRise();
        StartCoroutine(PlaySparkleAnimation());
    }

    private void OnEnable()
    {
        ResetDefaults();
        PlayAnimation();
        SoundManager.Instance.GameWinCheerAudioClip();
    }

    private void AnimateWinText()
    {
        // Scale up with OutBack ease
        winTextFromBottom.transform
            .DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack);

        // Rotate 360 degrees after 0.25s delay (around Z-axis)
        winTextFromBottom.transform
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

    private IEnumerator PlaySparkleAnimation()
    {
        sparkleImage.gameObject.SetActive(true);
        float frameRate = 0.05f; // 20 FPS
        foreach (var sprite in sparkleSprites)
        {
            sparkleImage.sprite = sprite;
            yield return new WaitForSeconds(frameRate);
        }

        sparkleImage.gameObject.SetActive(false);
    }
}
