using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UnitData : ScriptableObject
{
    //this holds our units static data
    public enum ETiles
    {
        grass,
        Forest,
        mountain,
        water
    }
    public Unit unit;
    public int moveRange;
    public int Defence;
    public List<ETiles> WalkableTiles;
    public bool IsWalkable(ETiles x)
    {
        return WalkableTiles.Contains(x);
    }
}
