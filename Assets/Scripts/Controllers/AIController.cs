using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoonSharp.Interpreter;
using UnityEngine;

public class AIController : MonoBehaviour {
    public SOResearch[] Researches;
    public string AIScriptFilePath;

    private float aliveTime;
    private BaseNode baseNode;
    private ProductionNode[] productionNodes;
    private ResourceNode[] resourceNodes;
    private Script aiScript;
    private string aiScriptString;
    private UIController uiController;

    void Awake() {
        UserData.RegisterAssembly();

        aiScript = new Script();
        uiController = UIController.Instance;

        StreamReader aiScriptStreamReader = new StreamReader(AIScriptFilePath);
        aiScriptString = aiScriptStreamReader.ReadToEnd();
        aiScriptStreamReader.Close();

        aiScript.DoString(aiScriptString);

        aiScript.Globals["Researches"] = Researches;

        aiScript.Options.DebugPrint = s => { Debug.Log(s); };
    }

    void OnResearchCompleted(SOResearch research, int team) {
        uiController.Store["CompletedResearch"][team].Add(research);
        uiController.UpdateValue("CompletedResearch");
        Debug.Log("New research completed: " + research.description + " on team: " + team.ToString());
    }

    void Start() {
        baseNode = GameObject.FindGameObjectsWithTag("BaseNode")
            .Select(baseNode => baseNode.GetComponent<BaseNode>())
            .Where(baseNode => baseNode.Team == 1)
            .First();
        productionNodes = GameObject.FindGameObjectsWithTag("ProductionNode")
            .Select(productionNode => productionNode.GetComponent<ProductionNode>())
            .Where(productionNode => productionNode.Team == 1)
            .ToArray();
        resourceNodes = GameObject.FindGameObjectsWithTag("ResourceNode")
            .Select(productionNode => productionNode.GetComponent<ResourceNode>())
            .Where(productionNode => productionNode.Team == 1)
            .ToArray();

        baseNode.ResearchCompleted += OnResearchCompleted;

        aiScript.Globals["BaseNode"] = baseNode;
        // aiScript.Globals["ProductionNodes"] = productionNodes;
        // aiScript.Globals["ResourceNodes"] = resourceNodes;
    }

    void Update() {
        aliveTime += Time.deltaTime;

        aiScript.Globals["AliveTime"] = aliveTime;

        aiScript.Call(aiScript.Globals["update"]);
    }
}
