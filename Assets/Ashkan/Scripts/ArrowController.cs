using DG.Tweening;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float rotationSensitivity = 0.3f;  // Sensitivity based on distance
    public float minAngle = 0f;               // Minimum allowed angle (left limit)
    public float maxAngle = 180f;             // Maximum allowed angle (right limit)
    public float minDistanceThreshold = 1f;   // Minimum distance before offset is applied
    public float offsetDistance = 3f;       // Vertical offset added when input is too close

    public GameObject threeRemaining;         // UI for 3 remaining
    public GameObject twoRemaining;           // UI for 2 remaining
    public GameObject oneRemaining;           // UI for 1 remaining

    [SerializeField] private int shootCount = 0; // Counts how many times player has shot
    [SerializeField] private PlayerType playerType;

    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, 90);

        // Make sure all indicators are active at the beginning
        threeRemaining?.SetActive(true);
        twoRemaining?.SetActive(true);
        oneRemaining?.SetActive(true);
    }

    void Update()
    {
        if (TimerManager.Instance && TimerManager.Instance.winloseState)
            return;

        if (playerType == PlayerType.mUser)
        {
            Vector3 inputPosition = Vector3.zero;
            bool isTouching = false;

            // Mouse
            if (Input.GetMouseButton(0))
            {
                inputPosition = Input.mousePosition;
                isTouching = true;
            }

            // Touch
            if (Input.touchCount > 0)
            {
                inputPosition = Input.GetTouch(0).position;
                isTouching = true;
            }

            if (isTouching)
            {
                RotateTowardsInput(inputPosition);
            }

            // Shoot on release
            if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                Debug.Log("XYZ HandleShot Called");
                HandleShot();
            }
        } else if (playerType == PlayerType.mAI)
        {
            GameManager.Instance.ExecuteAIMove(gameObject);
            //if (!DOTween.IsTweening(transform) && !hasAIFoundTarget)
            //{
            //    hasAIFoundTarget = true;
            //    FindTarget();
            //}
        }
    }

    void RotateTowardsInput(Vector3 inputScreenPosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(inputScreenPosition.x, inputScreenPosition.y, Camera.main.WorldToScreenPoint(transform.position).z));
        Vector2 direction = (Vector2)(worldPosition - transform.position);

        // If touch/mouse is too close, apply upward offset
        if (direction.magnitude < minDistanceThreshold)
        {
            worldPosition += Vector3.up * offsetDistance;
            direction = (Vector2)(worldPosition - transform.position);
        }

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (targetAngle < 0) targetAngle += 360f;
        targetAngle = Mathf.Clamp(targetAngle, minAngle, maxAngle);

        transform.rotation = Quaternion.Euler(0, 0, targetAngle);
    }

    public Vector2 GetDirection()
    {
        float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }

    public void HandleShot()
    {
        shootCount++;
        if (shootCount == 1 && threeRemaining != null)
            threeRemaining.SetActive(false);
        else if (shootCount == 2 && twoRemaining != null)
            twoRemaining.SetActive(false);
        else if (shootCount == 3 && oneRemaining != null)
            oneRemaining.SetActive(false);
    }

    public bool HasFiredAllShots()
    {
        return oneRemaining.activeInHierarchy == false;
    }
}
