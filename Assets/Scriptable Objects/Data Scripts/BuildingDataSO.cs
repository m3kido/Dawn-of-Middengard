using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class BuildingDataSO : ScriptableObject
{
    public EBuildings BuildingName;
    public Tile BuildingTile;
    public ETeamColors Color;
    public List<EUnits> DeployableUnits;
}
