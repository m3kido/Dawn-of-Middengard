using System;
using System.Collections.Generic;
using UnityEngine;

//this handles the game logic
public class GameManager : MonoBehaviour
{
    
    public int PlayerTurn = 0;
    public int Day = 1;
    public List<Player> Players;
    public static event Action OnStateChange;
    private EPlayerStates _gameState;
    public EPlayerStates GameState { 
        get { return _gameState; } 
        set {  _gameState = value;OnStateChange?.Invoke(); LastState = _gameState; }
    }
    public EPlayerStates LastState;

    private void Start()
    {
        GameState = EPlayerStates.Idle;
        // Initialize players
        Players = new List<Player>
        {
            new("Andrew",ETeamColors.Amber, ETeams.A, null),
            new("Freya",ETeamColors.Azure, ETeams.B, null)
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