using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private OpponentNameListSO opponentNameList;

    private Player user = new Player();
    private Player opponent = new Player();
    private string levelTitle;
    private SceneType sceneType;

    private GameMode gameMode;

    private GameAIBase currentGameAI;
    public static GameManager Instance { get; private set; }

    public Player User { get { return user; } }

    public Player Opponent { get { return opponent; } }

    public string LevelTitle { 
        get { return levelTitle; }
        set { levelTitle = value; }
    }

    public SceneType SceneToLoad {
        get { return sceneType; }
        set { sceneType = value; } 
    }

    public string GameName {  get { return gameMode.ToString(); } }
    public GameMode CurrentGameMode { get => gameMode;
        private set
        {
            gameMode = value;
        } 
    }

    public void InitializePlayers()
    {
        user.SetPlayerName("You");
        opponent.SetPlayerName(GetRandomOpponentName(opponentNameList));
        user.PlayerWins = 0;
        opponent.PlayerWins = 0;
    }

    public string GetRandomOpponentName(OpponentNameListSO nameListSO)
    {
        int index = Random.Range(0, nameListSO.opponentNames.Count);
        return nameListSO.opponentNames[index];
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
