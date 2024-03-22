using UnityEngine;

// We have to discuss this 
public class Building 
{  
    public Vector3Int Position;
    public int Owner;
    public EBuildings BuildingType;
    public int Health = 200;

    // Building constructor
    public Building(Vector3Int position, EBuildings buildingType, int owner)
    {
        Position = position;
        BuildingType = buildingType;
        Owner = owner;  
    }
}
