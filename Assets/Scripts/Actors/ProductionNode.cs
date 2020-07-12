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

    public GameObject ShipPrefab;
    public int Team;
    public LineRenderer navLine;
    public SOProductionNode ProductionNodeData;
    public SOShip[] ShipDataset;
    public SOTeamColors TeamColors;

    [SerializeField]
    private int shipIndex;
    private float buildProgress;
    private ProductionNodeState currentState;

    void Start() {
        currentState = ProductionNodeState.Building;
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
