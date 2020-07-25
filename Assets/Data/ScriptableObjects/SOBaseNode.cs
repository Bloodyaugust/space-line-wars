using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BaseNode", order = 1)]
public class SOBaseNode : ScriptableObject {
    public float armor;
    public float health;
    public Texture2D sprite;
}
