using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Selectable : MonoBehaviour {
    private SpriteRenderer spriteRenderer;
    private UIController uiController;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        uiController = UIController.Instance;

        uiController.StoreUpdated += OnStoreChanged;
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
