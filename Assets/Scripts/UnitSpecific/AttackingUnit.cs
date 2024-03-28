using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public enum EWeaponTypes
{
    Axe ,
    Bow , 
    Sword , 
}
public class Weapon
{
    [SerializeField] List<int> _damageList;
    [SerializeField] int _ammo;
    [SerializeField] EWeaponTypes _weaponType;
    [SerializeField] int _ammoaPerAttack; 

    public List<int> DamageList { get => _damageList; set => _damageList = value; }
    public int Ammo { get => _ammo; set => _ammo = value; }
    public EWeaponTypes Type { get => _weaponType; set => _weaponType = value; }

    public void decrementAmmo()
    {
        Ammo -= _ammoaPerAttack;
    }
}

public class AttackingUnit : Unit
{
    // List of damages that this attacking unit can apply to other units using each weapon 
    [SerializeField] List<Weapon> _weapons;
    [SerializeField] int _minRange;
    [SerializeField] int _maxRange;
    [SerializeField] List<int> _ammo; //Ammo for first and second weapon 

    public List<Weapon> Weapons { get => _weapons; set => _weapons = value; }
    public int MinRange { get => _minRange; set => _minRange = value; }
    public int MaxRange { get => _maxRange; set => _maxRange = value; }

    private bool _hasAttacked = false;
    public bool Attacked
    {
        get
        {
            return _hasAttacked;
        }
        set
        {
            _hasAttacked = value;
            if (_hasAttacked)
            {
                rend.color = Color.red;
            }
            else
            {
                rend.color = Color.white;
            }
        }
    }
    public float CalculateDamage(Unit target , AttackingUnit attacker, int weaponIndex )
    {
        
        int baseDamage = _weapons[0].DamageList[(int)target.Type];
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
    
    void ApplyDamage(Unit target, AttackingUnit attacker ,  int weaponIndex)
    {
        var damage = CalculateDamage(target, attacker,  weaponIndex);

        target.Health -= (int)damage;
        if (target != null)
        {
            damage = CalculateDamage( target , attacker,  weaponIndex);
            this.Health -= (int)damage;
        }


    }

    // scans area for targets in an Intervall [ min range, max range[
    List<Unit> ScanTargets(AttackingUnit attacker)
    {
        if (attacker == null || attacker.transform == null || Mm == null)
        {
            Debug.LogError("Error: Null reference detected in ScanTargets method.");
            return new List<Unit>();
        }

        var attackerPos = Mm.Map.WorldToCell(attacker.transform.position);
        if (attackerPos == null )
        {
            Debug.Log("Error a mon 7bb"); 
            return new List<Unit>();
        }
        List<Unit> targets = new List<Unit>();

        foreach (var unit in FindObjectsOfType<Unit>())
        {
            if (unit == null || unit.transform == null)
            {
                Debug.LogWarning("Skipping null unit reference.");
                continue;
            }

            var potentialTargetPos = Mm.Map.WorldToCell(unit.transform.position);

            bool IsInRange = (L1Distance(attackerPos, potentialTargetPos) >= attacker.MinRange) && (L1Distance(attackerPos, potentialTargetPos) < attacker.MaxRange);
            bool IsEnemy = attacker.Owner != unit.Owner;
            bool IsDamageable = attacker._weapons[0].DamageList[(int)unit.Type] != 0;

            if (IsInRange && IsEnemy && IsDamageable)
            {
                targets.Add(unit);
            }
        }

        return targets;
    }


    public void HighlightTargets(AttackingUnit attacker)
    {
        List<Unit> targets = ScanTargets(attacker);
        if (targets.Count > 0 ) {
                    foreach (var target in targets)
        {


                // Change the material color of the target to red
                Renderer renderer = target.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Debug.Log(target.Type);
                    MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
                    renderer.GetPropertyBlock(propBlock);
                    propBlock.SetColor("_Color", Color.green); // Set the color to red
                    renderer.SetPropertyBlock(propBlock);
                }
            }
        }
        else
        {
            Debug.Log("FUCKING 7AYAT");
        }

    }

    public bool canAttack(AttackingUnit attacker)
    {
        // Return true if there are targets available, otherwise return false
        List<Unit> targets = ScanTargets(attacker);
        if (targets.Count > 0)
        {
            return true;
        }
        return false ;
    }




}


