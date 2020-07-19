﻿using System;
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

    public GameObject ExplosionPrefab;
    public GameObject WeaponPrefab;
    public int Team;
    public LineRenderer NavLine;
    public SOShip ShipData;
    public SOTeamColors TeamColors;

    private bool turnedLastFrame;
    private int turnDirection;
    private Health health;
    private ShipMove shipMove;
    private ShipState currentState;
    private SetMaterialProperties setMaterialProperties;
    private TargetAcquisition targetAcquisition;
    private TrailRenderer trailRenderer;

    public void Initialize() {
        setMaterialProperties.SetMaterial(0f, TeamColors.Hues[Team], ShipData.sprite);
        trailRenderer.time = Mathf.Clamp(2 - ShipData.speed, 0.5f, 2);

        health.Initialize(ShipData.health);
        shipMove.Initialize(NavLine);
        targetAcquisition.Initialize(ShipData.weapons.Max(weapon => weapon.weapon.range), false);

        foreach (ShipWeaponDefinition weapon in ShipData.weapons) {
            Weapon newWeapon = Instantiate(WeaponPrefab, (Vector3)weapon.position + transform.position, Quaternion.identity, transform).GetComponent<Weapon>();

            newWeapon.WeaponData = weapon.weapon;
            newWeapon.Initialize();
        }

        gameObject.layer = LayerMask.NameToLayer(Team.ToString());

        SetState(ShipState.Follow);
    }

    void Awake() {
        health = GetComponentInChildren<Health>();
        setMaterialProperties = GetComponent<SetMaterialProperties>();
        shipMove = GetComponentInChildren<ShipMove>();
        targetAcquisition = GetComponentInChildren<TargetAcquisition>();
        trailRenderer = GetComponent<TrailRenderer>();
        health.Died += OnDied;
        shipMove.FollowComplete += OnFollowComplete;
        targetAcquisition.TargetAcquired += OnTargetAcquired;
        targetAcquisition.TargetLost += OnTargetLost;
    }

    void OnDied() {
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);

        Died?.Invoke();
        Destroy(gameObject);
    }

    void OnFollowComplete() {
        SetState(ShipState.Idle);
    }

    void OnTargetAcquired(Ship newTarget) {
        SetState(ShipState.Attack);
    }

    void OnTargetLost() {
        SetState(ShipState.Follow);
    }

    void SetState(ShipState newState) {
        currentState = newState;
        StateChange?.Invoke(currentState);
    }
}
