using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Research", order = 1)]
public class SOResearch : ScriptableObject, ITooltip {
    public float amount;
    public float cost;
    public Texture2D icon;
    public SOResearch[] prerequisites;
    public string description;
    public string key;

    public string GetTooltipText() {
        return $"<size=\"36px\"><align=\"center\">{name}</align></size>\r\n\r\n"
        + $"-<indent=\"15%\">Improves {key} by: {amount}</indent>\r\n"
        + $"-<indent=\"15%\">Cost: {cost}</indent>\r\n\r\n"
        + $"{description}";
    }
}
