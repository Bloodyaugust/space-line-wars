using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductionSelectionView : MonoBehaviour {
    public GameObject ShipProductionComponent;

    private bool shown;
    private ProductionNode selectedNode;
    private RectTransform productionOptionsPanel;
    private RectTransform view;
    private TextMeshProUGUI productionRate;
    private UIController uiController;

    void Awake() {
        productionOptionsPanel = transform.Find("ProductionOptionsPanel").GetComponent<RectTransform>();
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
        if (storeKey == "Selection" && uiController.Store[storeKey] != null && uiController.Store[storeKey].GetComponent<ProductionNode>()) {
            selectedNode = uiController.Store[storeKey].GetComponent<ProductionNode>();

            Show();
        }

        if (storeKey == "Selection" && (uiController.Store[storeKey] == null || !uiController.Store[storeKey].GetComponent<ProductionNode>()) && shown) {
            Hide();
        }
    }

    void Show() {
        Clear();

        foreach (SOShip shipData in selectedNode.ShipDataset) {
            GameObject newProductionOptionComponent = Instantiate(ShipProductionComponent, Vector3.zero, Quaternion.identity, (Transform)productionOptionsPanel);
            ShipProductionComponent shipProductionComponent = newProductionOptionComponent.GetComponent<ShipProductionComponent>();

            shipProductionComponent.Ship = shipData;
            newProductionOptionComponent.GetComponent<RawImage>().texture = shipData.sprite;

            shipProductionComponent.Clicked += OnShipProductionComponentClicked;
        }

        view.DOAnchorPos(new Vector2(0, 75), 0.2f);
        shown = true;
    }

    void Start() {
        Hide();
    }

    void Update() {
        if (selectedNode != null) {
            productionRate.text = $"{selectedNode.LastBuildProgress.ToString("n1")}/sec";

            foreach (Transform productionOptionTransform in (Transform)productionOptionsPanel) {
                productionOptionTransform.gameObject.GetComponent<Outline>().enabled = productionOptionTransform.gameObject.GetComponent<ShipProductionComponent>().Ship == selectedNode.CurrentShip;
            }
        }
    }
}
