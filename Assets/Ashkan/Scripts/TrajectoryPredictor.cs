using UnityEngine;
using System.Collections.Generic;

public class TrajectoryPredictor : MonoBehaviour
{
    public GameObject dotPrefab;
    public int dotCount = 30;
    public float stepDistance = 0.2f;
    public Transform shootPoint;
    public float shootForce = 10f;
    public LayerMask collisionLayer;

    private List<GameObject> dots = new List<GameObject>();
    private bool isAiming = false;

    void Start()
    {
        for (int i = 0; i < dotCount; i++)
        {
            GameObject dot = Instantiate(dotPrefab, transform);
            dot.SetActive(false);
            dots.Add(dot);
        }
    }

    void Update()
    {
        // Start aiming when holding left click
        if (Input.GetMouseButton(0))
        {
            isAiming = true;
        }
        else
        {
            isAiming = false;
        }

        // Hide dots when pressing Space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HideDots();
        }

        // Draw trajectory if aiming
        if (isAiming)
        {
            DrawTrajectory();
        }
    }

    void DrawTrajectory()
    {
        Vector2 position = shootPoint.position;
        Vector2 velocity = shootPoint.right * shootForce;

        for (int i = 0; i < dotCount; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(position, velocity.normalized, stepDistance, collisionLayer);
            if (hit.collider != null)
            {
                position = hit.point;
                velocity = Vector2.Reflect(velocity, hit.normal);
                position += velocity.normalized * 0.01f;
            }
            else
            {
                position += velocity.normalized * stepDistance;
            }

            dots[i].transform.position = position;
            dots[i].SetActive(true);
        }
    }

    void HideDots()
    {
        for (int i = 0; i < dots.Count; i++)
        {
            dots[i].SetActive(false);
        }
    }
}