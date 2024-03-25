using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class BuildingData : ScriptableObject
{
    public Tile Building;
    public EPlayerColors Color;
    public EBuildings BuildingType;
    public List<EUnitType> DeployableUnits;
}
