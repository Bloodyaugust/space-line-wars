using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ResourceNode", order = 1)]
public class SOResourceNode : ScriptableObject, ITooltip {
    public float resourceRate;
    public string[] resourceFlags;
    public Texture2D sprite;

    private string tooltipText;

    public string GetTooltipText() {
        if (tooltipText == "") {
            tooltipText = BuildTooltipText();
        }

        return tooltipText;
    }

    string BuildTooltipText() {
        string resourcesGenerated = resourceRate > 0 ? resourceRate.ToString() : String.Join(",", resourceFlags);

        return $"<size=\"36px\"><align=\"center\">{name}</align></size>\r\n\r\n"
        + $"-<indent=\"15%\">Generates: {resourcesGenerated}</indent>\r\n";
    }

    void OnEnable() {
        tooltipText = "";
    }
}
