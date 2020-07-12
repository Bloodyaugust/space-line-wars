using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceController : MonoBehaviour {
    private Dictionary<int, float> resourceRate = new Dictionary<int, float>() { {0, 0}, {1, 0} };
    private ResourceNode[] resourceNodes;

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
        resourceNodes = GameObject.FindGameObjectsWithTag("ResourceNode")
            .Select(gameObject => gameObject.GetComponent<ResourceNode>()).ToArray();

        for (int i = 0; i < resourceNodes.Length; i++) {
            ResourceNode currentNode = resourceNodes[i];

            if (currentNode.Team < 2) {
                resourceRate[currentNode.Team] += currentNode.ResourceNodeData.resourceRate;
            }

            currentNode.Captured += OnResourceNodeCaptured;
        }
    }
}
