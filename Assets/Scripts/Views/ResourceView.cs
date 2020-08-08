using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceView : MonoBehaviour {
    private Animation animation;
    private bool shown;
    private TextMeshProUGUI productionNodes;
    private TextMeshProUGUI resourceRate;
    private UIController uiController;

    void Awake() {
        animation = GetComponent<Animation>();
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

        if (storeKey == "GameState") {
            if (uiController.Store[storeKey] == GameState.Playing) {
                shown = true;
                
                foreach (AnimationState animationState in animation) {
                    animationState.time = 0;
                    animationState.speed = 1;
                    animation.Play();
                }
            } else if (shown == true) {
                shown = false;

                foreach (AnimationState animationState in animation) {
                    animationState.time = animationState.length;
                    animationState.speed = -1;
                    animation.Play();
                }
            }
        }
    }
}
