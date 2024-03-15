using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingManager : MonoBehaviour
{
    MapManager Mm;
    UnitManager Um;
    GameManager Gm;


    [SerializeField]
    private List<Unit> UnitPrefabs;
    private Dictionary<Tile, BuildingData> _buildingTileData;
    [SerializeField] private BuildingData[] _buildingDatas;


    public Dictionary<Vector3Int, Building> Buildings;

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
        Um = FindAnyObjectByType<UnitManager>();

        // Scan the map and put all the buldings in the _buildings dictionary
        ScanMapForBuildings();

    }

    // Scan the map and put all the buldings in the _buildings dictionary
    private void ScanMapForBuildings()
    {
        Buildings = new Dictionary<Vector3Int, Building>();
        foreach (var pos in Mm.Map.cellBounds.allPositionsWithin)
        {
            TileData PosTile = Mm.GetTileData(pos);

            if (PosTile != null && PosTile.TileType == ETileTypes.Building)

                if (PosTile != null && PosTile.TileType == ETileTypes.Building)
                {
                    BuildingData CurrData = _buildingTileData[Mm.Map.GetTile<Tile>(pos)];
                    Buildings.Add(pos, new Building(pos, CurrData.BuildingType, (int)CurrData.Color));

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

    public void SpawnUnit(EUnits UnitType, Building building, int Owner)
    {
        Unit NewUnit = Instantiate<Unit>(UnitPrefabs[(int)UnitType], building.Position, Quaternion.identity);
        NewUnit.Owner = Owner;
        NewUnit.HasMoved = true;
        //outline this mf
        if (NewUnit == null) { print("d");return; }
        Um.Units.Add(NewUnit);
        
    }

}
