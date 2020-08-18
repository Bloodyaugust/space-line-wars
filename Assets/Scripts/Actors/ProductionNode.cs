using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoonSharp.Interpreter;
using UnityEngine;

public enum ProductionNodeState {
    Idle,
    Building
}

[MoonSharpUserData]
public class ProductionNode : MonoBehaviour {
    public event Action<int, int> Captured;

    public bool StartCaptured;
    public float BuildProgress { get; private set; }
    public float LastBuildProgress { get; private set; }
    public GameObject ShipPrefab;
    public int Team;
    public LineRenderer navLine;
    public SOProductionNode ProductionNodeData;
    public SOShip CurrentShip;
    public SOShip[] ShipDataset;
    public SOTeamColors TeamColors;

    private Capturable capturable;
    private GameObject mapRoot;
    private ProductionNodeState currentState;
    private SetMaterialProperties setMaterialProperties;
    private Selectable selectable;
    private UIController uiController;

    public void Build(float amount, float nonDeltaAmount) {
        BuildProgress += amount;
        LastBuildProgress = nonDeltaAmount;
    }

    void Awake() {
        capturable = GetComponentInChildren<Capturable>();
        setMaterialProperties = GetComponent<SetMaterialProperties>();
        selectable = GetComponentInChildren<Selectable>();
        uiController = UIController.Instance;

        CurrentShip = ShipDataset[0];

        if (!StartCaptured) {
            Team = 2;
        }

        if (Team == 2) {
            selectable.gameObject.SetActive(false);
        }

        capturable.Captured += OnCaptured;
        uiController.StoreUpdated += OnStoreUpdated;
    }

    void OnCaptured(int newTeam) {
        int oldTeam = Team;
        Team = newTeam;
        BuildProgress = 0;

        currentState = ProductionNodeState.Building;

        Captured?.Invoke(Team, oldTeam);

        if (StartCaptured) {
            capturable.Captured -= OnCaptured;
            capturable.Disable();
        }

        if (!selectable.gameObject.activeSelf) {
            selectable.gameObject.SetActive(true);
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
        if (currentState == ProductionNodeState.Building && BuildProgress >= CurrentShip.cost) {
            GameObject newShip = Instantiate(ShipPrefab, transform.position, Quaternion.identity, mapRoot.transform);
            Ship shipComponent = newShip.GetComponent<Ship>();

            shipComponent.NavLine = navLine;
            shipComponent.ShipData = CurrentShip;
            shipComponent.Team = Team;
            shipComponent.Initialize();

            BuildProgress -= CurrentShip.cost;
        }
    }
}
