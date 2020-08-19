using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ResourceNodeState {
    Idle,
    Mining
}

public class ResourceNode : MonoBehaviour, ITooltip {
    public event Action<int, int, SOResourceNode> Captured;

    public bool StartCaptured;
    public int Team;
    public SOResourceNode ResourceNodeData;
    public SOTeamColors TeamColors;

    private Capturable capturable;
    [SerializeField]
    private ResourceNodeState currentState;
    private SetMaterialProperties setMaterialProperties;
    private UIController uiController;

    public string GetTooltipText() {
        return ResourceNodeData.GetTooltipText();
    }

    void Awake() {
        capturable = GetComponentInChildren<Capturable>();
        setMaterialProperties = GetComponent<SetMaterialProperties>();
        uiController = UIController.Instance;

        if (!StartCaptured) {
            Team = 2;
        }

        capturable.Captured += OnCaptured;
    }

    void OnCaptured(int newTeam) {
        int oldTeam = Team;
        Team = newTeam;

        currentState = ResourceNodeState.Mining;

        Captured?.Invoke(newTeam, oldTeam, ResourceNodeData);

        if (StartCaptured) {
            capturable.Captured -= OnCaptured;
            capturable.Disable();
        }

        setMaterialProperties.SetMaterial(1f, TeamColors.Hues[Team], ResourceNodeData.sprite);

        uiController.Store["SpecialResources"][Team].AddRange(ResourceNodeData.resourceFlags);

        if (oldTeam != 2) {
            foreach (string resource in ResourceNodeData.resourceFlags) {
                uiController.Store["SpecialResources"][oldTeam].Remove(resource);
            }
        }

        uiController.UpdateValue("SpecialResources");
    }

    void Start() {
        currentState = ResourceNodeState.Idle;

        Captured?.Invoke(Team, Team, ResourceNodeData);

        if (StartCaptured) {
            capturable.ForceCapture(Team);
        } else {
            setMaterialProperties.SetMaterial(0f, TeamColors.Hues[Team], ResourceNodeData.sprite);
        }
    }
}
