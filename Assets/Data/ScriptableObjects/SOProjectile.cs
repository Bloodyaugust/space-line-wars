using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Projectile", order = 1)]
public class SOProjectile : ScriptableObject {
    public float damage;
    public float health;
    public string moveType;
    public float range;
    public float speed;
}
