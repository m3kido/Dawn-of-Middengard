using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu]
public class BuildingData : ScriptableObject
{
    public Tile Building;
    public EColors Color;
    public EBuilding BuildingType;
    public List<EUnits> DeployableUnits;
}
