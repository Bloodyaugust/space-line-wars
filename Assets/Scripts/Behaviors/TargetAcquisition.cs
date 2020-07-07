using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAcquisition : MonoBehaviour {
    public event Action<GameObject> TargetAcquired;
    public event Action TargetLost;

    private bool hasTarget;
    private Ship currentTarget;
    private Ship parentShip;

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

                TargetAcquired?.Invoke(currentTarget.gameObject);
                currentTarget.Died += OnDied;
                hasTarget = true;
            }
        }
    }

    void Start() {
        parentShip = GetComponentInParent<Ship>();
    }
}
