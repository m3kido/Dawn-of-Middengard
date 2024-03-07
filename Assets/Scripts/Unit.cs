using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Unit : MonoBehaviour
{
   
    MapManager Mm;
    UnitManager Um;

    public int fuel = 100;
    public bool isSelected = false;
    public int moveRange = 3;
    public int TeamSide=1;
    
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


    public bool IsObstacle(Vector3Int tile, Unit unit)
    {
        Unit TileUnit = Um.FindUnit(tile);
        return (Mm.GetTileData(Mm.map.GetTile<Tile>(tile)).isObstacle || (TileUnit != null && TileUnit.TeamSide != unit.TeamSide));
    }
    //this function fills the tilefuel dictionary
    public void HighlightTiles()
    {
        isSelected= true;
        //empty to remove previous cases
        ValidTiles.Clear();
        //world to cell takes a float postion and return a grid position (partie entier)
        Vector3Int startPos = Mm.map.WorldToCell(transform.position);
        
        SeekTile(startPos, -1);
        
        foreach (var pos in ValidTiles.Keys)
        {
            if (ValidTiles[pos] <= fuel) {
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
        isSelected = false;
        foreach (var pos in ValidTiles.Keys)
        {
           
             Mm.map.SetColor(pos, Color.white);
         
            
        }
        ValidTiles.Clear();
    }


    //this function checks if a tile falls in the diamond shape around the player
    private bool InBounds(Vector3Int pos)
    {
        //|x1-x2|+|y1-y2|
        if (Mathf.Abs(Mm.map.WorldToCell(transform.position).x - pos.x) + Mathf.Abs(Mm.map.WorldToCell(transform.position).y - pos.y) <= moveRange)
        {
            return true;
        }
        return false;
    }


    //a recursive function
    private void SeekTile(Vector3Int current, int CurrFuel)
    {
        
        //we access the tile on the position
        Tile currTile = Mm.map.GetTile<Tile>(current);
        if(currTile == null ) { return; }
       
        if (CurrFuel < 0)
        {  
            //exception for the start tile
            CurrFuel = 0;
        }
        else
        {
            //add the current tile fuel cost to the current fuel
            CurrFuel += Mm.GetTileData(currTile).fuelCost;
        }
       
        if (CurrFuel > fuel ) { return; }

        //if the tile we are on is not an obstacle and falls in the diamond shape

        if (!IsObstacle(current,this) && InBounds(current))
        {

            if (!ValidTiles.ContainsKey(current))
            {
               ValidTiles.Add(current,CurrFuel);
            }
            else
            {
                if(CurrFuel < ValidTiles[current])
                {
                    ValidTiles[current] = CurrFuel;
                    
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

        SeekTile(up, CurrFuel);
        SeekTile(down, CurrFuel);
        SeekTile(left, CurrFuel);
        SeekTile(right, CurrFuel);
    }
    
    

}

