using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameMode gameMode;

    private GameAIBase currentGameAI;
    public static GameManager Instance { get; private set; }
    public GameMode CurrentGameMode { get => gameMode;
        private set
        { 
            gameMode = value;
        } 
    }

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

    public void SetCurrentGame(GameType gameType)
    {
        switch (gameType)
        {
            //case GameType.FillTheGap:
            //    currentGameAI = new FillTheGapGameAI();
            //    break;
            //case GameType.Maze:
            //    currentGameAI = new MazeGameAI();
            //    break;
            //case GameType.BalloonPop:
            //    currentGameAI = new BalloonPopGameAI();
            //    break;
            case GameType.AimShoot:
                currentGameAI = new AimShootGameAI();
                break;
            //case GameType.SwipeBall:
            //    currentGameAI = new SwipeBallGameAI();
            //    break;
            //case GameType.Math:
            //    currentGameAI = new MathGameAI();
            //    break;
            //case GameType.MatchCards:
            //    currentGameAI = new MatchCardsGameAI();
            //    break;
            default:
                currentGameAI = null;
                Debug.LogWarning("XYZ Unknown game type selected.");
                break;
        }
    }

    public void SetGameMode(GameMode gameMode)
    {
        CurrentGameMode = gameMode;
    }

    public void ExecuteAIMove(GameObject game)
    {
        currentGameAI?.PlayAIMove(game);
    }

    public void ChargeAndShoot(GameObject game)
    {
        currentGameAI?.ChargeAndShoot(game);
    }

    public bool IsTakingAPerfectShot()
    {
        return currentGameAI.IsTakingAPerfectShot;
    }
}
