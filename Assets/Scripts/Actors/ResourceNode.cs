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
    public event Action<ResourceNodeState> StateChange;

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
        Team = newTeam;

        currentState = ResourceNodeState.Mining;

        StateChange?.Invoke(currentState);

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

        StateChange?.Invoke(currentState);

        if (StartCaptured) {
            capturable.ForceCapture(Team);
        }
    }
}
