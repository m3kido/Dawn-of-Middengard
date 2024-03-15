using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UnitData : ScriptableObject
{
    public int MoveRange;
    public int Defence;
    public List<ETileTypes> WalkableTiles;

    public bool IsWalkable(ETileTypes x)
    {
        return WalkableTiles.Contains(x);
    }
}