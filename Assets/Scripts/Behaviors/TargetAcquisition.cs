using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAcquisition : MonoBehaviour {
    public event Action<Ship> TargetAcquired;
    public event Action TargetLost;

    private bool hasTarget;
    [SerializeField]
    private Ship currentTarget;
    private Ship parentShip;

    void Awake() {
        parentShip = GetComponentInParent<Ship>();
    }

    public void Initialize(float range) {
        GetComponent<CircleCollider2D>().radius = range / 2;
    }

    void OnDied() {
        TargetLost?.Invoke();
        currentTarget.Died -= OnDied;
        hasTarget = false;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (!hasTarget && collider.name == "Health") {
            Ship colliderShip = collider.gameObject.GetComponentInParent<Ship>();

            if (colliderShip.Team != parentShip.Team) {
                currentTarget = colliderShip;

                TargetAcquired?.Invoke(colliderShip);
                currentTarget.Died += OnDied;
                hasTarget = true;
            }
        }
    }
}
