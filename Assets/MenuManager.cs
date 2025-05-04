using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
  
  public GameObject CreditPage;
  
  
  public void CreditPageSwitch()
  {
    if (CreditPage.activeSelf)
    {
      CreditPage.SetActive(false);
    }
    else
    {
      CreditPage.SetActive(true);
    }
  }

  public void LeaderBoardswitch()
  {
    if (SceneManager.GetActiveScene().name == "MainMenu")
    {
      SceneManager.LoadScene("LeaderBoard");
    }
    else if (SceneManager.GetActiveScene().name == "LeaderBoard")
    {
      SceneManager.LoadScene("MainMenu");
    }
  }
}
