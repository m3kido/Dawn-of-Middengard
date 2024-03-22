using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ActionMenu : MonoBehaviour
{
    CursorManager Cc;
    GameManager Gm;
    UnitManager Um;
    BuildingManager Bm;

    public Sprite Cursor;
    public GameObject Options;
    List<GameObject> OptionsList;
    int SelectedOption;

    public GameObject WaitOption;
    //public GameObject FireOption;
    // public GameObject CaptureOption;

    private GameObject WaitOptionInstance;
    private GameObject WaitOptionInstance2;
    //public GameObject FireOption;
    // public GameObject CaptureOption;
    private void Awake()
    {
        Cc = FindAnyObjectByType<CursorManager>();
        Um = FindAnyObjectByType<UnitManager>();
        Gm = FindAnyObjectByType<GameManager>();
        Bm = FindAnyObjectByType<BuildingManager>();
        OptionsList = new List<GameObject>();

        WaitOptionInstance = Instantiate(WaitOption, Options.transform);
        WaitOptionInstance.SetActive(false);
        WaitOptionInstance2 = Instantiate(WaitOption, Options.transform);
        WaitOptionInstance2.SetActive(false);

    }
    private void OnEnable()
    {
        if (Bm.Buildings == null) { return; }
        CalculateOptions();

    }
    private void OnDisable()
    {
        if(OptionsList.Count == 0) { return; }
        OptionsList[SelectedOption].transform.GetChild(0).GetComponent<Image>().color = Color.clear;
        foreach (GameObject option in OptionsList) { option.SetActive(false); }
        OptionsList.Clear();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Gm.CurrentPlayerState = EPlayerStates.Selecting;
            Um.SelectedUnit.transform.position = Cc.SaveTile;
            if (Um.Path.Count != 0)
            {
                Cc.HoverTile = Um.Path.Last();
            }

            Um.SelectUnit(Um.SelectedUnit);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {

            if (OptionsList[SelectedOption].name.Contains("Wait"))
            {
                Um.EndMove();
                Gm.CurrentPlayerState = EPlayerStates.Idle;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            OptionsList[SelectedOption].transform.GetChild(0).GetComponent<Image>().color = Color.clear;
            SelectedOption = (SelectedOption - 1 + OptionsList.Count) % OptionsList.Count;
            OptionsList[SelectedOption].transform.GetChild(0).GetComponent<Image>().color = Color.white;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            OptionsList[SelectedOption].transform.GetChild(0).GetComponent<Image>().color = Color.clear;
            SelectedOption = (SelectedOption + 1 ) % OptionsList.Count;
            OptionsList[SelectedOption].transform.GetChild(0).GetComponent<Image>().color = Color.white;
        }



    }
    private void CalculateOptions()
    {

        //CheckFire();
        CheckCapture();
        WaitOptionInstance2.SetActive(true);
        OptionsList.Add(WaitOptionInstance2);

        if (OptionsList.Count > 0)
        {
            SelectedOption = 0;
            OptionsList[SelectedOption].transform.GetChild(0).GetComponent<Image>().color = Color.white;

        }

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
        if (Bm.Buildings == null) { return; }
        var building = Bm.Buildings.ContainsKey(Cc.HoverTile) ? Bm.Buildings[Cc.HoverTile] : null;
        if (building != null && building.Owner != Gm.PlayerTurn)
        {
            //OptionsList.Add( Instantiate(CaptureOption, Options.transform));
        }
        else
        {
            WaitOptionInstance.SetActive(true);
            OptionsList.Add(WaitOptionInstance);


        }

    }
}
