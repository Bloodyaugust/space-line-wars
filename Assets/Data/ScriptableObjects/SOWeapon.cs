using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Weapon", order = 1)]
public class SOWeapon : ScriptableObject {
    public int clipSize;
    public float cooldown;
    public float firingArc;
    public float reload;
    public float range;
}
