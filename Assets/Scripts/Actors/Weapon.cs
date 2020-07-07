using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponState {
    Attack,
    Cooldown,
    Idle,
    Reload
}

public class Weapon : MonoBehaviour {
    public SOWeapon WeaponData;

    private Ship ship;
    private TargetAcquisition targetAcquisition;
    private WeaponState currentState;

    void OnTargetAcquired(GameObject newTarget) {
        SetState(WeaponState.Attack);
    }

    void OnTargetLost() {
        SetState(WeaponState.Idle);
    }

    void SetState(WeaponState newState) {
        currentState = newState;
    }

    void Start() {
        ship = GetComponentInParent<Ship>();
        targetAcquisition = GetComponentInChildren<TargetAcquisition>();

        targetAcquisition.Initialize(WeaponData.range);

        targetAcquisition.TargetAcquired += OnTargetAcquired;
        targetAcquisition.TargetLost += OnTargetLost;

        SetState(WeaponState.Idle);
    }
}
