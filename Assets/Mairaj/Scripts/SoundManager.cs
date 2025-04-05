using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip gameStartAudioClip;

    [SerializeField]
    private AudioClip gameOverAudioClip;

    [SerializeField]
    private AudioClip lifeLostAudioClip;

    [SerializeField]
    private AudioClip miniGameCompleteAudioClip;

    [SerializeField]
    private AudioClip buttonClickAudioClip;

    [SerializeField]
    private AudioClip cardFlipAudioClip;

    [SerializeField]
    private AudioClip cardMatchAudioClip;

    [SerializeField]
    private AudioClip cardMismatchAudioClip;

    [SerializeField]
    private AudioClip projectileBounceAudioClip;

    [SerializeField]
    private AudioClip shootAudioClip;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    // Declare a function for each audio clip

    public void GameStartAudioClip()
    {
        Instance.audioSource.PlayOneShot(gameStartAudioClip);
    }

    public void GameOverAudioClip()
    {
        Instance.audioSource.PlayOneShot(gameOverAudioClip);
    }

    public void LifeLostAudioClip()
    {
        Instance.audioSource.PlayOneShot(lifeLostAudioClip);
    }

    public void MiniGameCompleteAudioClip()
    {
        Instance.audioSource.PlayOneShot(miniGameCompleteAudioClip);
    }

    public void ButtonClickAudioClip()
    {
        Instance.audioSource.PlayOneShot(buttonClickAudioClip);
    }

    public void CardFlipAudioClip()
    {
        Instance.audioSource.PlayOneShot(cardFlipAudioClip);
    }

    public void CardMatchAudioClip()
    {
        Instance.audioSource.PlayOneShot(cardMatchAudioClip);
    }

    public void CardMismatchAudioClip() {
        Instance.audioSource.PlayOneShot(cardMismatchAudioClip);
    }

    public void ProjectileBounceAudioClip()
    {
        Instance.audioSource.PlayOneShot(projectileBounceAudioClip);
    }

    public void ShootAudioClip()
    {
        Instance.audioSource.PlayOneShot(shootAudioClip);
    }
}
