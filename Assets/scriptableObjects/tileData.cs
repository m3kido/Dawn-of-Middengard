using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileData : ScriptableObject
{
    public Tile[] tiles;
    public bool isObstacle;
    public int fuelCost;

}
