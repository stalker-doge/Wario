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

    [SerializeField]
    private float curtainAnimTimer =  0.5f;

    private string sceneName;

    private bool isSwitchingScene = false;


    [SerializeField]
    private int gamesToPlay = 5;

    [SerializeField]
    private bool difficultyIncreased = false;

    private bool increasedDifficulty = false;

    [SerializeField]
    private GameObject difficultyText;

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
            difficultyIncreased = false;
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

            //increments the games played counter only once per scene switch
          if (!increasedDifficulty && TimerManager.Instance && TimerManager.Instance.isPaused)
            {
                increasedDifficulty = true;
                DifficultyManager.Instance.gamesPlayed++;
                if (DifficultyManager.Instance.gamesPlayed >= gamesToPlay)
                {
                    DifficultyManager.Instance.IncreaseDifficulty();
                    difficultyText.SetActive(true);
                }
            }
            if (timer > timeToWait)
            {
                //gives the player a score based on the time left
                if (TimerManager.Instance)
                {
                    TimerManager.Instance.isPaused = true;
                }
                else
                {
                    Debug.LogError("TimerManager not found in the scene.");
                }
               

                if (!isSwitchingScene)
                {
                    isSwitchingScene = true;
                    CurtainAnimController.Instance?.AnimateTowardsCenter(curtainAnimTimer, () => {
                        SwitchScene(sceneName);
                    });
                }
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
        SceneManager.LoadScene(SceneDatabaseManager.Instance?.GetSceneString(SceneType.Loading));
        GameManager.Instance.SetGameMode(GameMode.SinglePlayer);
    }

    public void OnOnlinePlayPressed()
    {
        SceneManager.LoadScene(SceneDatabaseManager.Instance?.GetSceneString(SceneType.MPGameSelection));
        GameManager.Instance.SetGameMode(GameMode.Online);
    }

    public void OnAimAndShootPressed()
    {
        SceneManager.LoadScene(SceneDatabaseManager.Instance?.GetSceneString(SceneType.MPOpponentSelection));
        GameManager.Instance.SetCurrentGame(GameType.AimShoot);
        GameManager.Instance.InitializeGame();
        GameManager.Instance.LevelTitle = "Hardcoded";
        GameManager.Instance.SceneToLoad = SceneType.AimAndShootOnline;
    }

    public void SwitchScene(string sceneName)
    {
        //Load scene
        SceneManager.LoadScene(sceneName);
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
            case "Aim and Shoot 1":
                localizedLevelName.StringReference.TableEntryReference = "AimAndShoot_Title";
                break;
            case "Aim and Shoot 2":
                localizedLevelName.StringReference.TableEntryReference = "AimAndShoot_Title";
                break;
            case "Aim and Shoot 3":
                localizedLevelName.StringReference.TableEntryReference = "AimAndShoot_Title";
                break;
            case "Aim&Shoot":
                localizedLevelName.StringReference.TableEntryReference = "AimAndShoot_Title";
                break;
            case "Hole in One":
                localizedLevelName.StringReference.TableEntryReference = "AimAndShoot_Title";
                break;
            case "GyroscopGame":
                localizedLevelName.StringReference.TableEntryReference = "RollingTheBall_Title";
                break;
            case "Fill The Gap":
                localizedLevelName.StringReference.TableEntryReference = "FillTheGap_Title";
                break;
            case "Fill The GapNewVariant":
                localizedLevelName.StringReference.TableEntryReference = "FillTheGap_Title";
                break;
            case "Aim&ShootOnline":
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
