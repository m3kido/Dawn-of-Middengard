using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;

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
//this handles the game logic
public class GameManager : MonoBehaviour
{
    public List<Player> Players;
    public int PlayerTurn = 0;
    public bool GameEnded=false;
    void Start()
    {
        Players = new List<Player>
        {
            new("Andrew", 0, 0),
            new("Freya", 1, 0)
        };
    }
   
    void Update()
    {
        
    }

 
 



}
