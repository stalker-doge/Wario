using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.SetInt("Lives", 3);
        PlayerPrefs.SetInt("Difficulty", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
