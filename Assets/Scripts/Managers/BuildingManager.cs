using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class BuildingManager : MonoBehaviour
{
    MapManager Mm;
    UnitManager Um;
    GameManager Gm;


    [FormerlySerializedAs("UnitPrefabs")] [SerializeField] private List<Unit> _unitPrefabs;
    private Dictionary<Tile, BuildingDataSO> _buildingTileData;
    [SerializeField] private BuildingDataSO[] _buildingDatas;


    public Dictionary<Vector3Int, Building> Buildings;

    // Get Building datas of every building type from the inspector
    private void Awake()
    {
        _buildingTileData = new Dictionary<Tile, BuildingDataSO>();

        foreach (var buildingData in _buildingDatas)
        {
            _buildingTileData.Add(buildingData.BuildingTile, buildingData);
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
    private void OnEnable()
    {
        GameManager.OnDayEnd += AddGold;
    }
    private void OnDisable()
    {
        GameManager.OnDayEnd -= AddGold;
    }

    // Scan the map and put all the buldings in the _buildings dictionary
    private void ScanMapForBuildings()
    {
        Buildings = new Dictionary<Vector3Int, Building>();
        foreach (var pos in Mm.Map.cellBounds.allPositionsWithin)
        {
            TerrainDataSO posTile = Mm.GetTileData(pos);


            if (posTile != null && posTile.TerrainName == ETerrains.Building)
            {
                BuildingDataSO currData = _buildingTileData[Mm.Map.GetTile<Tile>(pos)];
                Buildings.Add(pos, new Building(pos, currData.BuildingName, (int)currData.Color));

            }
        }
    }

    // Get building data of given grid position
    public BuildingDataSO GetBuildingData(Vector3Int pos)
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

    public void SpawnUnit(EUnits unitType, Building building, int owner)
    {
        Unit newUnit = Instantiate<Unit>(_unitPrefabs[(int)unitType], building.Position, Quaternion.identity);
        newUnit.Owner = owner;
        newUnit.HasMoved = true;
        //outline this mf
        if (newUnit == null) { print("d");  return; }
        Um.Units.Add(newUnit);
    }

    private void AddGold()
    {
        foreach (var building in Buildings.Values)
        {
            if(building.Owner < 4)
            {
                Gm.Players[building.Owner].Gold += 1000;
            }
           
        }
    }
}
