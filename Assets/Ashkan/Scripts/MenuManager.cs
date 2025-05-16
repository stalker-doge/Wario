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
    if (SceneManager.GetActiveScene().name == SceneDatabaseManager.Instance.GetSceneString(SceneType.MainMenu))
    {
      SceneManager.LoadScene(SceneDatabaseManager.Instance.GetSceneString(SceneType.Leaderboard));
    }
    else if (SceneManager.GetActiveScene().name == SceneDatabaseManager.Instance.GetSceneString(SceneType.Leaderboard))
    {
      SceneManager.LoadScene(SceneDatabaseManager.Instance.GetSceneString(SceneType.MainMenu));
    }
  }
}
