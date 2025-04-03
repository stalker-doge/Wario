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
        //if there are no scenes in the array, add all scenes except the current scene
        if (scenes.Length == 0)
        {
            //Get all scenes
            scenes = new string[UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings - 1];
            int index = 0;
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                if (sceneName != UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
                {
                    scenes[index] = sceneName;
                    index++;
                }
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
        //Load scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
