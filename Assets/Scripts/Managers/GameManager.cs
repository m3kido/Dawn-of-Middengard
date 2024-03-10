using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;

//this handles the game logic
public class GameManager : MonoBehaviour
{
    public List<Player> Players;
    public int PlayerTurn = 0;
    public bool GameEnded=false;
    public int Day = 1;

    public static event Action OnTurnEnd;
    public static event Action OnDayEnd;

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
        if (Input.GetKeyDown(KeyCode.C))
        {
            EndTurn();
        }
    }
    private void EndTurn()
    {
        PlayerTurn= (PlayerTurn+1)%Players.Count;
        OnTurnEnd?.Invoke();
        if (PlayerTurn == 0)
        {
            Day++;
            OnDayEnd?.Invoke();
        }
    }

 
 



}
