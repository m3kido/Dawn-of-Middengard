using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Weapon
{
    public int MinRange;
    public int MaxRange;

    public List<int> DamageList;

    [SerializeField] private int _munitions;
    [SerializeField] private int _munitionsConsumption;

    public int Munitions { get => _munitions; set { if (value > 0) _munitions = value; } }
}
