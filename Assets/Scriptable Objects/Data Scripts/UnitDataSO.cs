using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UnitDataSO : ScriptableObject
{
    public EUnits UnitName;
    public int MoveRange;
    public int MinAttackRange;
    public int MaxAttackRange;
    public int MaxProvisions;
    public bool HasTwoWeapons;
    public int MaxEnergyOrbs;
    public int LineOfSight;
    public int Cost;
    public List<ETerrains> WalkableTerrains;

    public bool IsWalkable(ETerrains tile)
    {
        return WalkableTerrains.Contains(tile);
    }
}
