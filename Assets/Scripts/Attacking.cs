using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Attacking : Unit
{

    
    protected int2 attackRange;
    [SerializeField] AttackMatrix AttackMatrix;
    [SerializeField] protected List<int> AttackList;

    void FindTargets() { }

    protected int CalculateDamage(Unit target)
    {
        int baseDamage = AttackList[(int)target.Type];
        return baseDamage;
    }

    void Attack() { }
}
