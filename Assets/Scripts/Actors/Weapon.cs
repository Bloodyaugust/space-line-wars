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
    [SerializeField]
    private Ship target;
    private TargetAcquisition targetAcquisition;
    private WeaponState currentState;

    void Awake() {
        ship = GetComponentInParent<Ship>();
        targetAcquisition = GetComponentInChildren<TargetAcquisition>();

        targetAcquisition.Initialize(WeaponData.range);

        targetAcquisition.TargetAcquired += OnTargetAcquired;
        targetAcquisition.TargetLost += OnTargetLost;

        SetState(WeaponState.Idle);
    }

    void OnTargetAcquired(Ship newTarget) {
        SetState(WeaponState.Attack);
        target = newTarget;
    }

    void OnTargetLost() {
        SetState(WeaponState.Idle);
    }

    void SetState(WeaponState newState) {
        currentState = newState;
    }

    void Update() {
        switch (currentState) {
            case WeaponState.Attack:
                transform.right = target.transform.position - transform.position;
                break;
            default:
                break;
        }
    }
}
