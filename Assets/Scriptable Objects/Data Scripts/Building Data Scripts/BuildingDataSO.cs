using UnityEngine;
using UnityEngine.Tilemaps;

// Scriptable object to get building datas
public abstract class BuildingDataSO : ScriptableObject
{
    public EBuildings BuildingType;
    public Tile BuildingTile;
    public ETeamColors Color;
}