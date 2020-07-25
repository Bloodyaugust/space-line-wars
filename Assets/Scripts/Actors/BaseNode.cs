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

    public int StartingTeam;
    public int Team { get; set; }
    public SOBaseNode BaseNodeData;
    public SOTeamColors TeamColors;

    private BaseNodeState currentState;
    private float researchProgress;
    private Health health;
    private SetMaterialProperties setMaterialProperties;

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
        currentState = BaseNodeState.Idle;

        gameObject.layer = LayerMask.NameToLayer(Team.ToString());
        setMaterialProperties.SetMaterial(1f, TeamColors.Hues[Team], BaseNodeData.sprite);

        health.Initialize(BaseNodeData.health);
    }

    void Update() {

    }
}
