using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Services.Core;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI score;
    //[SerializeField]
    //private TextMeshProUGUI lives;

    [SerializeField]
    private TextMeshProUGUI highScore;

    [SerializeField]
    private TextMeshProUGUI lastScore;
    // Start is called before the first frame update

    // For localization purpose
    private LocalizeStringEvent highScoreEvent;
    private LocalizeStringEvent lastScoreEvent;
    private LocalizeStringEvent livesCountEvent;
    private LocalizeStringEvent currentScoreEvent;

    void Start()
    {

        //checks if the scene is the end scene
        if (SceneManager.GetActiveScene().name == SceneDatabaseManager.Instance.GetSceneString(SceneType.EndScene))
        {
            //resets the score and lives
            PlayerPrefs.SetInt("Score", 0);
            PlayerPrefs.SetInt("Lives", 3);
            //highScore.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0).ToString();
            //lastScore.text= "Score: " + PlayerPrefs.GetInt("LastScore", 0).ToString();

            // Get the LocalizeStringEvent components
            highScoreEvent = highScore.GetComponent<LocalizeStringEvent>();
            lastScoreEvent = lastScore.GetComponent<LocalizeStringEvent>();

            // Apply them to the localized UI
            SetSmartInt(currentScoreEvent, "targetValue", PlayerPrefs.GetInt("Score", 0));
            SetSmartInt(highScoreEvent, "targetValue", PlayerPrefs.GetInt("HighScore", 0));
            SetSmartInt(lastScoreEvent, "targetValue", PlayerPrefs.GetInt("LastScore", 0));
            return;
        }
        //checks if the score and lives texts are not null
        if (score == null)
        {
            return;
        }

        //gets the score and lives from PlayerPrefs
        //score.text = "Score: " + PlayerPrefs.GetInt("Score", 0).ToString();
        //lives.text = "Lives: " + PlayerPrefs.GetInt("Lives", 3).ToString();
        currentScoreEvent = score.GetComponent<LocalizeStringEvent>();
        //livesCountEvent = lives.GetComponent<LocalizeStringEvent>();
        SetSmartInt(currentScoreEvent, "targetValue", PlayerPrefs.GetInt("Score", 0));
        //SetSmartInt(livesCountEvent, "targetValue", PlayerPrefs.GetInt("Lives", 3));
        //if lives is less than or equal 0, immediately switch to the end scene
        if (PlayerPrefs.GetInt("Lives", 3) <= 0)
        {
            SceneManager.LoadScene(SceneDatabaseManager.Instance.GetSceneString(SceneType.EndScene));
        }
    }
    private void SetSmartInt(LocalizeStringEvent localizeEvent, string variableName, int value)
    {
        if (localizeEvent == null) return;

        if (!localizeEvent.StringReference.ContainsKey(variableName))
        {
            localizeEvent.StringReference.Add(variableName, new IntVariable());
        }

        var variable = localizeEvent.StringReference[variableName] as StringVariable;
        variable.Value = value.ToString();
        localizeEvent.RefreshString();
    }
}
