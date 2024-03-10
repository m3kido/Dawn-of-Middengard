using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CursorControler : MonoBehaviour
{
    //this is our player controler
    UnitManager Um;
    MapManager Mm;
    GameManager Gm;

    public Vector3Int HoverTile
    {
        get => Mm.map.WorldToCell(transform.position);
        set => transform.position = value;
    }



    // Start is called before the first frame update
    void Start()
    {
        Um = FindAnyObjectByType<UnitManager>();
        Mm = FindAnyObjectByType<MapManager>();
        Gm = FindAnyObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        //dont handle any input if a unit is moving or attacking
        if (Um.SelectedUnit!= null && Um.SelectedUnit.IsMoving) { return; }
        if (Input.GetKeyDown(KeyCode.X))
        {
            XClicked();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpaceClicked();
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
                    //add tile to path
                    int cost = Mm.GetTileData(Mm.map.GetTile<Tile>(HoverTile + offset)).FuelCost;
                    if (Um.PathCost + cost > Um.SelectedUnit.Fuel) { return; }
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
                        Um.PathCost += Mm.GetTileData(Mm.map.GetTile<Tile>(pos)).FuelCost;
                    }

                }
            }
            Um.DrawPath();
        }

        HoverTile += offset;
       

    }
    private void XClicked()
    {
        //will be modified to handle canceling the move
        
            if (Um.SelectedUnit != null)
            {
                //cancel select
                HoverTile = Mm.map.WorldToCell(Um.SelectedUnit.transform.position);
                Um.DeselectUnit();

            }
        
    }
    private void SpaceClicked()
    {
        Unit RefUnit = Um.FindUnit(HoverTile);
        //if there is a unit on that tile
        if (RefUnit != null)
        {
            //u cant select an another unit when one is selected 
            //this will be modified
            if (Um.SelectedUnit != null)
            {
                if (Um.SelectedUnit == RefUnit) { Um.DeselectUnit(); }
                return;
            }
            //u cant select a unit that isnt yours
            if (RefUnit.Owner != Gm.PlayerTurn) { return; }
            //u cant select a unit that has moved
            if (RefUnit.HasMoved) { return; }

            Um.SelectUnit(RefUnit);
        }
        else
        {
            if (Um.SelectedUnit != null)
            {
                //move towards the selected tile
                StartCoroutine(Um.MoveUnit());
            }
        }
    }
 
   
}
