using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingManager : MonoBehaviour
{
    MapManager Mm;
    GameManager Gm;

    private Dictionary<Vector3Int, Building> _buildings;
    private Dictionary<Tile, BuildingData> _buildingTileData;
    [SerializeField] private BuildingData[] _buildingDatas;

    // Get Building datas of every building type from the inspector
    private void Awake()
    {
        _buildingTileData = new Dictionary<Tile, BuildingData>();

        foreach (var buildingData in _buildingDatas)
        {   
                _buildingTileData.Add(buildingData.Building, buildingData);
        }
    }

    void Start()
    {
        // Get the Map and Game Managers from the hierarchy
        Mm = FindAnyObjectByType<MapManager>();
        Gm = FindAnyObjectByType<GameManager>();

        // Scan the map and put all the buldings in the _buildings dictionary
        ScanMapForBuildings();
    }

    // Scan the map and put all the buldings in the _buildings dictionary
    private void ScanMapForBuildings()
    {
        foreach (var pos in Mm.Map.cellBounds.allPositionsWithin)
        {
            if (Mm.GetTileData(pos).TileType == ETileTypes.Building)
            {
                BuildingData CurrData = _buildingTileData[Mm.Map.GetTile<Tile>(pos)];
                _buildings.Add(pos, new Building(pos,CurrData.BuildingType, (int)CurrData.Color));
            }
        }
    }

    // Get building data of given grid position
    public BuildingData GetBuildingData(Vector3Int pos)
    {
        return _buildingTileData[Mm.Map.GetTile<Tile>(pos)];
    }

    // Capture building
    public void CaptureBuilding(Building building, Unit unit)
    {
        building.Health -= unit.Health;
        if (building.Health <= 0)
        {
            building.Owner = unit.Owner;
            building.Health = 20;
        }
    }

/*    public void SpawnUnit(EUnits unitType ,Building building)
    {
        
    }*/
}
