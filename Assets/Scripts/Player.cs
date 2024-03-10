public class Player
{
    public string Name;
    public bool Dead = false;
    public int TeamSide;
    public int Captain;
    public int Gold=0;
    
    
    public Player(string name,int teamSide,int captain)
    {
        Name = name;
        TeamSide = teamSide;
        Captain = captain;
    }
}