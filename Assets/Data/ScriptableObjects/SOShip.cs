﻿using System;
using UnityEngine;

[Serializable]
public class ShipWeaponDefinition {
    public SOWeapon weapon;
    public Vector2 position;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Ship", order = 1)]
public class SOShip : ScriptableObject {
    public float armor;
    public float cost;
    public float health;
    public float speed;
    public float turnRate;
    public string moveType;
    public ShipWeaponDefinition[] weapons;
    public Texture2D sprite;
}
