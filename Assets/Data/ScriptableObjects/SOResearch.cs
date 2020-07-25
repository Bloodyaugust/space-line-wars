using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Research", order = 1)]
public class SOResearch : ScriptableObject {
    public float amount;
    public float cost;
    public Texture2D icon;
    public SOResearch[] prerequisites;
    public string description;
    public string key;
}
