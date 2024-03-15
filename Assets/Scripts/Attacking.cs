using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Attacking : Unit
{
    protected int2 AttackRange;
    [SerializeField] protected List<int> AttackList;

    private void FindTargets()
    {
    }

    protected int CalculateDamage(Unit target)
    {
        var baseDamage = AttackList[(int)target.Type];
        return baseDamage;
    }

    private void Attack()
    {
    }
}