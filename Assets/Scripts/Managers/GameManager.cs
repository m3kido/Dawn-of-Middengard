using System;
using System.Collections.Generic;
using UnityEngine;

//this handles the game logic
public class GameManager : MonoBehaviour
{
    public int PlayerTurn=0;
    public bool GameEnded=false;
    public int Day = 1;
    public List<Player> Players;

    private void Start()
    {
        Players = new List<Player>
        {
            new("Andrew",0, 0, null),
            new("Freya",1, 1, null)
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) EndTurn();
    }

    public static event Action OnTurnEnd;
    public static event Action OnDayEnd;

    private void EndTurn()
    {
        PlayerTurn = (PlayerTurn + 1) % Players.Count;
        OnTurnEnd?.Invoke();
        if (PlayerTurn != 0) return;
        Day++;
        OnDayEnd?.Invoke();
    }
}