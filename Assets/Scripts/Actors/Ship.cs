using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ShipState {
    Idle,
    Follow,
    Attack
}

public class Ship : MonoBehaviour {
    public event Action Died;
    public event Action<ShipState> StateChange;

    public GameObject WeaponPrefab;
    public int Team;
    public SOShip ShipData;

    private bool turnedLastFrame;
    private int turnDirection;
    private Health health;
    private ShipMove shipMove;
    private ShipState currentState;
    private TargetAcquisition targetAcquisition;

    void Awake() {
        health = GetComponentInChildren<Health>();
        shipMove = GetComponentInChildren<ShipMove>();
        targetAcquisition = GetComponentInChildren<TargetAcquisition>();

        health.Initialize(ShipData.health);
        shipMove.Initialize();
        targetAcquisition.Initialize(ShipData.weapons.Max(weapon => weapon.weapon.range), false);

        health.Died += OnDied;
        targetAcquisition.TargetAcquired += OnTargetAcquired;
        targetAcquisition.TargetLost += OnTargetLost;

        SetState(ShipState.Idle);
    }

    void OnDied() {
        Died?.Invoke();
        Destroy(gameObject);
    }

    void OnTargetAcquired(Ship newTarget) {
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
        foreach (ShipWeaponDefinition weapon in ShipData.weapons) {
            GameObject newWeapon = Instantiate(WeaponPrefab, (Vector3)weapon.position + transform.position, Quaternion.identity, transform);

            newWeapon.GetComponent<Weapon>().WeaponData = weapon.weapon;
        }
    }
}
