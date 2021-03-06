﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResearchController : MonoBehaviour {
    private BaseNode[] baseNodes;
    private UIController uiController;

    void Awake() {
        uiController = UIController.Instance;
    }

    void OnResearchCompleted(SOResearch research, int team) {
        uiController.Store["CompletedResearch"][team].Add(research);
        uiController.UpdateValue("CompletedResearch");
        Debug.Log("New research completed: " + research.description + " on team: " + team.ToString());
    }

    void Start() {
        baseNodes = GameObject.FindGameObjectsWithTag("BaseNode")
            .Select(baseNode => baseNode.GetComponent<BaseNode>()).ToArray();

        foreach (BaseNode baseNode in baseNodes) {
            baseNode.ResearchCompleted += OnResearchCompleted;
        }
    }
}
