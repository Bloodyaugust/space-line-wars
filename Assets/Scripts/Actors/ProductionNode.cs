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
    public float LastBuildProgress { get; private set; }
    public GameObject ShipPrefab;
    public int Team;
    public LineRenderer navLine;
    public SOProductionNode ProductionNodeData;
    public SOShip CurrentShip;
    public SOShip[] ShipDataset;
    public SOTeamColors TeamColors;

    private Capturable capturable;
    private float buildProgress;
    private GameObject mapRoot;
    private ProductionNodeState currentState;
    private SetMaterialProperties setMaterialProperties;
    private UIController uiController;

    public void Build(float amount, float nonDeltaAmount) {
        buildProgress += amount;
        LastBuildProgress = nonDeltaAmount;
    }

    void Awake() {
        capturable = GetComponentInChildren<Capturable>();
        setMaterialProperties = GetComponent<SetMaterialProperties>();
        uiController = UIController.Instance;

        CurrentShip = ShipDataset[0];

        if (!StartCaptured) {
            Team = 2;
        }

        capturable.Captured += OnCaptured;
        uiController.StoreUpdated += OnStoreUpdated;
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

    void OnStoreUpdated(string storeKey) {
        if (storeKey == "GameState") {
            if (uiController.Store[storeKey] == GameState.Over) {
                currentState = ProductionNodeState.Idle;
            }
        }
    }

    void Start() {
        currentState = ProductionNodeState.Idle;
        mapRoot = GameObject.Find("MapRoot");

        Captured?.Invoke(Team, Team);

        if (StartCaptured) {
            capturable.ForceCapture(Team);
        } else {
            setMaterialProperties.SetMaterial(0f, TeamColors.Hues[Team], ProductionNodeData.sprite);
        }
    }

    void Update() {
        if (currentState == ProductionNodeState.Building && buildProgress >= CurrentShip.cost) {
            GameObject newShip = Instantiate(ShipPrefab, transform.position, Quaternion.identity, mapRoot.transform);
            Ship shipComponent = newShip.GetComponent<Ship>();

            shipComponent.NavLine = navLine;
            shipComponent.ShipData = CurrentShip;
            shipComponent.Team = Team;
            shipComponent.Initialize();

            buildProgress -= CurrentShip.cost;
        }
    }
}
