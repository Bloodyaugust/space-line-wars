using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAcquisition : MonoBehaviour {
    public event Action<Ship> TargetAcquired;
    public event Action TargetLost;

    private bool loseTargetOnRangeExit;
    private List<Ship> possibleTargets;
    private Ship currentTarget;
    private Ship parentShip;

    public void Initialize(float range, bool respectRangeForTargetKeeping) {
        GetComponent<CircleCollider2D>().radius = range / 2;
        loseTargetOnRangeExit = respectRangeForTargetKeeping;
    }

    void Awake() {
        parentShip = GetComponentInParent<Ship>();

        possibleTargets = new List<Ship>();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.name == "Health") {
            Ship colliderShip = collider.gameObject.GetComponentInParent<Ship>();

            if (colliderShip.Team != parentShip.Team) {
                possibleTargets.Add(colliderShip);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.name == "Health") {
            Ship colliderShip = collider.gameObject.GetComponentInParent<Ship>();

            if (colliderShip.Team != parentShip.Team) {
                possibleTargets.Remove(colliderShip);
            }

            if (colliderShip == currentTarget && loseTargetOnRangeExit) {
                Untarget();
            }
        }
    }

    void Start() {
        gameObject.layer = LayerMask.NameToLayer(parentShip.Team.ToString());
    }

    void Untarget() {
        TargetLost?.Invoke();

        currentTarget.Died -= Untarget;
        currentTarget = null;
    }

    void Update() {
        if (possibleTargets.Count > 0 && possibleTargets[0] == null) {
            possibleTargets.RemoveAt(0);
        }

        if (currentTarget == null && possibleTargets.Count > 0) {
            currentTarget = possibleTargets[0];
            currentTarget.Died += Untarget;

            TargetAcquired?.Invoke(currentTarget);
        }
    }
}
