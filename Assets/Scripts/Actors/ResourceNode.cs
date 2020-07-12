using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ResourceNodeState {
    Idle,
    Mining
}

public class ResourceNode : MonoBehaviour {
    public event Action<int, int, SOResourceNode> Captured;

    public bool StartCaptured;
    public int Team;
    public SOResourceNode ResourceNodeData;
    public SOTeamColors TeamColors;

    private Capturable capturable;
    [SerializeField]
    private MaterialPropertyBlock materialBlock;
    private ResourceNodeState currentState;
    private SpriteRenderer spriteRenderer;

    void Awake() {
        capturable = GetComponentInChildren<Capturable>();
        materialBlock = new MaterialPropertyBlock();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (!StartCaptured) {
            Team = 2;
        }

        SetMaterial(0f);

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

        SetMaterial(1f);
    }

    void SetMaterial(float flashing) {
        materialBlock.SetTexture("_MainTex", spriteRenderer.sprite.texture);
        materialBlock.SetFloat("_Hue", TeamColors.Hues[Team]);
        materialBlock.SetFloat("_Flashes", flashing);
        spriteRenderer.SetPropertyBlock(materialBlock);
    }

    void Start() {
        currentState = ResourceNodeState.Idle;

        Captured?.Invoke(Team, Team, ResourceNodeData);

        if (StartCaptured) {
            capturable.ForceCapture(Team);
        }
    }
}
