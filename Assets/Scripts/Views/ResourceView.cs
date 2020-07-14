using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceView : MonoBehaviour {
    private TextMeshProUGUI productionNodes;
    private TextMeshProUGUI resourceRate;
    private UIController uiController;

    void Awake() {
        productionNodes = transform.Find("ProductionNodesPanel/ProductionNodes").GetComponent<TextMeshProUGUI>();
        resourceRate = transform.Find("ResourceRatePanel/ResourceRate").GetComponent<TextMeshProUGUI>();
        uiController = UIController.Instance;

        uiController.StoreUpdated += OnStoreUpdated;
    }

    void OnStoreUpdated(string storeKey) {
        if (storeKey == "ResourceRate") {
            resourceRate.text = uiController.Store[storeKey][0].ToString();
        }

        if (storeKey == "ProductionNodes") {
            productionNodes.text = uiController.Store[storeKey][0].ToString();
        }
    }
}
