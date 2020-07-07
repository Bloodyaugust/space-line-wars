using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMove : MonoBehaviour {
    private bool trackingLastFrame = true;
    private float speed;
    private float turnRate;
    private int turnDirection;
    private Ship ship;
    private Ship currentTarget;
    private ShipState currentState;
    private TargetAcquisition targetAcquisition;

    public void Initialize() {
        ship = GetComponent<Ship>();

        speed = ship.ShipData.speed;
        turnRate = ship.ShipData.turnRate;

        ship.StateChange += OnStateChange;
    }

    void Awake() {
        targetAcquisition = GetComponentInChildren<TargetAcquisition>();

        targetAcquisition.TargetAcquired += OnTargetAcquired;
        targetAcquisition.TargetLost += OnTargetLost;
    }

    void OnStateChange(ShipState newState) {
        currentState = newState;
    }

    void OnTargetAcquired(Ship newTarget) {
        currentTarget = newTarget;
    }

    void OnTargetLost() {
        currentTarget = null;
    }

    void Update() {
        switch(currentState) {
            case ShipState.Attack:
                if (currentTarget != null) {
                    float angleToTarget = Vector2.SignedAngle(transform.right, currentTarget.transform.position - transform.position);

                    if (Mathf.Abs(angleToTarget) <= 30) {
                        transform.Rotate(0, 0, turnRate * Time.deltaTime * Mathf.Sign(angleToTarget), Space.Self);
                        trackingLastFrame = true;
                    } else {
                        if (trackingLastFrame) {
                            turnDirection = turnDirection > 0 ? -1 : 1;
                        }

                        transform.Rotate(0, 0, turnRate * Time.deltaTime * turnDirection, Space.Self);
                        trackingLastFrame = false;
                    }
                }
                break;
            case ShipState.Idle:
                transform.Rotate(0, 0, turnRate * Time.deltaTime, Space.Self);
                break;
            default:
                break;
        }

        transform.Translate(transform.right * Time.deltaTime * speed, Space.World);
    }
}
