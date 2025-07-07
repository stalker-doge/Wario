using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject collisionAnimationObject;
    public Transform shootPoint;
    public ArrowController arrow;

    public float bulletSpeed = 10f;

    public int collisionCount = 0;
    public int collisionCountMax = 4;

    public int bulletLimit = 0;

    [SerializeField]
    private PlayerType playerType;

    public static System.Action ShootBulletLogicAICallback = null;

    //private static System.Action DestroyAllBulletsAICallback = null;

    private void Awake()
    {
        if (GameManager.Instance.CurrentGameMode == GameMode.Online)
        {
            if (playerType == PlayerType.mAI)
            {
                ShootBulletLogicAICallback += ShootBulletLogic;
            }
            //DestroyAllBulletsAICallback += OnBulletDestroy;
        }
    }

    //private void OnBulletDestroy()
    //{
    //    if (gameObject != null)
    //        Destroy(gameObject);
    //}

    private void Start()
    {
        bulletLimit = 0;
    }

    public void InitializePlayerType (PlayerType playerType)
    {
        this.playerType = playerType;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        //    Vector2 dir = arrow.GetDirection();

        //    bullet.GetComponent<Rigidbody2D>().AddForce(dir * bulletSpeed, ForceMode2D.Impulse);
        //}

        if (playerType == PlayerType.mUser)
        {
            if (GameManager.Instance.CurrentGameMode == GameMode.SinglePlayer)
            {
                if (Input.GetMouseButtonUp(0) && !TimerManager.Instance.winloseState)
                {
                    ShootBulletLogic();
                }
            } else if (GameManager.Instance.CurrentGameMode == GameMode.Online)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    ShootBulletLogic();
                }
            }
            
        } 
        //else if (playerType == PlayerType.mAI)
        //{
            //Invoke("ShootBulletLogic", 2f);
        //}
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            GameObject effect = Instantiate(collisionAnimationObject, transform.position, Quaternion.identity);
            Destroy(effect, 0.3f);
        }

        if (GameManager.Instance.CurrentGameMode == GameMode.SinglePlayer)
        {
            if (other.gameObject.CompareTag("Goal") && !TimerManager.Instance.winloseState)
            {
                SoundManager.Instance.MiniGameCompleteAudioClip();
                StartCoroutine(ScoreManager.Instance?.GameComplete());
                other.gameObject.SetActive(false);
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                Destroy(gameObject, 1.3f);
            }
        } else if (GameManager.Instance.CurrentGameMode == GameMode.Online)
        {
            if (other.gameObject.CompareTag("Goal"))
            {
                SoundManager.Instance.MiniGameCompleteAudioClip();
                if (playerType == PlayerType.mUser)
                {
                    GameManager.Instance.User.PlayerWins++;
                } else if (playerType == PlayerType.mAI)
                {
                    GameManager.Instance.Opponent.PlayerWins++;
                }

                other.gameObject.SetActive(false);
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                Destroy(gameObject, 1.3f);

                GameManager.Instance.UpdateScoreAndLoadScene();
            }
            //SoundManager.Instance.MiniGameCompleteAudioClip();
            //StartCoroutine(ScoreManager.Instance?.GameComplete());
            //other.gameObject.SetActive(false);
            //gameObject.GetComponent<SpriteRenderer>().enabled = false;
            //Destroy(gameObject, 1.3f);
        }
        
    }

    public void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        Vector2 dir = arrow.GetDirection();

        bullet.GetComponent<Rigidbody2D>().AddForce(dir * bulletSpeed, ForceMode2D.Impulse);
    }

    public void ShootBulletLogic()
    {
        if (bulletLimit < 3)
        {
            bulletLimit += 1;
            SoundManager.Instance.ShootAudioClip();

            if (shootPoint != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
                Vector2 dir = arrow.GetDirection();
                bullet.GetComponent<Bullet>()?.InitializePlayerType(playerType);

                if (bullet != null)
                    bullet.GetComponent<Rigidbody2D>().AddForce(dir * bulletSpeed, ForceMode2D.Impulse);
            }

        }
        else
        {
            // SoundManager.Instance.CardMismatchAudioClip();
        }
        //ShootBullet();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance.CurrentGameMode == GameMode.Online)
        {
            //DestroyAllBulletsAICallback?.Invoke();
            //DestroyAllBulletsAICallback -= OnBulletDestroy;
            if (playerType == PlayerType.mAI)
            {
                ShootBulletLogicAICallback -= ShootBulletLogic;
            }
        }
    }
}

public enum PlayerType
{
    mUser,
    mAI
}