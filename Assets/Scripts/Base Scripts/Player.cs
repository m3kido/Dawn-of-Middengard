// Class to represent the player
public class Player
{
    public string Name;
    public bool Lost = false;
    public ETeamColors Color;
    public ETeams TeamSide;
    public Captain PlayerCaptain;
    public int Gold = 0;
    
    // Player constructor
    public Player(string name, ETeamColors color, ETeams teamSide, Captain captain)
    {
       Name = name;
       Color = color;
       TeamSide = teamSide;
       PlayerCaptain = captain;
    }
}
