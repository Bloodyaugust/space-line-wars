using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMove : MonoBehaviour {
    private bool turnedLastFrame;
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
                float angleToTarget = Vector2.Angle(transform.right, currentTarget.transform.position - transform.position);

                if (angleToTarget >= 30) {
                    if (!turnedLastFrame) {
                        turnDirection = Random.value >= 0.5f ? -1 : 1;
                    }

                    transform.Rotate(0, 0, turnRate * Time.deltaTime * turnDirection, Space.Self);
                    turnedLastFrame = true;
                } else {
                    turnedLastFrame = false;
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
