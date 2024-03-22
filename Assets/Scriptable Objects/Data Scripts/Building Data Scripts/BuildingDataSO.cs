using UnityEngine;
using UnityEngine.Tilemaps;

// Scriptable object to get building datas
public abstract class BuildingDataSO : ScriptableObject
{
    public EBuildings BuildingName;
    public Tile BuildingTile;
    public ETeamColors Color;
}
