using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShipProductionComponent : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
    public event Action<SOShip> Clicked;
    
    private bool disabled;
    private RawImage image;
    public SOShip Ship;
    private UIController uiController;

    public void Disable() {
        disabled = true;
        image.color = Color.HSVToRGB(0, 0, 0.33f);
    }

    public void Enable() {
        if (disabled) {
            disabled = false;

            image.color = Color.HSVToRGB(0, 0, 1);
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (!disabled) {
            Clicked?.Invoke(Ship);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        uiController.SetValue("TooltipItem", Ship);
    }

    public void OnPointerExit(PointerEventData eventData) {
        uiController.SetValue("TooltipItem", null);
    }

    void Awake() {
        image = GetComponent<RawImage>();
        uiController = UIController.Instance;

        Disable();
    }
}
