using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MiniGameAudioController : MonoBehaviour
{

    public List<string> miniGameSceneNames; // Add your mini-game scene names here
    public List<string> menuSceneName ; // Your menu scene name

    public AudioSource audioSource;
    private bool gameStartPlayed = false;

    void Awake()
    {
        if (FindObjectsOfType<MiniGameAudioController>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        audioSource = FindObjectOfType<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject menuMusic = GameObject.Find("SoundManager");

        if (miniGameSceneNames.Contains(scene.name))
        {
            if (!gameStartPlayed)
            {
                SoundManager.Instance.GameStartAudioClip();
                gameStartPlayed = true;
            }

            if (!audioSource.isPlaying || audioSource.clip != SoundManager.Instance.MinigameMusicAudioClip)
            {
                audioSource.clip = SoundManager.Instance.MinigameMusicAudioClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else if (menuSceneName.Contains(scene.name))
        {
            audioSource.Stop();
            gameStartPlayed = false;
            SoundManager.Instance.MenuMusic();
        }
    }
    
    
    
}