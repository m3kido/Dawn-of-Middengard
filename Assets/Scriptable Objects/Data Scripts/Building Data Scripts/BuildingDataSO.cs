using UnityEngine;
using UnityEngine.Tilemaps;

// Scriptable object to get building datas
public abstract class BuildingDataSO : ScriptableObject
{
    public EBuildings BuildingType;
    public ETeamColors Color;
    public Tile BuildingTile;
}