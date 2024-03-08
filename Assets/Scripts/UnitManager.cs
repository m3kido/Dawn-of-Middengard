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
    public bool IsMoving = false;

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

    
    public void DrawPath()
    {
        if (Path.Count > 1)
        {
            Mm.DrawArrow(Mm.map.WorldToCell(SelectedUnit.transform.position),Path[0],Path[1]);
            for (int i = 1; i < Path.Count - 1; i++)
            {
                Mm.DrawArrow(Path[i - 1],Path[i],Path[i + 1]);
            }
            Mm.DrawArrow(Path[^2],Path[^1],Path[^1]);
        }
        else if (Path.Count == 1)
        {
            Mm.DrawArrow(Mm.map.WorldToCell(SelectedUnit.transform.position),Path[0],Path[0]);
        }


    }
    public void UnDrawPath()
    {
        foreach (var pos in Path)
        {
            Mm.DrawArrow(pos, pos, pos);
        }
    }



    public IEnumerator MoveUnit()
    {
        IsMoving = true;
        SelectedUnit.ResetTiles();
        UnDrawPath();
        foreach (var pos in Path)
        {
            SelectedUnit.transform.position = pos;
            SelectedUnit.Fuel -= Mm.GetTileData(Mm.map.GetTile<Tile>(pos)).fuelCost;
            yield return new WaitForSecondsRealtime(0.08f);
        }
        IsMoving = false;

        Path.Clear();
        PathCost = 0;
        SelectedUnit = null;
    }
}
