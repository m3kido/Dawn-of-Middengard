public class Player
{
    public string Name;
    public bool Lost = false;
    public ETeamColors Color;
    public ETeams TeamSide;
    public Captain Captain;
    public int Gold;

    public bool IsCelesteActive { get; private set; } = false;

    // Player constructor
    public Player(string name, int color, int teamSide, Captain captain)
    {
        Name = name;

        Color = color;
        TeamSide = teamSide;

        Captain = captain;
    }
}
