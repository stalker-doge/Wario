using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathMinigame : MonoBehaviour
{
    public void StartGame(DifficultyLevel difficulty)
    {
        
    }

    void Start()
    {
        DifficultyLevel difficulty = (DifficultyLevel)PlayerPrefs.GetInt("DifficultyLevel", 0);
        StartGame(difficulty);
    }
}
