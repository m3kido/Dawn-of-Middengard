using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UnitData : ScriptableObject
{
    //this holds our units static data

    public int MoveRange;
    public int Defence;
    public List<ETileType> WalkableTiles;

    public bool IsWalkable(ETileType x)
    {
        return WalkableTiles.Contains(x);
    }
}
