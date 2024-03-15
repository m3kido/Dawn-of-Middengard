using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Unit : MonoBehaviour
{
    private MapManager Mm;
    private UnitManager Um;

    public UnitData Data { get; private set; }
    public int Health = 10;
    public int Fuel = 100;
    public EUnitType Type;
    public bool IsSelected = false;
    public int Owner;

    public bool IsMoving = false;
    public bool HasMoved = false;

    // Dictionart to hold the grid position of the valid tiles along with the fuel consumed to reach them


    public Dictionary<Vector3Int, int> ValidTiles = new();

    private void Start()
    {
        // Get map and unit manager from the hierarchy
        Mm = FindAnyObjectByType<MapManager>();
        Um = FindAnyObjectByType<UnitManager>();
    }

    // Highlight the accessible tiles to the unit
    public void HighlightTiles()
    {
        IsSelected = true;

        // Empty to remove previous cases
        ValidTiles.Clear();

        // WorlToCell takes a float postion and converts it to grid position
        var startPos = Mm.Map.WorldToCell(transform.position);

        // you can find SeekTile() just below
        SeekTile(startPos, -1);

        foreach (var pos in ValidTiles.Keys)
            if (ValidTiles[pos] <= Fuel)
            {
                Mm.Map.SetTileFlags(pos, TileFlags.None);
                Mm.HighlightTile(pos);
            }
            else
            {
                ValidTiles.Remove(pos);
            }
    }

    // Unhighlight the accessible tiles to the unit
    public void ResetTiles()
    {
        IsSelected = false;
        foreach (var pos in ValidTiles.Keys) Mm.UnHighlightTile(pos);
        ValidTiles.Clear();
    }

    // Check if the given grid position falls into the move range of the unit
    private bool InBounds(Vector3Int pos)
    {
        // Manhattan distance : |x1 - x2| + |y1 - y2|
        if (Mathf.Abs(Mm.Map.WorldToCell(transform.position).x - pos.x) +
            Mathf.Abs(Mm.Map.WorldToCell(transform.position).y - pos.y) <= Data.MoveRange) return true;
        return false;
    }


    // A recursive function to fill the ValidTiles dictionary
    private void SeekTile(Vector3Int current, int CurrFuel)
    {
        // Access the current tile
        var currTile = Mm.Map.GetTile<Tile>(current);
        if (currTile == null) return;

        if (CurrFuel < 0)
            // Exception for the start tile
            CurrFuel = 0;
        else
            // Add the current tile fuel cost to the current fuel
            CurrFuel += Mm.GetTileData(currTile).FuelCost;

        if (CurrFuel > Fuel) return;

        // If the current tile is not an obstacle and falls into the move range of the unit
        if (!Um.IsObstacle(current, this) && InBounds(current))
        {
            if (!ValidTiles.ContainsKey(current))
            {
                ValidTiles.Add(current, CurrFuel);
            }
            else
            {
                if (CurrFuel < ValidTiles[current])
                    ValidTiles[current] = CurrFuel;
                else
                    return;
            }
        }
        else
        {
            return;
        }


        // Explore the nighbouring tiles
        // Restrictions will be added so that we cant go out of the map
        var up = current + Vector3Int.up;
        var down = current + Vector3Int.down;
        var left = current + Vector3Int.left;
        var right = current + Vector3Int.right;

        SeekTile(up, CurrFuel);
        SeekTile(down, CurrFuel);
        SeekTile(left, CurrFuel);
        SeekTile(right, CurrFuel);
    }

    // Assert the unit has moved
    public void MarkMoved()
    {
        HasMoved = true;
    }
}