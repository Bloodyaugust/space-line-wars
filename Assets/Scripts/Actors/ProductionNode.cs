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
    public event Action<int, int> Captured;

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
    private ProductionNodeState currentState;
    private SetMaterialProperties setMaterialProperties;

    public void Build(float amount) {
        buildProgress += amount;
    }

    void Awake() {
        capturable = GetComponentInChildren<Capturable>();
        setMaterialProperties = GetComponent<SetMaterialProperties>();

        if (!StartCaptured) {
            Team = 2;
        }

        capturable.Captured += OnCaptured;
    }

    void OnCaptured(int newTeam) {
        int oldTeam = Team;
        Team = newTeam;
        buildProgress = 0;

        currentState = ProductionNodeState.Building;

        Captured?.Invoke(Team, oldTeam);

        if (StartCaptured) {
            capturable.Captured -= OnCaptured;
            capturable.Disable();
        }

        setMaterialProperties.SetMaterial(1f, TeamColors.Hues[Team], ProductionNodeData.sprite);
    }

    void Start() {
        currentState = ProductionNodeState.Idle;

        Captured?.Invoke(Team, Team);

        if (StartCaptured) {
            capturable.ForceCapture(Team);
        } else {
            setMaterialProperties.SetMaterial(0f, TeamColors.Hues[Team], ProductionNodeData.sprite);
        }
    }

    void Update() {
        if (currentState == ProductionNodeState.Building && buildProgress >= ShipDataset[shipIndex].cost) {
            GameObject newShip = Instantiate(ShipPrefab, transform.position, Quaternion.identity);
            Ship shipComponent = newShip.GetComponent<Ship>();

            shipComponent.NavLine = navLine;
            shipComponent.ShipData = ShipDataset[shipIndex];
            shipComponent.Team = Team;
            shipComponent.Initialize();

            buildProgress -= ShipDataset[shipIndex].cost;
        }
    }
}
