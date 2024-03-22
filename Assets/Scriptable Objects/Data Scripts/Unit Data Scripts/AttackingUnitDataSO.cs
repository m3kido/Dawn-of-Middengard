using UnityEngine;

// Subclass for attacking units
[CreateAssetMenu(fileName = "AttackingUnit", menuName = "Unit/AttackingUnit")]
public class AttackingUnitDataSO : UnitDataSO
{
    public int MinAttackRange;
    public int MaxAttackRange;
    public bool HasTwoWeapons;
    public int MaxEnergyOrbs;
}