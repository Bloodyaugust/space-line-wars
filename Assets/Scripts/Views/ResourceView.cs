using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceView : MonoBehaviour {
    private Animation animation;
    private bool shown;
    private TextMeshProUGUI productionNodes;
    private TextMeshProUGUI resourceRate;
    private RawImage antimatterIcon;
    private UIController uiController;

    void Awake() {
        animation = GetComponent<Animation>();
        antimatterIcon = transform.Find("SpecialResourcePanel/Antimatter").GetComponent<RawImage>();
        productionNodes = transform.Find("ProductionNodesPanel/ProductionNodes").GetComponent<TextMeshProUGUI>();
        resourceRate = transform.Find("ResourceRatePanel/ResourceRate").GetComponent<TextMeshProUGUI>();
        uiController = UIController.Instance;

        uiController.StoreUpdated += OnStoreUpdated;
    }

    void OnStoreUpdated(string storeKey) {
        if (storeKey == "ResourceRate") {
            resourceRate.text = uiController.Store[storeKey][0].ToString() + "/sec";
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

        if (storeKey == "SpecialResources") {
            if (uiController.Store["SpecialResources"][0].Contains("antimatter")) {
                antimatterIcon.color = new Color(1, 1, 1);
            } else {
                antimatterIcon.color = new Color(0.3018f, 0.3018f, 0.3018f);
            }
        }
    }
}
