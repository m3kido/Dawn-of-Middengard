using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class BuildingManager : MonoBehaviour
{
    List<Vector3Int> Buildings;
    MapManager Mm;
    void Start()
    {
        Mm = FindAnyObjectByType<MapManager>();
        foreach(var Pos in Mm.map.cellBounds.allPositionsWithin) {
            if (Mm.GetTileData(Pos).Type == ETileType.Building)
            {
                Buildings.Add(Pos);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
