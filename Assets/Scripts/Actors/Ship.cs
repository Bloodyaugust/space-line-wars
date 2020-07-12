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
    public LineRenderer NavLine;
    public SOShip ShipData;
    public SOTeamColors TeamColors;

    private bool turnedLastFrame;
    private int turnDirection;
    private Health health;
    private MaterialPropertyBlock materialBlock;
    private SpriteRenderer spriteRenderer;
    private ShipMove shipMove;
    private ShipState currentState;
    private TargetAcquisition targetAcquisition;

    public void Initialize() {
        materialBlock = new MaterialPropertyBlock();
        spriteRenderer = GetComponent<SpriteRenderer>();

        materialBlock.SetFloat("_Hue", TeamColors.Hues[Team]);
        spriteRenderer.SetPropertyBlock(materialBlock);

        health.Initialize(ShipData.health);
        shipMove.Initialize(NavLine);
        targetAcquisition.Initialize(ShipData.weapons.Max(weapon => weapon.weapon.range), false);

        foreach (ShipWeaponDefinition weapon in ShipData.weapons) {
            Weapon newWeapon = Instantiate(WeaponPrefab, (Vector3)weapon.position + transform.position, Quaternion.identity, transform).GetComponent<Weapon>();

            newWeapon.WeaponData = weapon.weapon;
            newWeapon.Initialize();
        }

        SetState(ShipState.Follow);
    }

    void Awake() {
        health = GetComponentInChildren<Health>();
        shipMove = GetComponentInChildren<ShipMove>();
        targetAcquisition = GetComponentInChildren<TargetAcquisition>();
        health.Died += OnDied;
        shipMove.FollowComplete += OnFollowComplete;
        targetAcquisition.TargetAcquired += OnTargetAcquired;
        targetAcquisition.TargetLost += OnTargetLost;
    }

    void OnDied() {
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
