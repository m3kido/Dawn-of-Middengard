using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UnitData : ScriptableObject
{
    //this holds our units static data
    public enum ETile
    {
        grass,
        Forest,
        mountain,
        water
    }
    
    public int moveRange;
    public int Defence;
    public List<ETile> WalkableTiles;
    public bool IsWalkable(ETile x)
    {
        return WalkableTiles.Contains(x);
    }
}
