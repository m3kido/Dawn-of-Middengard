using System.Collections.Generic;
using UnityEngine;

public struct Damages
{
    public float AttackDamage { get; private set; }
    public float DefenseDamage { get; private set; }
    public float TotalDamage { get; private set; }

    public Damages(float attackDamage, float defenseDamage, float totalDamage) : this()
    {
        AttackDamage = attackDamage;
        DefenseDamage = defenseDamage;
        TotalDamage = totalDamage;
    }
}


public abstract class AttackingUnit : Unit
{
    // List of damages that this attacking unit can apply to other units 
    [SerializeField] List<int> _damageList;

    public List<int> DamageList { get => _damageList; set => _damageList = value; }

    public Damages CalculateDamage(Unit target, Unit attacker)
    {
        int baseDamage = _damageList[(int)target.Type];
        Player attackerPlayer = Gm.Players[attacker.Owner];
        Captain attackerCaptain = attackerPlayer.Captain;
        int celesteAttack = attackerPlayer.IsCelesteActive ? attackerCaptain.CelesteDefense : 0;
        float attackDamage = baseDamage * (1 + attackerCaptain.PassiveAttack) * (1 + celesteAttack);


        int terrainStars = Mm.GetTileData(Mm.Map.WorldToCell(target.transform.position)).Defence;
        Player targetPlayer = Gm.Players[attacker.Owner];
        Captain targetCaptain = targetPlayer.Captain;
        int celesteDefense =  targetPlayer.IsCelesteActive ? targetCaptain.CelesteDefense : 0;
        float defenseDamage = (1 - terrainStars * target.Health / 1000) * (1 - targetCaptain.PassiveDefense) * (1 - celesteDefense);


        int chance = (attackerCaptain.Name == Captains.Andrew) ? Random.Range(2, 10) : Random.Range(1, 10);
        float totalDamage = attacker.Health / 100 * attackDamage * defenseDamage * (1 + chance / 100);
        return new Damages(attackDamage, defenseDamage, totalDamage);
    }

    void ApplyDamage(Unit target, Unit attacker)
    {
        Damages damage = CalculateDamage(target, attacker);

        target.Health -= (int)damage.AttackDamage;
        attacker.Health -= (int)damage.DefenseDamage;
 
    }


}
