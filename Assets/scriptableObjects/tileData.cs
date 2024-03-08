using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileData : ScriptableObject
{
    
    public UnitData.ETile Type;
    public Tile[] tiles;
    public int fuelCost;

}
