using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Terrain", menuName = "Terrain")]
public class TerrainDataSO : ScriptableObject
{  
    public ETerrains TerrainName;
    public Tile[] TerrainsOfSameType;
    public int ProvisionsCost;
    public int DefenceStars;
}
