using System;
using System.Collections.Generic;
using UnityEngine;

// Class to handle the game logic
public class GameManager : MonoBehaviour
{
    public int PlayerTurn = 0;
    public int Day = 1;
    public List<Player> Players;
    public static event Action OnStateChange;
    private EPlayerStates _currentPlayerState;
    public EPlayerStates LastState;

    public EPlayerStates CurrentPlayerState { 
        get { return _currentPlayerState; } 
        set { _currentPlayerState = value; OnStateChange?.Invoke(); LastState = _currentPlayerState; }
    }

    private void Start()
    {
        CurrentPlayerState = EPlayerStates.Idle;

        // Initialize players
        Players = new List<Player>
        {
            new("Mohamed", ETeamColors.Amber, ETeams.A, null),
            new("Oussama", ETeamColors.Azure, ETeams.B, null)
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

    // Method to end a turn
    private void EndTurn()
    {
        PlayerTurn = (PlayerTurn + 1) % Players.Count;
        OnTurnEnd?.Invoke();
        if (PlayerTurn != 0) return;
        Day++;
        OnDayEnd?.Invoke();
    }
}