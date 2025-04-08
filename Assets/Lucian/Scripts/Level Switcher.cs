using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitcher : MonoBehaviour
{
    // Start is called before the first frame update

    //Array of scenes
    [SerializeField]
    private string[] scenes;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchRandom()
    {
        //Get random scene
        int randomIndex = Random.Range(0, scenes.Length);
        string sceneName = scenes[randomIndex];
        //deletes the scene from the array
        List<string> sceneList = new List<string>(scenes);
        sceneList.RemoveAt(randomIndex);
        scenes = sceneList.ToArray();
        //saves the scenes to the PlayerPrefs
        PlayerPrefs.SetString("Scenes", string.Join(",", scenes));
        //Load scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void SwitchScene(string sceneName)
    {
        //Load scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
