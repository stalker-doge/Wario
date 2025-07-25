// Mairaj Muhammad -> 2415831
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameOverCaseAnim : MonoBehaviour
{
    [SerializeField]
    private Sprite[] fireExplosionSprites;
    [SerializeField]
    private Sprite[] bombExplosionSprites;
    [SerializeField]
    private Sprite bombOriginalSprite;
    [SerializeField]
    private Image bombFalling;
    [SerializeField]
    private Image fireExplosionImage;
    [SerializeField]
    private Image gameOverText;

    private RectTransform canvasRect;

    private void ResetDefaults()
    {
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        // Start bomb at top of screen
        bombFalling.rectTransform.anchoredPosition = new Vector2(0, canvasRect.rect.height);
        fireExplosionImage.gameObject.SetActive(false);
        gameOverText.transform.localScale = Vector3.zero;
        gameOverText.transform.rotation = Quaternion.identity;
    }

    private void OnEnable()
    {
        PlayAnimationWrapper();
    }

    private void PlayAnimationWrapper()
    {
        ResetDefaults();
        PlayAnimation();
        SoundManager.Instance.GameOverExplosionAudioClip(); // Optional: replace with actual sound
    }

    private void PlayAnimation()
    {
        // Bomb falls in 0.25s
        bombFalling.sprite = bombOriginalSprite;
        bombFalling.gameObject.SetActive(true);
        bombFalling.rectTransform
            .DOAnchorPosY(-canvasRect.rect.height * 0.25f, 0.25f)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                // Start bomb explosion animation
                StartCoroutine(PlayBombExplosion());
            });
    }

    private IEnumerator PlayBombExplosion()
    {
        float frameRate = 0.05f; // 20 FPS (0.5s total for ~10 frames)

        for (int i = 0; i < bombExplosionSprites.Length; i++)
        {
            bombFalling.sprite = bombExplosionSprites[i];
            yield return new WaitForSeconds(frameRate);
        }

        bombFalling.gameObject.SetActive(false);

        // Play fire explosion + game over text
        StartCoroutine(PlayFireExplosion());
        AnimateGameOverText();
    }

    private IEnumerator PlayFireExplosion()
    {
        fireExplosionImage.gameObject.SetActive(true);
        float frameRate = 0.05f;

        foreach (var sprite in fireExplosionSprites)
        {
            fireExplosionImage.sprite = sprite;
            yield return new WaitForSeconds(frameRate);
        }

        fireExplosionImage.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void AnimateGameOverText()
    {
        gameOverText.gameObject.SetActive(true);
        gameOverText.transform
            .DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack);

        gameOverText.transform
            .DORotate(new Vector3(0, 0, 360), 0.25f, RotateMode.FastBeyond360)
            .SetDelay(0.25f)
            .SetEase(Ease.OutCubic);
    }
}
