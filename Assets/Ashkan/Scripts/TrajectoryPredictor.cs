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

    private static bool isEligibleToShoot = true;

    public static bool IsEligibleToShoot { get => isEligibleToShoot;
        set { 
            isEligibleToShoot=value;
        } 
    }

    [SerializeField]
    private PlayerType playerType;

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
        if (GameManager.Instance.CurrentGameMode == GameMode.SinglePlayer)
        {
            DrawTrajectoryUser();
        } 
        else if (GameManager.Instance.CurrentGameMode == GameMode.Online)
        {
            if (playerType == PlayerType.mUser)
            {
                DrawTrajectoryUser();
            }
            else
            {
                DrawTrajectoryAI();
            }
        }   
    }

    void DrawTrajectoryAI()
    {
        Vector2 position = shootPoint.position;
        Vector2 velocity = shootPoint.right * shootForce;

        bool hasHitOnce = false;

        for (int i = 0; i < dots.Count; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(position, velocity.normalized, stepDistance, collisionLayer);

            if (hit.collider != null)
            {
                position = hit.point;
                velocity = Vector2.Reflect(velocity, hit.normal);
                position += velocity.normalized * 0.01f;

                hasHitOnce = true;

                for (int j = i + 1; j < dots.Count; j++)
                {
                    dots[j].SetActive(false);
                }

                // Optional perfect shot logic
                if (GameManager.Instance.IsTakingAPerfectShot())
                {
                    //Debug.Log("XYZ IsTakingAPerfectShot " + isEligibleToShoot);
                    if (hit.collider.gameObject.CompareTag("Goal") && IsEligibleToShoot)
                    {
                        GameManager.Instance.ChargeAndShoot(gameObject);
                    }
                }
            }
            else
            {
                position += velocity.normalized * stepDistance;
                dots[i].transform.position = position;
                dots[i].SetActive(true);

                SpriteRenderer sr = dots[i].GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    Color color = sr.color;
                    color.a = hasHitOnce ? 0f : 1f; // Transparent after first hit
                    sr.color = color;
                }
            }
        }
    }


    void DrawTrajectoryUser()
    {
        Vector2 position = shootPoint.position;
        Vector2 velocity = shootPoint.right * shootForce;

        for (int i = 0; i < dotCount; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(position, velocity.normalized, stepDistance, collisionLayer);

            if (hit.collider != null)
            {
                //Debug.Log("XYZ " + hit.collider.name);
                position = hit.point;
                dots[i].transform.position = position;
                dots[i].SetActive(true);
                for (int j = i + 1; j < dots.Count; j++)
                {
                    dots[j].SetActive(false);
                }
                break;
            }
            else
            {
                position += velocity.normalized * stepDistance;
                dots[i].transform.position = position;
                dots[i].SetActive(true);
            }
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