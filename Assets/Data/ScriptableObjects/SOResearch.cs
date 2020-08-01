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

    private string tooltipText;

    public string GetTooltipText() {
        if (tooltipText == "") {
            tooltipText = BuildTooltipText();
        }

        return tooltipText;
    }

    string BuildTooltipText() {
        return $"<size=\"36px\"><align=\"center\">{name}</align></size>\r\n\r\n"
        + $"-<indent=\"15%\">Improves {key} by: {amount}</indent>\r\n"
        + $"-<indent=\"15%\">Cost: {cost}</indent>\r\n"
        + $"{GetPrerequisiteText()}\r\n\r\n"
        + $"{description}";
    }

    string GetPrerequisiteText() {
        if (prerequisites.Length > 0) {
            string prereqs = "-<indent=\"15%\">Requires: ";

            foreach (SOResearch prerequisite in prerequisites) {
                if (prerequisite != prerequisites[0]) {
                    prereqs += ", ";
                }
                prereqs += prerequisite.name;
            }

            prereqs += "</indent>";

            return prereqs;
        }

        return "";
    }
}
