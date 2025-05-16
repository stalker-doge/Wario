using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{

    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private AudioClip musicClip1;
    [SerializeField]
    private AudioClip musicClip2;

    public static MusicManager Instance { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of DifficultyManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == SceneDatabaseManager.Instance.GetSceneString(SceneType.MainMenu))
        {
            musicSource.clip = musicClip1;

        }
        else
        {
            musicSource.clip=musicClip2;
        }
        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }
}
