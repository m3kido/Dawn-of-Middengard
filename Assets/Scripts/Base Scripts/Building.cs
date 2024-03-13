using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building 
{  
    public Vector3Int Position;
    public int Owner;
    public EBuildings BuildingType;
    public int Health = 20;

    // Building constructor
    public Building(Vector3Int position, EBuildings buildingType, int owner)
    {
        Position = position;
        BuildingType = buildingType;
        Owner = owner;  
    }
}
