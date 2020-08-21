using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductionSelectionView : MonoBehaviour {
    public GameObject ShipProductionComponent;

    private bool shown;
    private ProductionNode selectedNode;
    private RectTransform productionOptionsPanel;
    private RectTransform productionProgressPanel;
    private RectTransform progressBarForeground;
    private RectTransform view;
    private TextMeshProUGUI productionProgress;
    private TextMeshProUGUI productionRate;
    private UIController uiController;

    void Awake() {
        productionOptionsPanel = transform.Find("ProductionOptionsPanel").GetComponent<RectTransform>();
        productionProgressPanel = transform.Find("ProductionProgressPanel").GetComponent<RectTransform>();
        progressBarForeground = productionProgressPanel.Find("ProgressBarForeground").GetComponent<RectTransform>();
        productionProgress = productionProgressPanel.Find("ProgressPercentage").GetComponent<TextMeshProUGUI>();
        productionRate = transform.Find("ProductionRatePanel/ProductionRate").GetComponent<TextMeshProUGUI>();
        uiController = UIController.Instance;
        view = GetComponent<RectTransform>();

        uiController.StoreUpdated += OnStoreUpdated;
    }

    void Clear() {
        foreach (Transform productionOptionTransform in (Transform)productionOptionsPanel) {
            GameObject.Destroy(productionOptionTransform.gameObject);
        }
    }

    void Hide() {
        transform.DOMoveY(0, 0.2f);
        shown = false;
        Clear();
    }

    void OnShipProductionComponentClicked(SOShip shipData) {
        selectedNode.CurrentShip = shipData;
    }

    void OnStoreUpdated(string storeKey) {
        if (storeKey == "Selection" && uiController.Store[storeKey] != null && uiController.Store[storeKey].GetComponent<ProductionNode>() != null) {
            selectedNode = uiController.Store[storeKey].GetComponent<ProductionNode>();

            Show();
        }

        if (storeKey == "Selection" && (uiController.Store[storeKey] == null || uiController.Store[storeKey].GetComponent<ProductionNode>() == null) && shown) {
            Hide();
        }

        if (storeKey == "GameState" && uiController.Store[storeKey] == GameState.Over && shown) {
            Hide();
        }

        if (storeKey == "CompletedResearch" && selectedNode != null) {
            foreach (Transform productionOptionTransform in (Transform)productionOptionsPanel) {
                ShipProductionComponent shipProductionComponent = productionOptionTransform.GetComponent<ShipProductionComponent>();

                if (shipProductionComponent.Ship.prerequisites.Length == 0 || shipProductionComponent.Ship.prerequisites.All(prerequisite => uiController.Store["CompletedResearch"][selectedNode.Team].Contains(prerequisite))) {
                    shipProductionComponent.Enable();
                }
            }
        }
    }

    void Show() {
        Clear();

        SOShip[] buildableShips = selectedNode.BuildOptions();

        foreach (SOShip shipData in selectedNode.ShipDataset) {
            GameObject newProductionOptionComponent = Instantiate(ShipProductionComponent, Vector3.zero, Quaternion.identity, (Transform)productionOptionsPanel);
            ShipProductionComponent shipProductionComponent = newProductionOptionComponent.GetComponent<ShipProductionComponent>();

            shipProductionComponent.Ship = shipData;
            newProductionOptionComponent.GetComponent<RawImage>().texture = shipData.sprite.texture;

            if (buildableShips.Contains(shipProductionComponent.Ship)) {
                shipProductionComponent.Enable();
            }

            shipProductionComponent.Clicked += OnShipProductionComponentClicked;
        }

        view.DOAnchorPos(new Vector2(362, 75), 0.2f);
        shown = true;
    }

    void Start() {
        Hide();
    }

    void Update() {
        if (selectedNode != null) {
            productionProgress.text = $"{((selectedNode.BuildProgress / selectedNode.CurrentShip.cost) * 100).ToString("N0")}%";
            productionRate.text = $"{selectedNode.LastBuildProgress.ToString("n1")}/sec";
            progressBarForeground.localScale = new Vector3(selectedNode.BuildProgress / selectedNode.CurrentShip.cost, 1, 1);

            foreach (Transform productionOptionTransform in (Transform)productionOptionsPanel) {
                productionOptionTransform.gameObject.GetComponent<Outline>().enabled = productionOptionTransform.gameObject.GetComponent<ShipProductionComponent>().Ship == selectedNode.CurrentShip;
            }
        }
    }
}
