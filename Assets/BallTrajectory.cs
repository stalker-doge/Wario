using System;
using System.Collections.Generic;
using UnityEngine;

public class BallTrajectory : MonoBehaviour , IMiniGame
{
    public Rigidbody2D ballRigidbody;
    public Transform shootPoint;
    public GameObject dotPrefab;
    public int dotCount = 100;
    public float dotSpacing = 0.1f;

    public DragController dragController;

    private List<Transform> dots = new List<Transform>();

    void Start()
    {
        DifficultyLevel difficulty = (DifficultyLevel)PlayerPrefs.GetInt("DifficultyLevel");
        SetDifficulty(difficulty);
    }

    public void ShowTrajectory(Vector3 direction)
    {
        Vector3 force = direction * dragController.forceToAdd;
        Vector2 velocity = force / ballRigidbody.mass;

        for (int i = 0; i < dotCount; i++)
        {
            float t = i * dotSpacing;
            Vector3 pos = (Vector3)ballRigidbody.position 
                          + (Vector3)(velocity * t) 
                          + 0.5f * (Vector3)(Physics2D.gravity * ballRigidbody.gravityScale) * t * t;

            dots[i].position = pos;
            dots[i].gameObject.SetActive(true);
        }
    }

    public void HideTrajectory()
    {
        foreach (Transform dot in dots)
        {
            dot.gameObject.SetActive(false);
        }
    }

    public void SetDifficulty(DifficultyLevel difficulty)
    {
        if (difficulty == DifficultyLevel.Easy)
        {
            dotCount = 10;
        }
        else if (difficulty == DifficultyLevel.Medium)
        {
            dotCount = 5;
        }
        else if (difficulty == DifficultyLevel.Hard)
        {
            dotCount = 3;
        }
        
        for (int i = 0; i < dotCount; i++)
        {
            GameObject dot = Instantiate(dotPrefab, transform);
            dot.SetActive(false);
            dots.Add(dot.transform);
        }
    }
}