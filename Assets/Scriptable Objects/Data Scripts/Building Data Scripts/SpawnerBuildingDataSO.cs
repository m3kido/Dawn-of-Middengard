using System.Collections.Generic;
using UnityEngine;

// Subclass for unit spawning buildings
[CreateAssetMenu(fileName = "SpawnerBuilding", menuName = "Building/SpawnerBuilding")]
public class SpawnerBuildingDataSO : BuildingDataSO
{
    // Can't be captured
    // Doesn't provide gold
    // Can Spawn units
    public List<EUnits> DeployableUnits;
}