using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAcquisition : MonoBehaviour {
    public event Action<GameObject> TargetAcquired;
    public event Action TargetLost;

    private Ship currentTarget;
    private Ship parentShip;

    void OnDied() {
        TargetLost?.Invoke();
        currentTarget.Died -= OnDied;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.name == "Health") {
            Ship colliderShip = collider.gameObject.GetComponentInParent<Ship>();

            if (colliderShip.Team != parentShip.Team) {
                currentTarget = colliderShip;

                TargetAcquired?.Invoke(currentTarget.gameObject);
                currentTarget.Died += OnDied;
            }
        }
    }

    void Start() {
        parentShip = GetComponentInParent<Ship>();
    }
}
