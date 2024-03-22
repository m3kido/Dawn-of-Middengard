using System.Collections.Generic;
using UnityEngine;

// Scriptable object to get unit datas
public abstract class UnitDataSO : ScriptableObject
{
    public EUnits UnitType;
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
