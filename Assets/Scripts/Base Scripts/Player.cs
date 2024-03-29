﻿using UnityEngine;

public class Player
{
    public string Name;
    public bool Lost = false;
    public EPlayerColors Color;
    public ETeams TeamSide;
    public Captain Captain;
    public int Gold = 0;
    
    // Player constructor
    public Player(string name, EPlayerColors color, ETeams teamSide, Captain captain)
    {
       Name = name;

       Color = color;
       TeamSide = teamSide;

       Captain = captain;
    }
}
