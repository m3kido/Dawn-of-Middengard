using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CursorControler : MonoBehaviour
{
    
    UnitManager Um;
    MapManager Mm;

    Vector3Int HoverTile = Vector3Int.zero;
    Vector3Int SelectedTile;

    // Start is called before the first frame update
    void Start()
    {
        Um = FindAnyObjectByType<UnitManager>();
        Mm = FindAnyObjectByType<MapManager>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }
    void HandleInput()
    {
        if (Um.IsMoving) { return; }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SelectedTile = HoverTile;
            Unit refUnit = Um.FindUnit(SelectedTile);
            //if there is a unit on that tile
            if (refUnit != null)
            {
                //u cant move to an occupied tile
                if (Um.SelectedUnit != null && Um.SelectedUnit != refUnit) { return; }

                Um.SelectedUnit = refUnit;
                if (Um.SelectedUnit.isSelected)
                {
                    Um.SelectedUnit.ResetTiles();
                    Um.SelectedUnit = null;
                    Um.Path.Clear();
                    Um.PathCost = 0;
                }
                else
                {
                    Um.SelectedUnit.HighlightTiles();

                }
            }
            else
            {
                if (Um.SelectedUnit != null)
                { 
                    StartCoroutine(Um.MoveUnit());
                }
            }
        }

        //arrows
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveSelector(Vector3Int.right);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveSelector(Vector3Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveSelector(Vector3Int.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveSelector(Vector3Int.down);
        }
    }
     void MoveSelector(Vector3Int offset)
    {
        //dont let the cursor move out of the highlited tiles
        if (Um.SelectedUnit != null && !Um.SelectedUnit.ValidTiles.ContainsKey(HoverTile + offset))
        {
            return;
        }
        //if a unit is selected record the path
        if (Um.SelectedUnit != null)
        {

            //if u hit the start point
            if (Um.SelectedUnit.transform.position == HoverTile + offset)
            {
                Um.UnDrawPath();
                Um.Path.Clear();
                Um.PathCost = 0;

            }
            else
            {
                int index = Um.Path.IndexOf(HoverTile + offset);
                //returns -1 if not found
                if (index < 0)
                {
                    int cost = Mm.GetTileData(Mm.map.GetTile<Tile>(HoverTile + offset)).fuelCost;
                    if (Um.PathCost + cost > Um.SelectedUnit.fuel) { return; }
                    Um.UnDrawPath();
                    Um.Path.Add(HoverTile + offset);
                    Um.PathCost += cost;

                }
                else
                {
                    //remove the loop
                    Um.UnDrawPath();
                    Um.Path.RemoveRange(index + 1, Um.Path.Count - index - 1);
                    //recalculate the new fuel cost
                    Um.PathCost = 0;
                    foreach (Vector3Int pos in Um.Path)
                    {
                        Um.PathCost += Mm.GetTileData(Mm.map.GetTile<Tile>(pos)).fuelCost;
                    }

                }
            }
            Um.DrawPath();
        }

        HoverTile += offset;
        transform.position += offset;

    }
 
   
}
