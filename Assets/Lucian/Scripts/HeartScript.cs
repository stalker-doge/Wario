using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartScript : MonoBehaviour
{

    [SerializeField]
    int lives;

    [SerializeField]
    Image Heart1;
    [SerializeField]
    Image Heart2;
    [SerializeField]
    Image Heart3;

    [SerializeField]
    Sprite HeartEmpty;
    // Start is called before the first frame update
    void Start()
    {
        lives = PlayerPrefs.GetInt("Lives", 3);
        switch (lives)
        {
            case 2: 
                Heart3.sprite = HeartEmpty;
                break;
            case 1:
                Heart3.sprite = HeartEmpty;
                Heart2.sprite = HeartEmpty;
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
