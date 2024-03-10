using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
public class UnitManager : MonoBehaviour
{
    //handles any unit interaction
    //keeps track of units and the path drawn br the cursor
    Unit[] Units;

    public Unit SelectedUnit;
    

    public Vector3Int SaveTile;
    public List<Vector3Int> Path = new();
    public int PathCost = 0;

   
    MapManager Mm;
    
    void Start()
    {
        Mm = FindAnyObjectByType<MapManager>();
        Units = FindObjectsOfType<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        GameManager.OnDayEnd += ResetUnits;
    }
    private void OnDisable()
    {
        GameManager.OnDayEnd -= ResetUnits;
    }
    //gets the unit on that tile
    public Unit FindUnit(Vector3Int pos)
    {
        foreach (Unit unit in Units)
        {
            if (Mm.map.WorldToCell(unit.transform.position) == pos)
            {
                return unit;
            }

        }
        return null;
    }

    //draws the arrow path
    public void DrawPath()
    {
       
            for (int i = 0; i < Path.Count ; i++)
            {
                if (i == 0)
                {
                    //start case because the start point is not in the path list
                    Mm.DrawArrow(Mm.map.WorldToCell(SelectedUnit.transform.position), Path[0], Path[Mathf.Clamp( 1, 0, Path.Count - 1)]);
                    continue;
                }
                //the clamp is for capping the i at its max (path.count -1)
                Mm.DrawArrow(Path[i - 1],Path[i],Path[Mathf.Clamp( i + 1,0,Path.Count-1)]);
            }
    }
    //this undraws the path arrow
    public void UnDrawPath()
    {
        foreach (var pos in Path)
        {
            Mm.DrawArrow(pos, pos, pos);
        }
    }


    //moves the unit
    //will be modified to handle showing the bar UI (for confirming the move and attacking ..)
    public IEnumerator MoveUnit()
    {
        SelectedUnit.IsMoving = true;
        SelectedUnit.ResetTiles();
        UnDrawPath();
        foreach (var pos in Path)
        {
            SelectedUnit.transform.position = pos;
            SelectedUnit.Fuel -= Mm.GetTileData(Mm.map.GetTile<Tile>(pos)).FuelCost;
            yield return new WaitForSecondsRealtime(0.08f);
        }
        SelectedUnit.IsMoving = false;

        Path.Clear();
        PathCost = 0;
        SelectedUnit.MarkMoved();
        SelectedUnit = null;
    }
    
    //this runs at the end of the day 
    //will be modified to reset has attacked
    private void ResetUnits()
    {
        foreach(var unit in Units) {
            unit.HasMoved= false;
        }
    }

    public void DeselectUnit()
    {
        SelectedUnit.ResetTiles();
        UnDrawPath();
        SelectedUnit = null;
        Path.Clear();
        PathCost = 0;
    }
    public void SelectUnit(Unit unit)
    {
        SelectedUnit = unit;
        SelectedUnit.HighlightTiles();
    }
}
