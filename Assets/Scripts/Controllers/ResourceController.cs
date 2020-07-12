using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceController : MonoBehaviour {
    private Dictionary<int, int> productionNodeCount = new Dictionary<int, int>() { {0, 0}, {1, 0} };
    private Dictionary<int, float> resourceRate = new Dictionary<int, float>() { {0, 0}, {1, 0} };
    private ProductionNode[] productionNodes;
    private ResourceNode[] resourceNodes;

    void OnProductionNodeCaptured(int newTeam, int oldTeam) {
        if (newTeam != 2) {
            productionNodeCount[newTeam] += 1;

            if (oldTeam != 2) {
                productionNodeCount[oldTeam]--;
            }
        }

        Debug.Log("Production Nodes: " + productionNodeCount[0] + ", " + productionNodeCount[1]);
    }

    void OnResourceNodeCaptured(int newTeam, int oldTeam, SOResourceNode resourceNodeData) {
        if (newTeam != 2) {
            resourceRate[newTeam] += resourceNodeData.resourceRate;

            if (oldTeam != 2) {
                resourceRate[oldTeam] -= resourceNodeData.resourceRate;
            }
        }

        Debug.Log("Resource Rates: " + resourceRate[0] + ", " + resourceRate[1]);
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
