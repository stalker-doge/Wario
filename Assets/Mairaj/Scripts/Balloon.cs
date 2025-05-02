using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    public static System.Action BalloonPoppedCallback = null;

    [SerializeField]
    private BalloonType type;
    public void OnBalloonClicked() 
    {
        if (type == BalloonType.Red) {
            
            SoundManager.Instance.CardMismatchAudioClip();

            // Create a sequence for handling the delay and the shake
            Sequence shakeSequence = DOTween.Sequence();

            // Add the shake position effect after the delay
            shakeSequence.Append(transform.DOShakePosition(0.1f, new Vector3(20f, 0f, 0f), 10, 90, false, true)
                .SetLoops(UnityEngine.Random.Range(3, 3)));  // 3 to x iterations of shake

            // Add vibration for Android after the shake
            shakeSequence.OnComplete(() =>
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    Handheld.Vibrate();  // Vibrate the device
                }
            });
        } else {
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
