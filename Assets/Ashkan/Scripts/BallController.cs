using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject dustAnimationObject;
    public float minImpactForce = 2f; // Minimum collision force to trigger effect
    public float forceMultiplier = 10f;
    public float maxX = 1.5f;

    private Vector2 startTouchPos;
    private Vector2 endTouchPos;
    private Rigidbody2D rb;

    [SerializeField]
    private PlayerType playerType;

    private void Awake()
    {
        if (GameManager.Instance.CurrentGameMode == GameMode.Online)
        {
            gameObject.layer = LayerMask.NameToLayer("Ground");
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Ground"), gameObject.layer, true);
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void InitializeBallPlayer(PlayerType player)
    {
        this.playerType = player;
        if (player == PlayerType.mAI)
        {
            Color redColor = new Color32(0xFF, 0x00, 0x00, 0xFF);
            GetComponent<SpriteRenderer>().color = redColor;
        }
        else if (player == PlayerType.mUser)
        {
            Color blueColor = new Color32(0x00, 0x00, 0xFF, 0xFF);
            GetComponent<SpriteRenderer>().color = blueColor;
        }
    }

    void Update()
    {
        if (GameManager.Instance.CurrentGameMode == GameMode.SinglePlayer)
        {
            if (!TimerManager.Instance.winloseState)
                DetectSwipeInput();
        }
        else if (GameManager.Instance.CurrentGameMode == GameMode.Online)
        {
            if (playerType == PlayerType.mUser)
            {
                DetectSwipeInput();
            } else if (playerType == PlayerType.mAI)
            {
                GenerateSwipeInput();
            }
        }
        
    }

    private void GenerateSwipeInput()
    {
        // Debug.Log("XYZ GenerateSwipeInput Called");
        GameManager.Instance.ExecuteAIMove(gameObject);
    }

    void DetectSwipeInput()
    {
        // Debug.Log("XYZ DetectSwipeInput Called");
        // Touch input (mobile)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                startTouchPos = touch.position;

            if (touch.phase == TouchPhase.Ended)
            {
                endTouchPos = touch.position;
                ApplySwipeForce(endTouchPos - startTouchPos);
            }
        }

        // Mouse input (for editor)
        if (Input.GetMouseButtonDown(0))
            startTouchPos = Input.mousePosition;

        if (Input.GetMouseButtonUp(0))
        {
            endTouchPos = Input.mousePosition;
            ApplySwipeForce(endTouchPos - startTouchPos);
        }
    }

    void ApplySwipeForce(Vector2 swipe)
    {
        if (swipe.magnitude > 50f) // prevent weak swipes
        {
            swipe.Normalize();
            Vector2 force = swipe * forceMultiplier;
            rb.AddForce(force, ForceMode2D.Impulse); // Add impulse force
        }
    }

    public void ForceSwipeLeft()
    {
        Vector2 swipeLeft = new Vector2(-150f, 0f);
        ApplySwipeForce(swipeLeft);
    }

    public void ForceSwipeRight()
    {
        Vector2 swipeRight = new Vector2(150f, 0f);
        ApplySwipeForce(swipeRight);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (GameManager.Instance.CurrentGameMode == GameMode.SinglePlayer)
        {
            if (other.gameObject.CompareTag("Target") && !TimerManager.Instance.LosePage.activeSelf)
            {
                StartCoroutine(ScoreManager.Instance?.GameComplete());
            }
        } else if (GameManager.Instance.CurrentGameMode == GameMode.Online)
        {
            if (other.gameObject.CompareTag("Target"))
            {
                if (playerType == PlayerType.mUser)
                {
                    GameManager.Instance.User.PlayerWins++;
                } else if (playerType == PlayerType.mAI)
                {
                    GameManager.Instance.Opponent.PlayerWins++;
                }

                GameManager.Instance.UpdateScoreAndLoadScene();
            }
        }


        if (other.gameObject.CompareTag("Wall"))
        {
            // Check impact force
            if (other.relativeVelocity.magnitude > minImpactForce)
            {
                // Get collision contact point
                Vector3 contactPoint = other.GetContact(0).point;

                // Instantiate the effect at the contact point
                GameObject dust = Instantiate(dustAnimationObject, contactPoint, Quaternion.identity);

                // Destroy after 0.3 seconds
                Destroy(dust, 0.3f);

                SoundManager.Instance.ShootAudioClip();
            }
        }
    }
}