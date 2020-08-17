using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoonSharp.Interpreter;
using UnityEngine;

public enum BaseNodeState {
    Idle,
    Researching
}

[MoonSharpUserData]
public class BaseNode : MonoBehaviour, ITargetable {
    public event Action Died;
    public event Action<SOResearch, int> ResearchCompleted;

    public BaseNodeState CurrentState;
    public int StartingTeam;
    public int Team { get; set; }
    public SOBaseNode BaseNodeData;
    public SOResearch CurrentResearch;
    public SOTeamColors TeamColors;

    private float researchProgress; 
    private Health health;
    private SetMaterialProperties setMaterialProperties;
    private UIController uiController;

    public void Research(float amount) {
        researchProgress += amount;
    }

    public float ResearchProgressPercentage() {
        if (CurrentState == BaseNodeState.Researching) {
            return researchProgress / CurrentResearch.cost;
        }

        return 0;
    }

    public string ResearchProgressText() {
        if (CurrentState == BaseNodeState.Researching) {
            float progressAsPercentage = (researchProgress / CurrentResearch.cost) * 100;
            return $"{progressAsPercentage.ToString("N0")}%";
        }

        return "Inactive";
    }

    void Awake() {
        health = GetComponentInChildren<Health>();
        setMaterialProperties = GetComponent<SetMaterialProperties>();
        uiController = UIController.Instance;

        health.Died += OnDied;

        Team = StartingTeam;
    }

    void OnDied() {
        Died?.Invoke();
        Destroy(gameObject);

        bool[] newDestroyedBaseNodes = uiController.Store["DestroyedBaseNodes"];
        newDestroyedBaseNodes[Team] = true;

        uiController.SetValue("DestroyedBaseNodes", newDestroyedBaseNodes);
        uiController.SetValue("GameState", GameState.Over);
    }

    void Start() {
        CurrentState = BaseNodeState.Idle;

        gameObject.layer = LayerMask.NameToLayer(Team.ToString());
        setMaterialProperties.SetMaterial(1f, TeamColors.Hues[Team], BaseNodeData.sprite);

        health.Initialize(BaseNodeData.health);
    }

    void Update() {
        if (CurrentState == BaseNodeState.Idle && CurrentResearch != null) {
            CurrentState = BaseNodeState.Researching;
        }

        if (CurrentState == BaseNodeState.Researching && researchProgress >= CurrentResearch.cost) {
            CurrentState = BaseNodeState.Idle;
            researchProgress = 0;

            ResearchCompleted?.Invoke(CurrentResearch, Team);

            CurrentResearch = null;
        }
    }
}
