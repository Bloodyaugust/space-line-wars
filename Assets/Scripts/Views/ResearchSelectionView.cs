using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResearchSelectionView : MonoBehaviour {
    public GameObject ResearchComponent;
    public SOResearch[] Researches;

    private bool shown;
    private BaseNode selectedNode;
    private RectTransform researchOptionsPanel;
    private RectTransform view;
    private UIController uiController;

    void Awake() {
        researchOptionsPanel = transform.Find("ResearchOptionsPanel").GetComponent<RectTransform>();
        uiController = UIController.Instance;
        view = GetComponent<RectTransform>();

        uiController.StoreUpdated += OnStoreUpdated;
    }

    void Clear() {
        foreach (Transform researchOptionTransform in (Transform)researchOptionsPanel) {
            GameObject.Destroy(researchOptionTransform.gameObject);
        }
    }

    void Hide() {
        transform.DOMoveY(0, 0.2f);
        shown = false;
        Clear();
    }

    void OnResearchComponentClicked(SOResearch researchData) {
        selectedNode.CurrentResearch = researchData;
    }

    void OnStoreUpdated(string storeKey) {
        if (storeKey == "Selection" && uiController.Store[storeKey] != null && uiController.Store[storeKey].GetComponent<BaseNode>() != null) {
            selectedNode = uiController.Store[storeKey].GetComponent<BaseNode>();

            Show();
        }

        if (storeKey == "Selection" && (uiController.Store[storeKey] == null || uiController.Store[storeKey].GetComponent<BaseNode>() == null) && shown) {
            Hide();
        }

        if (storeKey == "CompletedResearch" && selectedNode != null) {
            foreach (Transform researchOptionTransform in (Transform)researchOptionsPanel) {
                ResearchComponent researchComponent = researchOptionTransform.GetComponent<ResearchComponent>();

                if (uiController.Store["CompletedResearch"][selectedNode.Team].Contains(researchComponent.Research)) {
                    researchComponent.Disable();
                }
            }
        }
    }

    void Show() {
        Clear();

        foreach (SOResearch researchData in Researches) {
            GameObject newResearchOptionComponent = Instantiate(ResearchComponent, Vector3.zero, Quaternion.identity, (Transform)researchOptionsPanel);
            ResearchComponent researchComponent = newResearchOptionComponent.GetComponent<ResearchComponent>();

            bool isDisabled = uiController.Store["CompletedResearch"][selectedNode.Team].Contains(researchData);
            researchComponent.Initialize(isDisabled, researchData);

            researchComponent.Clicked += OnResearchComponentClicked;
        }

        view.DOAnchorPos(new Vector2(0, 75), 0.2f);
        shown = true;
    }

    void Start() {
        Hide();
    }

    void Update() {
        if (selectedNode != null) {
            foreach (Transform researchOptionTransform in (Transform)researchOptionsPanel) {
                researchOptionTransform.gameObject.GetComponent<Outline>().enabled = researchOptionTransform.gameObject.GetComponent<ResearchComponent>().Research == selectedNode.CurrentResearch;
            }
        }
    }
}
