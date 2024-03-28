using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ActionMenu : MonoBehaviour
{
    CursorController Cc;
    GameManager Gm;
    UnitManager Um;
    BuildingManager Bm;
    AttackManager Am; 

    public Sprite Cursor;
    public GameObject Options;
    List<GameObject> OptionsList;//Hadi optionList ta3na 
    int SelectedOption;

    [SerializeField] GameObject WaitOption;
    [SerializeField] GameObject AttackOption;
    //public GameObject FireOption;
    // public GameObject CaptureOption;

    private GameObject WaitOptionInstance;
    private GameObject AttackOptionInstance;
    //Till now hada li dayrin option 

    //public GameObject FireOption;
    //public GameObject CaptureOption;
    private void Awake()
    {
        Cc = FindAnyObjectByType<CursorController>();
        Um = FindAnyObjectByType<UnitManager>();
        Gm = FindAnyObjectByType<GameManager>();
        Bm = FindAnyObjectByType<BuildingManager>();
        Am = FindAnyObjectByType<AttackManager>();

        OptionsList = new List<GameObject>();

        WaitOptionInstance = Instantiate(WaitOption, Options.transform);
        WaitOptionInstance.SetActive(false);
        AttackOptionInstance = Instantiate(AttackOption, Options.transform);
        AttackOptionInstance.SetActive(false);

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
            Gm.GameState = EGameStates.Selecting;
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
                Gm.GameState = EGameStates.Idle;
            }
            //Logique to do when the player choose to attack 
            else if (OptionsList[SelectedOption].name.Contains("Attack")) {
                AttackingUnit AttackingUnit =(AttackingUnit) Um.SelectedUnit;
                AttackingUnit.HighlightTargets(0);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Um.EndMove();
                    Gm.GameState = EGameStates.Idle;
                }
                   
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
        //Hna n3amr ola list te3 option 

        CheckAttackOption();
        CheckCapture();
        AttackOptionInstance.SetActive(true);
        OptionsList.Add(AttackOptionInstance);

        if (OptionsList.Count > 0)
        {
            SelectedOption = 0;
            OptionsList[SelectedOption].transform.GetChild(0).GetComponent<Image>().color = Color.white; 

        }

    }
    //hna ncheki ida n9der nattacki ()9awed :)
    private void CheckAttackOption()
    {
        if (Um.SelectedUnit is AttackingUnit && Am.UnitCanAttack((AttackingUnit)Um.SelectedUnit, 0)) // 0 hadak just for weapon 0 brk 
            //AttackingUnit manager vide eliya 
        {
            AttackOptionInstance.SetActive(true);
            OptionsList.Add(AttackOptionInstance);
        }
    }
    
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
