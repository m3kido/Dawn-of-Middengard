using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TerrainDataSO : ScriptableObject
{  
    public ETerrains TerrainName;
    public Tile[] TerrainsOfSameType;
    public int ProvisionsCost;
    public int DefenceStars;
}
