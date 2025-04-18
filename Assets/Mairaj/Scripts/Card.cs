//Mairaj Muhammad -->2415831
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Card : MonoBehaviour
{
    [SerializeField]
    private Text cardNo;

    [Header("One Sided (Back, Front) Rotation Timer")]
    [Range(0.05f, 0.5f)]
    [SerializeField]
    private float rotateTimer = 0.15f;

    private int cardNum;

    private void Awake()
    {

        if (cardNo)
        {
            cardNo.gameObject.SetActive(false);
        }
    }

    public void InitializeCardNumber(int number)
    {
        this.cardNo.text = number + "";
        cardNum = number;
    }

    public int GetCardNumber()
    {
        return cardNum;
    }

    public void ShakeCardAndReset()
    {
        // Play a mismatch sound
        SoundManager.Instance.CardMismatchAudioClip();

        // Create a sequence for handling the delay and the shake
        Sequence shakeSequence = DOTween.Sequence();

        // Add a delay of 0.3 seconds before starting the shake
        shakeSequence.AppendInterval(0.3f);

        // Add the shake position effect after the delay
        shakeSequence.Append(transform.DOShakePosition(0.1f, new Vector3(5f, 0f, 0f), 10, 90, false, true)
            .SetLoops(Random.Range(3, 3)));  // 3 to x iterations of shake

        // Add vibration for Android after the shake
        shakeSequence.OnComplete(() =>
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                Handheld.Vibrate();  // Vibrate the device
            }
        });
    }

    public void OnButtonClicked()
    {
        // Play a flip sound
        SoundManager.Instance.CardFlipAudioClip();

        // Notify the manager that this card was clicked
        FindObjectOfType<FindTwoCardGameManager>().OnCardClicked(this);

        // Rotate the card
        Rotate(true);
    }

    private void Rotate(bool setTextActive)
    {
        // Play a flip sound
        SoundManager.Instance.CardFlipAudioClip();

        GetComponent<Button>().enabled = false;
        transform.DORotate(new Vector3(0, 90, 0), rotateTimer, RotateMode.Fast).OnComplete(() => {
            cardNo.gameObject.SetActive(setTextActive);
            transform.DORotate(new Vector3(0, 0, 0), rotateTimer, RotateMode.Fast).OnComplete(() =>
            {
                GetComponent<Button>().enabled = !setTextActive;
            });
        });
    }

    public void ResetCard()
    {
        Rotate(false);
    }

    public float GetRotateTimer()
    {
        return rotateTimer;
    }
}
