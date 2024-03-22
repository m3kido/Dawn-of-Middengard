using System.Collections.Generic;
using UnityEngine;

// Subclass for unit spawning buildings
[CreateAssetMenu(fileName = "SpawnerBuilding", menuName = "Building/SpawnerBuilding")]
public class SpawnerBuildingDataSO : BuildingDataSO
{
    public List<EUnits> DeployableUnits;
}