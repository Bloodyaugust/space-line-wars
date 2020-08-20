using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BaseNode", order = 1)]
public class SOBaseNode : ScriptableObject, IHealthy {
    public float armor;
    public float health;
    public float shield;
    public float shieldRegen;
    public Texture2D sprite;

    public float Armor { get { return armor; } }
    public float Health { get { return health; } }
    public float Shield { get { return shield; } }
    public float ShieldRegen { get { return shieldRegen; } }
}
