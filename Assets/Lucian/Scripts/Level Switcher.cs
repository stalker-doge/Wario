using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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

    private string sceneName;


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
            levelName.text = sceneName;
            timer += Time.deltaTime;
            if (timer > timeToWait)
            {
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

        foreach(string s in scenes)
        {
            Debug.Log("XYZ Scene " + s);
        }
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
}
