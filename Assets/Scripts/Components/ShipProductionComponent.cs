using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShipProductionComponent : MonoBehaviour, IPointerClickHandler {
    public event Action<SOShip> Clicked;
    
    public SOShip Ship;

    public void OnPointerClick(PointerEventData eventData) {
        Clicked?.Invoke(Ship);
    }
}
