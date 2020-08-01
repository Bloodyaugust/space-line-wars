using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShipProductionComponent : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
    public event Action<SOShip> Clicked;
    
    public SOShip Ship;
    private UIController uiController;

    public void OnPointerClick(PointerEventData eventData) {
        Clicked?.Invoke(Ship);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        uiController.SetValue("TooltipItem", Ship);
    }

    public void OnPointerExit(PointerEventData eventData) {
        uiController.SetValue("TooltipItem", null);
    }

    void Awake() {
        uiController = UIController.Instance;
    }
}
