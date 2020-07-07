using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAcquisition : MonoBehaviour {
    public event Action<Ship> TargetAcquired;
    public event Action TargetLost;


    private bool hasTarget;
    private bool loseTargetOnRangeExit;
    private Ship currentTarget;
    private Ship parentShip;

    public void Initialize(float range, bool respectRangeForTargetKeeping) {
        GetComponent<CircleCollider2D>().radius = range / 2;
        loseTargetOnRangeExit = respectRangeForTargetKeeping;
    }

    void Awake() {
        parentShip = GetComponentInParent<Ship>();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (!hasTarget && collider.name == "Health") {
            Ship colliderShip = collider.gameObject.GetComponentInParent<Ship>();

            if (colliderShip.Team != parentShip.Team) {
                currentTarget = colliderShip;

                TargetAcquired?.Invoke(colliderShip);
                currentTarget.Died += Untarget;
                hasTarget = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (loseTargetOnRangeExit && hasTarget && collider.name == "Health") {
            Ship colliderShip = collider.gameObject.GetComponentInParent<Ship>();

            if (colliderShip == currentTarget) {
                Untarget();
            }
        }
    }

    void Untarget() {
        TargetLost?.Invoke();

        currentTarget.Died -= Untarget;
        currentTarget = null;
        hasTarget = false;
    }
}
