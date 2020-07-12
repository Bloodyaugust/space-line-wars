using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ResourceNode", order = 1)]
public class SOResourceNode : ScriptableObject {
    public float resourceRate;
    public string[] resourceFlags;
}
