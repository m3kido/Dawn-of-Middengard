using System.Collections.Generic;
using UnityEngine;

public class AttackingUnit : Unit
{
    // List of damages that this attacking unit can apply to other units 
    [SerializeField] List<int> _damageList;

    [SerializeField] int _minRange;
    [SerializeField] int _maxRange;

    public List<int> DamageList { get => _damageList; set => _damageList = value; }
    public int MinRange { get => _minRange; set => _minRange = value; }
    public int MaxRange { get => _maxRange; set => _maxRange = value; }

    public float CalculateDamage(Unit target, Unit attacker)
    {

        int baseDamage = _damageList[(int)target.Type];
        Player attackerPlayer = Gm.Players[attacker.Owner];
        Captain attackerCaptain = attackerPlayer.Captain;
        int celesteAttack = attackerPlayer.IsCelesteActive ? attackerCaptain.CelesteDefense : 0;
        float attackDamage = baseDamage * (1 + attackerCaptain.PassiveAttack) * (1 + celesteAttack);


        int terrainStars = Mm.GetTileData(Mm.Map.WorldToCell(target.transform.position)).Defence;
        Player targetPlayer = Gm.Players[attacker.Owner];
        Captain targetCaptain = targetPlayer.Captain;
        int celesteDefense = targetPlayer.IsCelesteActive ? targetCaptain.CelesteDefense : 0;
        float defenseDamage = (1 - terrainStars * target.Health / 1000) * (1 - targetCaptain.PassiveDefense) * (1 - celesteDefense);


        int chance = (attackerCaptain.Name == Captains.Andrew) ? Random.Range(2, 10) : Random.Range(1, 10);
        float totalDamage = attacker.Health / 100 * attackDamage * defenseDamage * (1 + chance / 100);
        return totalDamage;
    }

    void ApplyDamage(Unit target, Unit attacker)
    {
        var damage = CalculateDamage(target, attacker);

        target.Health -= (int)damage;
        if (target != null)
        {
            damage = CalculateDamage(attacker, target);
            attacker.Health -= (int)damage;
        }


    }

    // scans area for targets in an Intervall [ min range, max range[
    List<Unit> ScanTargets(AttackingUnit attacker)
    {
        var attackerPos = Mm.Map.WorldToCell(attacker.transform.position);
        List<Unit> targets = new();

        foreach (var unit in FindObjectsOfType<Unit>())
        {
            var potentialTargetPos = Mm.Map.WorldToCell(unit.transform.position);

            bool IsInRange = (L1Distance(attackerPos, potentialTargetPos) >= attacker.MinRange) && (L1Distance(attackerPos, potentialTargetPos) < attacker.MaxRange);
            bool IsEnemy = attacker.Owner != unit.Owner;
            bool IsDamageable = attacker.DamageList[(int)unit.Type] != 0;

            if (IsInRange && IsEnemy && IsDamageable)
            {
                targets.Add(unit);
            }
        }

        return targets;
    }

    


}


