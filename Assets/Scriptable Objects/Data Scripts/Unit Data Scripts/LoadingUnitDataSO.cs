using UnityEngine;
using System.Collections.Generic;

// Subclass for loading units
[CreateAssetMenu(fileName = "LoadingUnit", menuName = "Unit/LoadingUnit")]
public class LoadingUnitDataSO : UnitDataSO
{
    public int numberOfLoadableUnits;
    public List<EUnits> LoadableUnits;
}