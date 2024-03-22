using UnityEngine;
using UnityEngine.Tilemaps;

// Scriptable object to get terrain datas
[CreateAssetMenu(fileName = "Terrain", menuName = "Terrain")]
public class TerrainDataSO : ScriptableObject
{  
    public ETerrains TerrainType;
    public Tile[] TerrainsOfSameType;
    public int ProvisionsCost;
    public int DefenceStars;
}
