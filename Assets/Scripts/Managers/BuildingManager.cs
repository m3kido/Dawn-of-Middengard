using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

// Class to manage Buildings
public class BuildingManager : MonoBehaviour
{
    // Managers will be needed
    MapManager Mm;
    UnitManager Um;
    GameManager Gm;

    // List to store units that can be bought in the building
    [FormerlySerializedAs("UnitPrefabs")] [SerializeField] private List<Unit> _unitPrefabs;

    // Dictionary mapping a tile (on which there's a building) to its building data
    private Dictionary<Tile, BuildingDataSO> _buildingDataFromTile;

    // Array containing building datas of all buildings (provided in the inspector)
    [SerializeField] private BuildingDataSO[] _buildingDatas;

    // Dictionary mapping a position (on which there's a building) to its building
    public Dictionary<Vector3Int, Building> Buildings;

    private void Awake()
    {
        // Fill the _buildingDataFromTile dictionary
        _buildingDataFromTile = new Dictionary<Tile, BuildingDataSO>();
        foreach (var buildingData in _buildingDatas)
        {
            // Put the Building tile as a key, and the building data as a value
            _buildingDataFromTile.Add(buildingData.BuildingTile, buildingData);
        }
    }

    void Start()
    {
        // Get the Map, Game and Unit Managers from the hierarchy
        Mm = FindAnyObjectByType<MapManager>();
        Gm = FindAnyObjectByType<GameManager>();
        Um = FindAnyObjectByType<UnitManager>();

        // Scan the map and put all the buldings in the Buildings dictionary
        ScanMapForBuildings();
    }

    private void OnEnable()
    {
        // GetGoldFromBuildings subscribes to day end event
        GameManager.OnDayEnd += GetGoldFromBuildings;
    }

    private void OnDisable()
    {
        // GetGoldFromBuildings unsubscribes from day end event
        GameManager.OnDayEnd -= GetGoldFromBuildings;
    }

    // Scan the map and put all the buldings in the Buildings dictionary
    private void ScanMapForBuildings()
    {
        Buildings = new Dictionary<Vector3Int, Building>();
        foreach (var pos in Mm.Map.cellBounds.allPositionsWithin)
        {
            TerrainDataSO posTile = Mm.GetTileData(pos);
            if (posTile != null && posTile.TerrainType == ETerrains.Building)
            {
                BuildingDataSO currData = _buildingDataFromTile[Mm.Map.GetTile<Tile>(pos)];
                Buildings.Add(pos, new Building(pos, currData.BuildingType, (int)currData.Color));
            }
        }
    }

    // Get building data of given grid position
    public BuildingDataSO GetBuildingData(Vector3Int pos)
    {
        return _buildingDataFromTile[Mm.Map.GetTile<Tile>(pos)];
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

    // Spawn a unit from a building
    public void SpawnUnit(EUnits unitType, Building building, int owner)
    {
        Unit newUnit = Instantiate<Unit>(_unitPrefabs[(int)unitType], building.Position, Quaternion.identity);
        newUnit.Owner = owner;
        newUnit.HasMoved = true;
        if (newUnit == null) { print("d"); return; }
        Um.Units.Add(newUnit);
    }

    // Gain gold every day
    private void GetGoldFromBuildings()
    {
        foreach (var building in Buildings.Values)
        {
            // MODIFICATION NEEDED: We have tp check whether the building can provide gold or not (only villages can)
            if (building.Owner < 4)
            {
                Gm.Players[building.Owner].Gold += 1000;
            }
        }
    }
}
