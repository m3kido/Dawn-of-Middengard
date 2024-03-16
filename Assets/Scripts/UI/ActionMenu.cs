using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ActionMenu : MonoBehaviour
{
    CursorController Cc;
    GameManager Gm;
    UnitManager Um;
    BuildingManager Bm;
    public GameObject Options;
    List<GameObject> OptionsList;
    GameObject SelectedOption;

    public GameObject WaitOption;
    //public GameObject FireOption;
    // public GameObject CaptureOption;
    private void Awake()
    {
        Cc = FindAnyObjectByType<CursorController>();
        Um = FindAnyObjectByType<UnitManager>();
        Gm = FindAnyObjectByType<GameManager>();
        Bm = FindAnyObjectByType<BuildingManager>();
        OptionsList = new List<GameObject>();
    }
    private void OnEnable()
    {
 
        CalculateOptions();
        
    }
    private void OnDisable()
    {
        foreach (GameObject option in OptionsList) { Destroy(option); }
        OptionsList.Clear();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Gm.GameState = EGameStates.Selecting;
            Um.SelectedUnit.transform.position = Cc.SaveTile;
            if(Um.Path.Count!=0) {
                Cc.HoverTile = Um.Path.Last();
            }
            
            Um.SelectUnit(Um.SelectedUnit);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
          
            if(SelectedOption == WaitOption)
            {
                Um.EndMove();
                Gm.GameState= EGameStates.Idle;
            }
        }



    }
    private void CalculateOptions()
    {
      
        //CheckFire();
        CheckCapture();
        
    }
  /* private void CheckFire()
    {
        if (Um.SelectedUnit.CanAttack())
        {
            OptionsList.Add(FireOption);
        }
    }*/
    private void CheckCapture()
    {
        if(Bm.Buildings == null) { return; }
        var building = Bm.Buildings.ContainsKey(Cc.HoverTile)? Bm.Buildings[Cc.HoverTile]:null;
        if (building != null && building.Owner != Gm.PlayerTurn ) {
          //OptionsList.Add( Instantiate(CaptureOption, Options.transform));
        }
        else
        {
            OptionsList.Add(  Instantiate(WaitOption, Options.transform));
            if (OptionsList.Count == 1) { SelectedOption = WaitOption; }
        }
       
    }
}
