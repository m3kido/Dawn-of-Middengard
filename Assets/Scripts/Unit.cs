using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Unit : MonoBehaviour
{
    
    MapManager Mm;
    UnitManager Um;

    public UnitData Data;
    public int Fuel = 100;
    public bool IsSelected = false;
    public int MoveRange = 3;
    public int TeamSide=1;
    public int Owner=0;

    public bool IsMoving = false;
    public bool HasMoved=false; 
    
    //hold the grid position of the valid tiles along with the fuel consumed to reach them
    
    public Dictionary<Vector3Int,int> ValidTiles = new();

    void Start()
    {
        Mm = FindAnyObjectByType<MapManager>();
        Um = FindAnyObjectByType<UnitManager>();

    }
    private void Update()
    {
        
        
    }


  
    //this function fills the tilefuel dictionary
    public void HighlightTiles()
    {
        IsSelected= true;
        //empty to remove previous cases
        ValidTiles.Clear();
        //world to cell takes a float postion and return a grid position (partie entier)
        Vector3Int startPos = Mm.map.WorldToCell(transform.position);
        
        SeekTile(startPos, -1);
        
        foreach (var pos in ValidTiles.Keys)
        {
            if (ValidTiles[pos] <= Fuel) {
                Mm.map.SetTileFlags(pos, TileFlags.None);
                Mm.map.SetColor(pos, Color.red);
            }
            else
            {
                ValidTiles.Remove(pos);
            }
           
            
        }


    }
   
    public void ResetTiles()
    {
        IsSelected = false;
        foreach (var pos in ValidTiles.Keys)
        {
           
             Mm.map.SetColor(pos, Color.white);
         
            
        }
        ValidTiles.Clear();
    }

    public bool IsObstacle(Vector3Int pos)
    {
        Unit tileUnit = Um.FindUnit(pos);
        return (!tileUnit && tileUnit.TeamSide != TeamSide) || !Data.IsWalkable(Mm.GetTileData(pos).Type);
    }

    //this function checks if a tile falls in the diamond shape around the player
    private bool InBounds(Vector3Int pos)
    {
        //|x1-x2|+|y1-y2|
        var position = transform.position;
        return Mathf.Abs(Mm.map.WorldToCell(position).x - pos.x) + Mathf.Abs(Mm.map.WorldToCell(position).y - pos.y) <= MoveRange;
    }


    //a recursive function
    private void SeekTile(Vector3Int current, int currFuel)
    {
        
        //we access the tile on the position
        Tile currTile = Mm.map.GetTile<Tile>(current);
        if(!currTile ) return; 
       
        if (currFuel < 0)
        {  
            //exception for the start tile
            currFuel = 0;
        }
        else
        {
            //add the current tile fuel cost to the current fuel
            currFuel += Mm.GetTileData(currTile).fuelCost;
        }
       
        if (currFuel > Fuel ) { return; }

        //if the tile we are on is not an obstacle and falls in the diamond shape

        if (!IsObstacle(current) && InBounds(current))
        {

            if (!ValidTiles.ContainsKey(current))
            {
               ValidTiles.Add(current,currFuel);
            }
            else
            {
                if(currFuel < ValidTiles[current])
                {
                    ValidTiles[current] = currFuel;
                    
                }else { return; }
            }
        }
        else return;
        
       
        //we call the funtion to its neighbours
        //restrictions will be added so that u cant go out of the map
        Vector3Int up = current+ Vector3Int.up;
        Vector3Int down = current + Vector3Int.down;
        Vector3Int left = current + Vector3Int.left;
        Vector3Int right = current + Vector3Int.right;

        SeekTile(up, currFuel);
        SeekTile(down, currFuel);
        SeekTile(left, currFuel);
        SeekTile(right, currFuel);
    }
    
    public void MarkMoved()
    {
        HasMoved = true;
    }
    
    void FindTargets()
    {
        
    }

}

