using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MazeDragPlayer : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 targetPosition;
    private bool isDragging = false;
    private bool isTouchingWall = false;

    public float moveSpeed = 10f;
    public float rotationSpeed = 5f;

    [SerializeField]
    private PlayerType playerType;

    private List<Transform> pathTransforms = null;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // this blocks Unity's physics from rotating the object
        targetPosition = transform.position;
    }

    void OnMouseDown()
    {
        isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    public void InitializePathTransformsAndPlayMove(List<Transform> pathTransforms)
    {
        this.pathTransforms = pathTransforms;
        GameManager.Instance.ExecuteAIMove(gameObject);
    }
    public void InitializePlayerType(PlayerType playerType)
    {
        this.playerType = playerType;

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        SphereCollider sphereCollider = GetComponent<SphereCollider>();

        if (spriteRenderer != null)
        {
            if (playerType == PlayerType.mUser)
            {
                spriteRenderer.color = Color.blue;
                gameObject.layer = LayerMask.NameToLayer("Default");

                int userLayer = gameObject.layer;
                int groundLayer = LayerMask.NameToLayer("Ground");
                Physics.IgnoreLayerCollision(userLayer, groundLayer, true);
            }
            else if (playerType == PlayerType.mAI)
            {
                spriteRenderer.color = Color.red;
                gameObject.layer = LayerMask.NameToLayer("Ground");
                Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;         // Disables physics simulation (no gravity or forces)
                    rb.useGravity = false;         // Just in case
                    rb.interpolation = RigidbodyInterpolation.None;
                    rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
                    rb.constraints = RigidbodyConstraints.FreezeAll; // Optional: freeze position & rotation
                }

            }
        }
        else
        {
            Debug.LogWarning("XYZ: No SpriteRenderer found in child objects.");
        }

        if (sphereCollider == null)
        {
            Debug.LogWarning("XYZ: No SphereCollider found on this object.");
        }
    }

    public void PlayMoveAI(float totalTime)
    {
        Debug.Log("XYZ PlayMoveAI Called");
        if (pathTransforms == null || pathTransforms.Count < 2)
        {
            Debug.LogWarning("XYZ: Not enough points to animate.");
            return;
        }

        int steps = pathTransforms.Count;
        float stepDuration = totalTime / steps;

        Sequence movementSequence = DOTween.Sequence();

        foreach (Transform point in pathTransforms)
        {
            movementSequence.Append(transform.DOMove(point.position, stepDuration).SetEase(Ease.Linear));
        }

        movementSequence.OnComplete(() => Debug.Log("XYZ: AI reached the end of the path."));
    }
    void Update()
    {
        if ((GameManager.Instance.CurrentGameMode == GameMode.SinglePlayer && !TimerManager.Instance.winloseState) || (GameManager.Instance.CurrentGameMode == GameMode.Online && playerType != PlayerType.mAI))
        {
            if (isDragging)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0f;
                targetPosition = mousePos;
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 newPosition = Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        Vector3 direction = newPosition - rb.position;

        // Rotate only if not touching wall
        if (direction.magnitude > 0.01f && !isTouchingWall)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        rb.MovePosition(newPosition);

       // rb.velocity = Vector3.ClampMagnitude(rb.velocity, moveSpeed);
    }

    // Detect collision with walls
    void OnCollisionEnter(Collision collision)
    {
   
        if (collision.gameObject.CompareTag("MazeWall"))
        {
            moveSpeed = 6;
            isTouchingWall = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("MazeWall"))
        {
            moveSpeed = 8;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.CompareTag("MazeGoal"))
        {
            Debug.Log("Maze Done");
            // You can also add effects, sounds, or next level logic here
            //SoundManager.Instance.MiniGameCompleteAudioClip();

            if (GameManager.Instance.CurrentGameMode == GameMode.SinglePlayer)
            {
                if (ScoreManager.Instance)
                {
                    StartCoroutine(ScoreManager.Instance.GameComplete());
                }
            } else if (GameManager.Instance.CurrentGameMode == GameMode.Online)
            {
                if (playerType == PlayerType.mUser)
                {
                    GameManager.Instance.User.PlayerWins++;
                }
                else if (playerType == PlayerType.mAI)
                {
                    GameManager.Instance.Opponent.PlayerWins++;
                }

                GameManager.Instance.UpdateScoreAndLoadScene();
            }
        }
    
    }
}