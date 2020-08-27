using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMove : MonoBehaviour {
    public event Action FollowComplete;

    private bool trackingLastFrame = true;
    private float speed;
    private float turnRate;
    private int navLineIndex = 0;
    private int turnDirection;
    private LineRenderer navLine;
    private Ship ship;
    private ITargetable currentTarget;
    private ShipState currentState;
    private TargetAcquisition targetAcquisition;
    private Vector3[] navLinePoints;

    public void Initialize(LineRenderer newNavLine) {
        ship = GetComponent<Ship>();

        navLine = newNavLine;
        speed = ship.ShipData.speed;
        turnRate = ship.ShipData.turnRate;

        navLinePoints = new Vector3[navLine.positionCount];
        navLine.GetPositions(navLinePoints);

        if (ship.Team == 0) {
            Array.Reverse(navLinePoints);
        }

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

    void OnTargetAcquired(ITargetable newTarget) {
        currentTarget = newTarget;
    }

    void OnTargetLost() {
        currentTarget = null;
    }

    void Update() {
        Vector3 movement = Vector3.zero;
        Vector3 targetDirection;
        Vector3 rotatedTargetDirection;
        Quaternion targetRotation;

        switch(currentState) {
            case ShipState.Attack:
                if (currentTarget != null) {
                    switch(ship.ShipData.moveType) {
                        case "capital":
                            targetDirection = (navLinePoints[navLineIndex] - transform.position).normalized;
                            rotatedTargetDirection = Quaternion.Euler(0, 0, 90) * targetDirection;
                            targetRotation = Quaternion.LookRotation(Vector3.forward, rotatedTargetDirection);

                            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * turnRate);
                            movement = transform.right * Time.deltaTime * speed;
                            break;
                        case "fighter":
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
                            movement = transform.right * Time.deltaTime * speed;
                            break;
                        case "strafing":
                            angleToTarget = Vector2.SignedAngle(transform.right, currentTarget.transform.position - transform.position);

                            transform.Rotate(0, 0, turnRate * Time.deltaTime * Mathf.Sign(angleToTarget), Space.Self);

                            movement = transform.up * Time.deltaTime * speed * Mathf.Sin(Time.time) / 4;
                            break;
                        default:
                            break;
                    }
                }
                break;
            case ShipState.Follow:
                if (Vector3.Distance(transform.position, navLinePoints[navLineIndex]) <= Mathf.Lerp(2, 0.1f, ship.ShipData.turnRate / 180)) {
                    if (navLineIndex == navLine.positionCount - 1) {
                        FollowComplete?.Invoke();
                    } else {
                        navLineIndex++;
                    }
                }

                targetDirection = (navLinePoints[navLineIndex] - transform.position).normalized;
                rotatedTargetDirection = Quaternion.Euler(0, 0, 90) * targetDirection;
                targetRotation = Quaternion.LookRotation(Vector3.forward, rotatedTargetDirection);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * turnRate);
                movement = transform.right * Time.deltaTime * speed;
                break;
            case ShipState.Idle:
                transform.Rotate(0, 0, turnRate * Time.deltaTime, Space.Self);
                movement = transform.right * Time.deltaTime * speed;
                break;
            default:
                break;
        }

        transform.Translate(movement, Space.World);
    }
}
