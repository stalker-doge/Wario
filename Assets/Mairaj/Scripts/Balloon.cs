using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    public static System.Action BalloonPoppedCallback = null;
    public void OnBalloonClicked() 
    {
        BalloonPoppedCallback?.Invoke();
        SoundManager.Instance.BalloonPopAudioClip();
        Destroy(gameObject);
    }
}
