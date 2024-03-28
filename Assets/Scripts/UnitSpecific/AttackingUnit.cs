using System.Collections.Generic;
using UnityEngine;

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

    public float CalculateDamage(Unit target , int weaponIndex )
    {
        
        int baseDamage = _weapons[weaponIndex].DamageList[(int)target.Type];
        Player attackerPlayer = Gm.Players[this.Owner];
        Captain attackerCaptain = attackerPlayer.Captain;
        int celesteAttack = attackerPlayer.IsCelesteActive ? attackerCaptain.CelesteDefense : 0;
        float attackDamage = baseDamage * (1 + attackerCaptain.PassiveAttack) * (1 + celesteAttack);


        int terrainStars = Mm.GetTileData(Mm.Map.WorldToCell(target.transform.position)).Defence;
        Player targetPlayer = Gm.Players[this.Owner];
        Captain targetCaptain = targetPlayer.Captain;
        int celesteDefense = targetPlayer.IsCelesteActive ? targetCaptain.CelesteDefense : 0;
        float defenseDamage = (1 - terrainStars * target.Health / 1000) * (1 - targetCaptain.PassiveDefense) * (1 - celesteDefense);


        int chance = (attackerCaptain.Name == Captains.Andrew) ? Random.Range(2, 10) : Random.Range(1, 10);
        float totalDamage = this.Health / 100 * attackDamage * defenseDamage * (1 + chance / 100);
        return totalDamage;
    }
    
    void ApplyDamage(Unit target, int weaponIndex)
    {
        var damage = CalculateDamage(target,  weaponIndex);

        target.Health -= (int)damage;
        if (target != null)
        {
            damage = CalculateDamage( target ,  weaponIndex);
            this.Health -= (int)damage;
        }


    }

    // scans area for targets in an Intervall [ min range, max range[
    List<Unit> ScanTargets( int weaponIndex)
    {
        var attackerPos = Mm.Map.WorldToCell(this.transform.position);
        List<Unit> targets = new();

        foreach (var unit in FindObjectsOfType<Unit>())
        {
            var potentialTargetPos = Mm.Map.WorldToCell(unit.transform.position);

            bool IsInRange = (L1Distance(attackerPos, potentialTargetPos) >= this.MinRange) && (L1Distance(attackerPos, potentialTargetPos) < this.MaxRange);
            bool IsEnemy = this.Owner != unit.Owner;
            bool IsDamageable = this._weapons[weaponIndex].DamageList[(int)unit.Type] != 0;

            if (IsInRange && IsEnemy && IsDamageable)
            {
                targets.Add(unit);
            }
        }

        return targets;
    }

    public void HighlightTargets(int weaponIndex)
    {
        List<Unit> targets = ScanTargets(weaponIndex);
        foreach (var target in targets)
        {
            // Change the material color of the target to red
            Renderer renderer = target.GetComponent<Renderer>();
            if (renderer != null)
            {
                MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(propBlock);
                propBlock.SetColor("_Color", Color.red); // Set the color to red
                renderer.SetPropertyBlock(propBlock);
            }
        }
    }

    public bool canAttack(int weaponIndex)
    {
        // Return true if there are targets available, otherwise return false
        return ScanTargets(weaponIndex).Count > 0;
    }




}


