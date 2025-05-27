using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadAsyncMainMenu());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator LoadAsyncMainMenu()
    {
        AsyncOperation asyncload = SceneManager.LoadSceneAsync("Main Menu");
        while(!asyncload.isDone)
        {
            yield return null;
        }
    }
}
