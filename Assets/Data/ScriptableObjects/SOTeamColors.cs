using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TeamColors", order = 1)]
public class SOTeamColors : ScriptableObject {
    public Color[] Colors;
    public float[] Hues;
}
