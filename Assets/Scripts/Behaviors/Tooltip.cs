using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour {
    private ITooltip parentTooltip;
    private UIController uiController;

    void Awake() {
        uiController = UIController.Instance;
    }

    void OnMouseEnter() {
        uiController.SetValue("TooltipItem", parentTooltip);
    }

    void OnMouseExit() {
        if (uiController.Store["TooltipItem"] == parentTooltip) {
            uiController.SetValue("TooltipItem", null);
        }
    }

    void Start() {
        parentTooltip = GetComponentInParent<ITooltip>();
    }
}
