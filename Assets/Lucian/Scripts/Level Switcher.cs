using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

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

        //if there are no scenes in the array, add all scenes except scenes in a folder called systen
        if (scenes.Length == 0)
        {
            //Get all scenes in the project
            string[] allScenes = UnityEditor.EditorBuildSettingsScene.GetActiveSceneList(UnityEditor.EditorBuildSettings.scenes);
            List<string> sceneList = new List<string>();
            foreach (string scene in allScenes)
            {
                //if the scene is not in a folder called system, add it to the list
                if (!scene.Contains("System"))
                {
                    Debug.Log("Adding scene: " + scene);
                    sceneList.Add(scene);
                }
            }
            scenes = sceneList.ToArray();
            //saves the scenes to the PlayerPrefs
            PlayerPrefs.SetString("Scenes", string.Join(",", scenes));

            //prints the scenes to the console
            foreach (string scene in scenes)
            {
                Debug.Log("Scene: " + scene);
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
