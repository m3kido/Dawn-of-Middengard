using UnityEngine;

public class Player
{
    public string Name;
    public bool Dead = false;
    public int TeamSide;
    public int Captain;
    public int Gold=0;
    
    
    public Player(string Name,int TeamSide,int Captain)
    {
       this.Name = Name;
       this.TeamSide = TeamSide;
       this.Captain = Captain;
    }
}
