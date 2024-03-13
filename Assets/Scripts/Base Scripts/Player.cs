using UnityEngine;

public class Player
{
    public string Name;
    public bool Lost = false;
    public ETeamColors Color;
    public ETeams TeamSide;
    public Captain Captain;
    public int Gold = 0;
    
    // Player constructor
    public Player(string name, int color, int teamSide, Captain captain)
    {
       Name = name;
       Color = (ETeamColors)color;
       TeamSide = (ETeams)teamSide;
       Captain = captain;
    }
}
