// Mairaj Muhammad -> 2415831
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadSceneAgainstClick : MonoBehaviour
{
    [SerializeField]
    private SceneType sceneType;
    public void OnButtonClick()
    {
        SceneManager.LoadScene(SceneDatabaseManager.Instance.GetSceneString(sceneType));
    }
}
