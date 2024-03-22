using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Class to represent a unit, associated to every unit prefab on the scene
public class Unit : MonoBehaviour
{
    MapManager Mm;
    UnitManager Um;
    SpriteRenderer rend;

    public UnitDataSO Data;
    public int Health;
    public int Provisions;
    public bool IsSelected = false;
    public int Owner;

    public bool IsMoving = false;
    private bool _hasMoved;
    public bool HasMoved
    {
        get
        {
            return _hasMoved;
        }
        set
        {
            _hasMoved = value;
            if (_hasMoved)
            {
                rend.color = Color.gray;
            }
            else
            {
                rend.color = Color.white;
            }
        }
    }

    // Dictionary to hold the grid position of the valid tiles along with the provisions consumed to reach them
    public Dictionary<Vector3Int, int> ValidTiles = new();

    void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        Health = 100;
        Provisions = Data.MaxProvisions;
        HasMoved = false;
    }

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
        Vector3Int startPos = Mm.Map.WorldToCell(transform.position);

        // You can find SeekTile() just below
        SeekTile(startPos, -1);

        foreach (var pos in ValidTiles.Keys)
        {
            if (ValidTiles[pos] <= Provisions)
            {
                Mm.Map.SetTileFlags(pos, TileFlags.None);
                Mm.HighlightTile(pos);
            }
            else
            {
                ValidTiles.Remove(pos);
            }
        }
    }

    // Unhighlight the accessible tiles to the unit
    public void ResetTiles()
    {
        IsSelected = false;
        foreach (var pos in ValidTiles.Keys)
        {
            Mm.UnHighlightTile(pos);
        }
        ValidTiles.Clear();
    }

    // Check if the given grid position falls into the move range of the unit
    private bool InBounds(Vector3Int pos)
    {
        // Manhattan distance : |x1 - x2| + |y1 - y2|
        if (Mathf.Abs(Mm.Map.WorldToCell(transform.position).x - pos.x) + Mathf.Abs(Mm.Map.WorldToCell(transform.position).y - pos.y) <= Data.MoveRange)
        {
            return true;
        }
        return false;
    }


    // A recursive function to fill the ValidTiles dictionary
    private void SeekTile(Vector3Int currentPosition, int currentProvisions)
    {
        // Access the current tile
        Tile currTile = Mm.Map.GetTile<Tile>(currentPosition);
        if (currTile == null) { return; }

        if (currentProvisions < 0)
        {
            // Exception for the start tile
            currentProvisions = 0;
        }
        else
        {
            // Add the current tile fuel cost to the current fuel
            currentProvisions += Mm.GetTileData(currTile).ProvisionsCost;
        }

        if (currentProvisions > Provisions) { return; }

        // If the current tile is not an obstacle and falls into the move range of the unit
        if (!Um.IsObstacle(currentPosition, this) && InBounds(currentPosition))
        {
            if (!ValidTiles.ContainsKey(currentPosition))
            {
                ValidTiles.Add(currentPosition, currentProvisions);
            }
            else
            {
                if (currentProvisions < ValidTiles[currentPosition])
                {
                    ValidTiles[currentPosition] = currentProvisions;

                }
                else { return; }
            }
        }
        else return;


        // Explore the nighbouring tiles
        // Restrictions will be added so that we cant go out of the map
        Vector3Int up = currentPosition + Vector3Int.up;
        Vector3Int down = currentPosition + Vector3Int.down;
        Vector3Int left = currentPosition + Vector3Int.left;
        Vector3Int right = currentPosition + Vector3Int.right;

        SeekTile(up, currentProvisions);
        SeekTile(down, currentProvisions);
        SeekTile(left, currentProvisions);
        SeekTile(right, currentProvisions);
    }
}

