using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHandler : MonoBehaviour
{

    public enum ThemeType { Light, Dark }
    public enum IconType { Swipe, Drag, Tap }
    
    [SerializeField] private ThemeType theme;
    [SerializeField] private IconType iconType;
    [SerializeField] private GameObject Swipe, SwipeBlack, Drag, DragBlack, Tap, TapBlack;
    [SerializeField] private bool test;

    private bool tutorialActive = false;

    private void Start()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (test)
        {
            gameObject.SetActive(true);
            tutorialActive = true;

        }
        else
        {
            if (!PlayerPrefs.HasKey("TutorialSeen_" + sceneName))
            {
                gameObject.SetActive(true);
                tutorialActive = true;
                PlayerPrefs.SetInt("TutorialSeen_" + sceneName, 1);
                PlayerPrefs.Save();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        
        SetIconByThemeAndType();
        
    }
    
    void Update()
    {
        if (tutorialActive && Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);
            tutorialActive = false;
        }
    }
    
    private void SetIconByThemeAndType()
    {
        Swipe.SetActive(false);
        SwipeBlack.SetActive(false);
        Drag.SetActive(false);
        DragBlack.SetActive(false);
        Tap.SetActive(false);
        TapBlack.SetActive(false);

        switch (iconType)
        {
            case IconType.Swipe:
                if (theme == ThemeType.Dark)
                    SwipeBlack.SetActive(true);
                else
                    Swipe.SetActive(true);
                break;

            case IconType.Drag:
                if (theme == ThemeType.Dark)
                    DragBlack.SetActive(true);
                else
                    Drag.SetActive(true);
                break;

            case IconType.Tap:
                if (theme == ThemeType.Dark)
                    TapBlack.SetActive(true);
                else
                    Tap.SetActive(true);
                break;
        }
    }
    
 


}
