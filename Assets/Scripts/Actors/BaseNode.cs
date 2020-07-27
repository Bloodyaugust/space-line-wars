using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BaseNodeState {
    Idle,
    Researching
}

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

    public void Research(float amount) {
        researchProgress += amount;
    }

    void Awake() {
        health = GetComponentInChildren<Health>();
        setMaterialProperties = GetComponent<SetMaterialProperties>();

        health.Died += OnDied;

        Team = StartingTeam;
    }

    void OnDied() {
        Died?.Invoke();
        Destroy(gameObject);
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

            ResearchCompleted?.Invoke(CurrentResearch, Team);

            CurrentResearch = null;
        }
    }
}
