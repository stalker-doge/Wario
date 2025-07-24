using System.Collections.Generic;
using UnityEngine;

public class BallTrajectory : MonoBehaviour
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
        for (int i = 0; i < dotCount; i++)
        {
            GameObject dot = Instantiate(dotPrefab, transform);
            dot.SetActive(false);
            dots.Add(dot.transform);
        }
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
}