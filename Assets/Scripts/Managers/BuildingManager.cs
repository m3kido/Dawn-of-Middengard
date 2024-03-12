using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class BuildingManager : MonoBehaviour
{
    Dictionary<Vector3Int, Building> Buildings;
    MapManager Mm;
    GameManager Gm;
    [SerializeField]
    private BuildingData[] BuildingDatas;

    private Dictionary<Tile, BuildingData> _BuildingTileData;

    
    private void Awake()
    {
        _BuildingTileData = new Dictionary<Tile, BuildingData>();

        foreach (var BuildingData in BuildingDatas)
        {   
                _BuildingTileData.Add(BuildingData.Building, BuildingData);
        }
    }
    void Start()
    {
        Mm = FindAnyObjectByType<MapManager>();
        Gm= FindAnyObjectByType<GameManager>();

        ScanMapForBuildings();
    }
    private void ScanMapForBuildings()
    {
        foreach (var Pos in Mm.map.cellBounds.allPositionsWithin)
        {
            if (Mm.GetTileData(Pos).TileType == ETileType.Building)
            {
                BuildingData CurrData = _BuildingTileData[Mm.map.GetTile<Tile>(Pos)];
                Buildings.Add(Pos, new Building(Pos,CurrData.BuildingType, (int)CurrData.Color));
            }
        }
    }
    public BuildingData GetBuildingData(Vector3Int pos)
    {
        return _BuildingTileData[Mm.map.GetTile<Tile>(pos)];
    }
    public void CaptureBuilding(Building building, Unit unit)
    {
        building.Health -= unit.Health;
        if (building.Health <= 0)
        {
            building.Owner = unit.Owner;
            building.Health = 20;
        }
    }
    public void SpawnUnit(EUnits UnitType ,Building building)
    {
        
    }
}
