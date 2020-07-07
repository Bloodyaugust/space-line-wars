﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponState {
    Cooldown,
    Idle,
    Reload
}

public class Weapon : MonoBehaviour {
    public SOWeapon WeaponData;

    private int clipRemaining;
    private float timeToCooldown;
    private float timeToReload;
    private Ship ship;
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
        target = newTarget;
    }

    void OnTargetLost() {
        target = null;
    }

    void SetState(WeaponState newState) {
        currentState = newState;
    }

    void Update() {
        timeToCooldown -= Time.deltaTime;
        timeToReload -= Time.deltaTime;

        if (timeToCooldown > 0) {
            currentState = WeaponState.Cooldown;
        }

        if (timeToReload > 0) {
            currentState = WeaponState.Reload;
        }

        if (timeToReload <= 0 && timeToCooldown <= 0) {
            if (currentState == WeaponState.Reload) {
                clipRemaining = WeaponData.clipSize;
            }

            currentState = WeaponState.Idle;
        }

        if (target != null) {
            float angleToTarget = Vector2.SignedAngle(ship.transform.right, target.transform.position - transform.position);

            Debug.DrawRay(transform.position, (target.transform.position - transform.position + (Vector3)Random.insideUnitCircle * 0.05f).normalized * Vector2.Distance(target.transform.position, transform.position), ship.Team > 0 ? Color.red : Color.green);

            if (Mathf.Abs(angleToTarget) <= WeaponData.firingArc) {
                transform.right = target.transform.position - transform.position;

                if (currentState == WeaponState.Idle) {
                    Debug.Log("fire: " + ship.name);

                    clipRemaining--;
                    timeToCooldown = WeaponData.cooldown;

                    if (clipRemaining <= 0) {
                        timeToReload = WeaponData.reload;
                    }
                }
            } else {
                transform.right = ship.transform.right;
            }
        }
    }
}
