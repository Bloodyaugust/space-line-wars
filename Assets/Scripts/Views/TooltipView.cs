using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipView : MonoBehaviour {
    private Image panelImage;
    private TextMeshProUGUI content;
    private UIController uiController;

    void Awake() {
        content = transform.Find("Content").GetComponent<TextMeshProUGUI>();
        panelImage = transform.GetComponent<Image>();
        uiController = UIController.Instance;

        uiController.StoreUpdated += OnStoreUpdated;
    }

    void OnStoreUpdated(string storeKey) {
        if (storeKey == "TooltipItem" && uiController.Store[storeKey] != null) {
            content.enabled = true;
            panelImage.enabled = true;

            content.text = ((ITooltip)uiController.Store[storeKey]).GetTooltipText();
        }

        if (uiController.Store[storeKey] == null) {
            content.enabled = false;
            panelImage.enabled = false;
        }
    }
}
