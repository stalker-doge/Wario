// Mairaj Muhammad -> 2415831
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderController : MonoBehaviour
{
    [SerializeField]
    private Slider volumeSlider;

    void Start()
    {
        // Load saved volume and set slider
        float savedVolume = PlayerPrefs.GetFloat(SoundManager.Instance?.GetGameVolumeKey, 1f);
        volumeSlider.value = savedVolume;

        // Add listener
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    void OnVolumeChanged(float volume)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetVolume(volume);
        }
    }
}
