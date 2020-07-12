using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ProductionNodeState {
    Idle,
    Building
}

public class ProductionNode : MonoBehaviour {
    public event Action<ProductionNodeState> StateChange;

    public bool StartCaptured;
    public GameObject ShipPrefab;
    public int Team;
    public LineRenderer navLine;
    public SOProductionNode ProductionNodeData;
    public SOShip[] ShipDataset;
    public SOTeamColors TeamColors;

    private Capturable capturable;
    [SerializeField]
    private int shipIndex;
    private float buildProgress;
    private MaterialPropertyBlock materialBlock;
    private ProductionNodeState currentState;
    private SpriteRenderer spriteRenderer;

    void Awake() {
        capturable = GetComponentInChildren<Capturable>();
        materialBlock = new MaterialPropertyBlock();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (!StartCaptured) {
            Team = 2;
        }

        capturable.Captured += OnCaptured;
    }

    void OnCaptured(int newTeam) {
        Team = newTeam;
        buildProgress = 0;

        materialBlock.SetTexture("_MainTex", spriteRenderer.sprite.texture);
        materialBlock.SetFloat("_Hue", TeamColors.Hues[Team]);
        spriteRenderer.SetPropertyBlock(materialBlock);

        currentState = ProductionNodeState.Building;

        if (StartCaptured) {
            capturable.Captured -= OnCaptured;
        }
    }

    void Start() {
        currentState = ProductionNodeState.Idle;

        if (StartCaptured) {
            capturable.ForceCapture(Team);
        }
    }

    void Update() {
        if (currentState == ProductionNodeState.Building) {
            buildProgress += ProductionNodeData.buildEfficiency * Time.deltaTime;

            if (buildProgress >= ShipDataset[shipIndex].buildTime) {
                GameObject newShip = Instantiate(ShipPrefab, transform.position, Quaternion.identity);
                Ship shipComponent = newShip.GetComponent<Ship>();

                shipComponent.NavLine = navLine;
                shipComponent.ShipData = ShipDataset[shipIndex];
                shipComponent.Team = Team;
                shipComponent.Initialize();

                buildProgress -= ShipDataset[shipIndex].buildTime;
            }
        }
    }
}
