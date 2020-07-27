using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResearchComponent : MonoBehaviour, IPointerClickHandler {
    public event Action<SOResearch> Clicked;
    
    public SOResearch Research;

    private bool disabled;
    private RawImage image;

    public void Disable() {
        if (!disabled) {
            disabled = true;

            image.color = Color.HSVToRGB(0, 0, 0.33f);
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

    void Awake() {
        image = GetComponent<RawImage>();
    }
}
