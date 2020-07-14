﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceController : MonoBehaviour {
    private Dictionary<int, int> productionNodeCount = new Dictionary<int, int>() { {0, 0}, {1, 0} };
    private Dictionary<int, float> resourceRate = new Dictionary<int, float>() { {0, 0}, {1, 0} };
    private ProductionNode[] productionNodes;
    private ResourceNode[] resourceNodes;
    private UIController uiController;

    void Awake() {
        uiController = UIController.Instance;
    }

    void OnProductionNodeCaptured(int newTeam, int oldTeam) {
        float[] storeProductionNodes = uiController.Store["ProductionNodes"];

        if (newTeam != 2) {
            productionNodeCount[newTeam] += 1;
            storeProductionNodes[newTeam] = productionNodeCount[newTeam];

            if (oldTeam != 2) {
                productionNodeCount[oldTeam]--;
                storeProductionNodes[oldTeam] = productionNodeCount[oldTeam];
            }
        }

        uiController.SetValue("ProductionNodes", storeProductionNodes);
    }

    void OnResourceNodeCaptured(int newTeam, int oldTeam, SOResourceNode resourceNodeData) {
        float[] storeResourceRates = uiController.Store["ResourceRate"];

        if (newTeam != 2) {
            resourceRate[newTeam] += resourceNodeData.resourceRate;
            storeResourceRates[newTeam] = resourceRate[newTeam];

            if (oldTeam != 2) {
                resourceRate[oldTeam] -= resourceNodeData.resourceRate;
                storeResourceRates[oldTeam] = resourceRate[oldTeam];
            }
        }

        uiController.SetValue("ResourceRate", storeResourceRates);
    }

    void Start() {
        productionNodes = GameObject.FindGameObjectsWithTag("ProductionNode")
            .Select(gameObject => gameObject.GetComponent<ProductionNode>()).ToArray();
        resourceNodes = GameObject.FindGameObjectsWithTag("ResourceNode")
            .Select(gameObject => gameObject.GetComponent<ResourceNode>()).ToArray();

        for (int i = 0; i < productionNodes.Length; i++) {
            ProductionNode currentNode = productionNodes[i];

            if (currentNode.Team < 2) {
                productionNodeCount[currentNode.Team] += 1;
            }

            currentNode.Captured += OnProductionNodeCaptured;
        }
        for (int i = 0; i < resourceNodes.Length; i++) {
            ResourceNode currentNode = resourceNodes[i];

            if (currentNode.Team < 2) {
                resourceRate[currentNode.Team] += currentNode.ResourceNodeData.resourceRate;
            }

            currentNode.Captured += OnResourceNodeCaptured;
        }
    }

    void Update() {
        for (int i = 0; i < productionNodes.Length; i++) {
            ProductionNode currentNode = productionNodes[i];

            if (currentNode.Team < 2) {
                currentNode.Build((resourceRate[currentNode.Team] / productionNodeCount[currentNode.Team]) * Time.deltaTime);
            }
        }
    }
}
