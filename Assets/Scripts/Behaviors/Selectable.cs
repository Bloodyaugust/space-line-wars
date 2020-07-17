using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Selectable : MonoBehaviour {
    private Capturable capturable;
    private SpriteRenderer spriteRenderer;
    private UIController uiController;

    void Awake() {
        capturable = transform.parent.gameObject.GetComponentInChildren<Capturable>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        uiController = UIController.Instance;

        capturable.Captured += OnCaptured;
        uiController.StoreUpdated += OnStoreChanged;
    }

    void OnCaptured(int newTeam) {
        if (uiController.Store["Selection"] != null && uiController.Store["Selection"] == transform.parent.gameObject) {
            uiController.SetValue("Selection", null);
        }
    }

    void OnStoreChanged(string storeKey) {
        if (storeKey == "Selection") {
            if (uiController.Store[storeKey] != null && uiController.Store[storeKey] == transform.parent.gameObject) {
                spriteRenderer.enabled = true;
            } else {
                spriteRenderer.enabled = false;
            }
        }
    }

    void OnMouseUp() {
        uiController.SetValue("Selection", transform.parent.gameObject);
    }
}
