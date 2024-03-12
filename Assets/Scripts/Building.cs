using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building 
{
    
    Vector3Int position;
    public int Owner;
    public EBuilding BuildingType;
    public int Health=20;
    public Building(Vector3Int position,EBuilding BuildingType ,int Owner)
    {
        this.position = position;
        this.BuildingType = BuildingType;
        this.Owner = Owner;
    
        
    }
   
    
   
}
