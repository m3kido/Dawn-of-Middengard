using UnityEngine;

public class Player
{
    public string Name;
    public bool Lost = false;
    public EColors Color;
    public ETeams TeamSide;
    public Captain Captain;
    public int Gold=0;
    
    
    public Player(string Name,int Color,int TeamSide,Captain Captain)
    {
       this.Name = Name;
       this.Color = (EColors)Color;
       this.TeamSide = (ETeams)TeamSide;
       this.Captain = Captain;
    }
}
