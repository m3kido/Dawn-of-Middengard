using System;
using System.Collections.Generic;
using UnityEngine;

//this handles the game logic
public class GameManager : MonoBehaviour
{
    public int PlayerTurn = 0;
    public bool GameEnded = false;
    public int Day = 1;
    public List<Player> Players;
    private void Start()
    {
        // Initialize players
        Players = new List<Player>
        {
            new("Andrew",0, 0, null),
            new("Freya",1, 1, null)
        };
    }

    private void Update()
    {
        // Handle input for turn end
        if (Input.GetKeyDown(KeyCode.C)) EndTurn();
    }

    // Declare turn end and day end events
    public static event Action OnTurnEnd;
    public static event Action OnDayEnd;

    // Method to end turn
    private void EndTurn()
    {
        PlayerTurn = (PlayerTurn + 1) % Players.Count;
        OnTurnEnd?.Invoke();
        if (PlayerTurn != 0) return;
        Day++;
        OnDayEnd?.Invoke();
    }
}