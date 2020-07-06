using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShipState {
    Idle,
    Follow,
    Attack
}

public class Ship : MonoBehaviour {
    public event Action Died;
    public event Action<ShipState> StateChange;

    public int Team;
    public SOShip ShipData;

    private bool turnedLastFrame;
    private int turnDirection;
    private Health health;
    private ShipMove shipMove;
    private ShipState currentState;
    private TargetAcquisition targetAcquisition;

    void OnDied() {
        Died?.Invoke();
        Destroy(gameObject);
    }

    void OnTargetAcquired(GameObject newTarget) {
        SetState(ShipState.Attack);
    }

    void OnTargetLost() {
        SetState(ShipState.Idle);
    }

    void SetState(ShipState newState) {
        currentState = newState;
        StateChange?.Invoke(currentState);
    }

    void Start() {
        health = GetComponentInChildren<Health>();
        shipMove = GetComponentInChildren<ShipMove>();
        targetAcquisition = GetComponentInChildren<TargetAcquisition>();

        health.Initialize(ShipData.health);
        shipMove.Initialize();

        health.Died += OnDied;
        targetAcquisition.TargetAcquired += OnTargetAcquired;
        targetAcquisition.TargetLost += OnTargetLost;

        SetState(ShipState.Idle);
    }
}
