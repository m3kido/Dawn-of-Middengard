using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileData : ScriptableObject
{
    
    public ETileType Type;
    public Tile[] Tiles;
    public int FuelCost;
    public int Defence;

}
