//Mairaj Muhammad ->2415831
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

    [SerializeField]
    private AudioClip balloonPopAudioClip;
    
    public AudioClip MinigameMusicAudioClip;
    
    [SerializeField]
    private AudioClip MenuMusicAudioClip;

    private string GAME_VOLUME_KEY = "GameVolume";

    public string GetGameVolumeKey
    {
        get {  return GAME_VOLUME_KEY; }
    }
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
        LoadSavedVolume();
        DontDestroyOnLoad(gameObject);
    }

    private void LoadSavedVolume()
    {
        // Load saved volume or default to full volume
        float savedVolume = PlayerPrefs.GetFloat(GAME_VOLUME_KEY, 1f);
        audioSource.volume = savedVolume;
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

    public void BalloonPopAudioClip() 
    {
        Instance.audioSource.PlayOneShot(balloonPopAudioClip);
    }
    
    public void MinigameMusic() 
    {
        Instance.audioSource.PlayOneShot(MinigameMusicAudioClip);
    }

    public void MenuMusic()
    {
        Instance.audioSource.PlayOneShot(MenuMusicAudioClip);
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;

        PlayerPrefs.SetFloat(GAME_VOLUME_KEY, volume);
    }

}
