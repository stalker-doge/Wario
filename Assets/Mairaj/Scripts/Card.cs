//Mairaj Muhammad ->2415831
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Card : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [Range(0.05f, 0.5f)][SerializeField] private float rotateTimer = 0.15f;

    private CardType cardType;
    private Sprite frontSprite;
    private Sprite backSprite;

    [SerializeField]
    float defaultWidth = 200f;
    [SerializeField]
    float newWidth = 150f;

    private void Awake()
    {
        FindTwoCardGameManager.EnableCardClicking += ActivateButtonClicking; 
        if (cardImage)
            cardImage.sprite = backSprite;
    }

    public void InitializeCard(CardType type, Sprite front, Sprite back)
    {
        cardType = type;
        frontSprite = front;
        backSprite = back;

        if (cardImage)
            cardImage.sprite = backSprite;
    }

    public CardType GetCardType()
    {
        return cardType;
    }

    public Sprite GetFrontSprite()
    {
        return frontSprite;
    }

    public void ShakeCardAndReset()
    {
        SoundManager.Instance.CardMismatchAudioClip();
        Sequence shakeSequence = DOTween.Sequence();
        shakeSequence.AppendInterval(0.3f);
        shakeSequence.Append(transform.DOShakePosition(0.1f, new Vector3(5f, 0f, 0f), 10, 90, false, true));
        shakeSequence.OnComplete(() =>
        {
            if (Application.platform == RuntimePlatform.Android)
                Handheld.Vibrate();
        });
    }

    public void OnButtonClicked()
    {
        SoundManager.Instance.CardFlipAudioClip();
        FindTwoCardGameManager.OnCardClickedCallback?.Invoke(this);
        Rotate(true);
    }

    public void Rotate(bool showFront, Action CompletionCallback = null, bool rotateInstant = false)
    {
        SoundManager.Instance?.CardFlipAudioClip();

        GetComponent<Button>().enabled = false;
        transform.DORotate(new Vector3(0, 90, 0), rotateInstant ? 0 : rotateTimer, RotateMode.Fast).OnComplete(() =>
        {
            cardImage.sprite = showFront ? frontSprite : backSprite;

            RectTransform rect = cardImage.transform as RectTransform;
            Vector2 size = rect.sizeDelta;
            size.x = showFront ? newWidth : defaultWidth;
            rect.sizeDelta = size;

            transform.DORotate(new Vector3(0, 0, 0), rotateInstant ? 0 : rotateTimer, RotateMode.Fast).OnComplete(() =>
                {
                    GetComponent<Button>().enabled = !showFront;
                    CompletionCallback?.Invoke();
                });
        });
    }

    public void ResetCard(Action CompletionCallback = null)
    {
        Rotate(false, CompletionCallback);
    }

    public float GetRotateTimer()
    {
        return rotateTimer;
    }

    private void ActivateButtonClicking(bool activate)
    {
        GetComponent<Button>().enabled = activate;
    }

    private void OnDestroy()
    {
        FindTwoCardGameManager.EnableCardClicking -= ActivateButtonClicking;
    }
}
