using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    // Singleton instance
    public static AttackManager Instance;

    private void Awake()

    {
        // Ensure only one instance of AttackManager exists
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Method to initiate an attack
    public void InitiateAttack(AttackingUnit attacker, Unit target, int weaponIndex)
    {
        
    }

    // Method to check if a unit can attack
    public bool UnitCanAttack(AttackingUnit attacker, int weaponIndex)
    {
        
        return attacker.canAttack(attacker); 
    }
    //till now hada li fih 
    //dok mbed ndir fih handling te3 Attack Action 

}