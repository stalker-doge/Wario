using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManagerAI : MonoBehaviour
{
    public static DifficultyManagerAI Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


}
