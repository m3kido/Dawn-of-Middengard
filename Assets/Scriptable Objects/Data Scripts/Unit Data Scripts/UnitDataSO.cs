using System.Collections.Generic;
using UnityEngine;

public abstract class UnitDataSO : ScriptableObject
{
    public EUnits UnitName;
    public int MoveRange;
    public int MaxProvisions;
    public int LineOfSight;
    public int Cost;
    public List<ETerrains> WalkableTerrains;

    public bool IsWalkable(ETerrains tile)
    {
        return WalkableTerrains.Contains(tile);
    }
}
