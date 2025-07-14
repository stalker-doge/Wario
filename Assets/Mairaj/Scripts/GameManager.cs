using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private OpponentNameListSO opponentNameList;

    private Player user;
    private Player opponent;
    private string levelTitle;
    private SceneType sceneType;
    private GameMode gameMode;
    private GameAIBase currentGameAI;
    [SerializeField]
    private int totalRounds = 0;
    private int currentRounds = 0;
    [SerializeField]
    private float nextGameStartingIn;
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

    public string GameName { get { return gameMode.ToString(); } }

    public int CurrentRoundNumber
    {
        get { return currentRounds; }
        set { currentRounds = value; }
    }

    public int TotalRounds
    {
        get { return totalRounds; }
        set { totalRounds = value; }
    }

    public float NextGameStartsIn
    {
        get { return nextGameStartingIn; }
    }
    public GameMode CurrentGameMode { get => gameMode;
        private set
        {
            gameMode = value;
        }
    }

    public void InitializeGame()
    {
        user = new Player();
        opponent = new Player();
        user.SetPlayerName("You");
        opponent.SetPlayerName(GetRandomOpponentName(opponentNameList));
        user.PlayerWins = 0;
        opponent.PlayerWins = 0;
        currentRounds = 0;
    }

    public OpponentNameListSO GetOpponentNamesList()
    {
        return opponentNameList;
    }

    public void UpdateScoreAndLoadScene()
    {
        int halfRounds = TotalRounds / 2;
        Debug.Log("XYZ Rounds " + halfRounds);

        if (User.PlayerWins > halfRounds || Opponent.PlayerWins > halfRounds)
        {
            Debug.Log("XYZ someone wins the game!");
            SceneManager.LoadScene(SceneDatabaseManager.Instance.GetSceneString(SceneType.MPWinLoss));
        }
        else if (CurrentRoundNumber < TotalRounds)
        {
            Debug.Log("XYZ Next round...");
            CurrentRoundNumber++;
            SceneManager.LoadScene(SceneDatabaseManager.Instance.GetSceneString(SceneType.MPGameTransition));
        }
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
                TrajectoryPredictor.IsEligibleToShoot = true;
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
