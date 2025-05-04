using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfTrajectory : MonoBehaviour
{

    [SerializeField]
    private GameObject dotPrefab;

    [SerializeField]
    private int dotCount = 30;
    [SerializeField]
    private float stepDistance = 0.2f;
    [SerializeField]
    private Transform shootPoint;
    [SerializeField]
    private float shootForce = 10f;
    [SerializeField]
    private LayerMask collisionLayer;

    private List<GameObject> dots = new List<GameObject>();
    private bool isAiming = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < dotCount; i++)
        {
            GameObject dot = Instantiate(dotPrefab, transform);
            dot.SetActive(false);
            dots.Add(dot);
        }
    }

    // Update is called once per frame
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

        if (Input.GetMouseButtonUp(0))
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

        //draws a trajectory, taking into account the gravity
        for (int i = 0; i < dotCount; i++)
        {
            // Calculate the position of the dot
            position += velocity * stepDistance;
            velocity += Physics2D.gravity * Time.deltaTime;
            // Set the position of the dot
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
