
public class Player
{
    private string playerName;
    private int playerWins;
    public void SetPlayerName(string pName)
    {
        playerName = pName;
    }
    public int PlayerWins
    {
        get
            { return playerWins; }
        set
        {
            playerWins = value;
        }
    }
    public string PlayerName
    {
        get { return playerName; }
    }
}
