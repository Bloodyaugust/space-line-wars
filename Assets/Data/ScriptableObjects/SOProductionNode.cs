using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ProductionNode", order = 1)]
public class SOProductionNode : ScriptableObject {
    public int tier;
    public float buildEfficiency;
}
