using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Components;

public class LevelSwitcher : MonoBehaviour
{
    // Start is called before the first frame update

    //Array of scenes
    [SerializeField]
    private string[] scenes;
    [SerializeField]
    private float timeToWait = 3;

    [SerializeField]
    private bool loading = false;

    [SerializeField]
    private TextMeshProUGUI levelName;

    // Localized String references
    [SerializeField]
    private LocalizeStringEvent localizedLevelName;

    private string sceneName;

    private int gamesPlayed = 0;


    private float timer = 0;
    void Start()
    {
        //Get the scenes from PlayerPrefs
        string scenesString = PlayerPrefs.GetString("Scenes", "");
        //if there are no scenes in PlayerPrefs, set the scenes to an empty array
        if (string.IsNullOrEmpty(scenesString))
        {
            scenes = new string[0];

        }
        else
        {
            //split the scenes string into an array
            scenes = scenesString.Split(',');
        }

        //if there are no scenes in the array, add all scenes except scenes in a folder called System
        if (scenes.Length == 0)
        {
            // Get all scenes in build settings
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            // Initialize the scenes array with the number of scenes in build settings
            for (int i = 0; i < sceneCount; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                // Check if the scene is in the "System" folder, if so, skip it and shrink the array

                if (scenePath.Contains("System"))
                {
                    continue;
                }
                // Get the scene name from the path
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                // Add the scene name to the array
                List<string> sceneList = new List<string>(scenes);
                sceneList.Add(sceneName);
                scenes = sceneList.ToArray();

            }
        }

        if (loading)
        {
            FindRandom();
        }
    }

    // Update is called once per frame
    void Update()
    {


        if (loading)
        {
            //levelName.text = sceneName;
            SetLevelTitle(sceneName);
            timer += Time.deltaTime;
            if (timer > timeToWait)
            {
                //gives the player a score based on the time left
                TimerManager timerManager = FindObjectOfType<TimerManager>();
                if (timerManager != null)
                {
                    TimerManager.Instance.isPaused = false;
                }
                else
                {
                    Debug.LogError("TimerManager not found in the scene.");
                }
                //increment the games played
                gamesPlayed++;
                //if games played is greater than 5, reset the games played and up the difficulty
                if (gamesPlayed > 5)
                {
                    gamesPlayed = 0;
                    //up the difficulty
                    DifficultyManager.Instance.IncreaseDifficulty();
                }
                SwitchScene(sceneName);
            }
        }
    }

    public void FindRandom()
    {
        //Get random scene
        int randomIndex = Random.Range(0, scenes.Length);
        sceneName = scenes[randomIndex];
        //deletes the scene from the array
        List<string> sceneList = new List<string>(scenes);
        sceneList.RemoveAt(randomIndex);
        scenes = sceneList.ToArray();
        //saves the scenes to the PlayerPrefs
        PlayerPrefs.SetString("Scenes", string.Join(",", scenes));

        SetLevelTitle(sceneName);
    }

    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");
    }

    public void SwitchScene(string sceneName)
    {
        //Load scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    // Function to set the correct localized string for each level
    public void SetLevelTitle(string level)
    {
        switch (level)
        {
            case "MathGame":
                localizedLevelName.StringReference.TableEntryReference = "MathGame_Title";
                break;
            case "Pop the Balloons":
                localizedLevelName.StringReference.TableEntryReference = "PopTheBalloons_Title";
                break;
            case "Match the Cards":
                localizedLevelName.StringReference.TableEntryReference = "MatchTwoCards_Title";
                break;
            case "MazeGame":
                localizedLevelName.StringReference.TableEntryReference = "MazeGame_Title";
                break;
            case "Aim and Shoot":
                localizedLevelName.StringReference.TableEntryReference = "AimAndShoot_Title";
                break;
            default:
                Debug.LogWarning("Level not found in the localization table.");
                return;
        }

        // Refresh the string to display the translated text
        localizedLevelName.RefreshString();
    }

}
