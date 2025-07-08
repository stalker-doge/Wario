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
    Image cross1;
    [SerializeField]
    Image cross2;
    [SerializeField]
    Image cross3;
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
                cross3.gameObject.SetActive(true);
                break;
            case 1:
                Heart3.sprite = HeartEmpty;
                Heart2.sprite = HeartEmpty;
                cross3.gameObject.SetActive(true);
                cross2.gameObject.SetActive(true);
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
