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

public class Ship : MonoBehaviour, ITargetable, ITooltip {
    public event Action Died;
    public event Action<ShipState> StateChange;

    public GameObject ExplosionPrefab;
    public GameObject WeaponPrefab;
    public int Team { get; set; }
    public LineRenderer NavLine;
    public SOShip ShipData;
    public SOTeamColors TeamColors;

    private bool turnedLastFrame;
    private int turnDirection;
    private Health health;
    private ShipMove shipMove;
    private ShipState currentState;
    private SetMaterialProperties setMaterialProperties;
    private SpriteRenderer spriteRenderer;
    private TargetAcquisition targetAcquisition;
    private TrailRenderer trailRenderer;

    public string GetTooltipText() {
        return ShipData.GetTooltipText();
    }

    public void Initialize() {
        setMaterialProperties.SetMaterial(0f, TeamColors.Hues[Team]);
        spriteRenderer.sprite = ShipData.sprite;
        trailRenderer.time = Mathf.Clamp(2 - ShipData.speed, 0.5f, 2);

        health.Initialize(ShipData);
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetAcquisition = GetComponentInChildren<TargetAcquisition>();
        trailRenderer = GetComponent<TrailRenderer>();
        health.Died += OnDied;
        shipMove.FollowComplete += OnFollowComplete;
        targetAcquisition.TargetAcquired += OnTargetAcquired;
        targetAcquisition.TargetLost += OnTargetLost;
    }

    void OnDied() {
        ParticleSystem particleSystem = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity).GetComponentInChildren<ParticleSystem>();
        var main = particleSystem.main;
        main.startColor = TeamColors.Colors[Team];

        Died?.Invoke();
        Destroy(gameObject);
    }

    void OnFollowComplete() {
        SetState(ShipState.Idle);
    }

    void OnTargetAcquired(ITargetable newTarget) {
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
