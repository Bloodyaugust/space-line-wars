using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResearchComponent : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
    public event Action<SOResearch> Clicked;
    
    public SOResearch Research;
    private UIController uiController;

    private bool disabled;
    private RawImage image;

    public void Disable() {
        if (!disabled) {
            disabled = true;

            image.color = Color.HSVToRGB(0, 0, 0.33f);
        }
    }

    public void Enable() {
        if (disabled) {
            disabled = false;

            image.color = Color.HSVToRGB(0, 0, 1);
        }
    }

    public void Initialize(bool isDisabled, SOResearch research) {
        Research = research;

        image.texture = research.icon;

        if (isDisabled) {
            Disable();
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (!disabled) {
            Clicked?.Invoke(Research);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        uiController.SetValue("TooltipItem", Research);
    }

    public void OnPointerExit(PointerEventData eventData) {
        uiController.SetValue("TooltipItem", null);
    }

    void Awake() {
        image = GetComponent<RawImage>();
        uiController = UIController.Instance;
    }
}
