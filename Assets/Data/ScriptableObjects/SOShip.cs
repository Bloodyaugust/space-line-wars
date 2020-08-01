﻿using System;
using UnityEngine;

[Serializable]
public class ShipWeaponDefinition {
    public SOWeapon weapon;
    public Vector2 position;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Ship", order = 1)]
public class SOShip : ScriptableObject, ITooltip {
    public float armor;
    public float cost;
    public float health;
    public float speed;
    public float turnRate;
    public string description;
    public string moveType;
    public ShipWeaponDefinition[] weapons;
    public Texture2D sprite;

    private string tooltipText;

    public string GetTooltipText() {
        if (tooltipText == "") {
            tooltipText = BuildTooltipText();
        }

        return tooltipText;
    }

    string BuildTooltipText() {
        return $"<size=\"36px\"><align=\"center\">{name}</align></size>\r\n\r\n"
        + $"-<indent=\"15%\">Armor: {armor}</indent>\r\n"
        + $"-<indent=\"15%\">Cost: {cost}</indent>\r\n"
        + $"-<indent=\"15%\">Health: {health}</indent>\r\n"
        + $"-<indent=\"15%\">Speed: {speed}</indent>\r\n\r\n"
        + $"{description}";
    }
}
