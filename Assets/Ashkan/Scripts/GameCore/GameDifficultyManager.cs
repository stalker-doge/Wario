    using UnityEngine;

public class GameDifficultyManager : MonoBehaviour
{
    public DifficultyMode mode = DifficultyMode.Random;
    private int currentIndex = 0;

    public DifficultyLevel GetNextDifficulty()
    {
        if (mode == DifficultyMode.Random)
        {
            return (DifficultyLevel)Random.Range(0, 3);
        }
        else // Sorted mode
        {
            currentIndex++;
            if (currentIndex <= 5)
                return DifficultyLevel.Hard;
            else if (currentIndex <= 15)
                return DifficultyLevel.Medium;
            else
                return DifficultyLevel.Easy;
        }
    }

    public void ResetProgress()
    {
        currentIndex = 0;
    }
}