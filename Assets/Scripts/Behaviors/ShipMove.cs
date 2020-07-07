using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMove : MonoBehaviour {
    private float speed;
    private float turnRate;
    private Ship ship;
    private ShipState currentState;

    public void Initialize() {
        ship = GetComponent<Ship>();

        speed = ship.ShipData.speed;
        turnRate = ship.ShipData.turnRate;

        ship.StateChange += OnStateChange;
    }

    void OnStateChange(ShipState newState) {
        currentState = newState;
    }

    void Update() {
        switch(currentState) {
            case ShipState.Attack:
                transform.Rotate(0, 0, turnRate * Time.deltaTime, Space.Self);
                break;
            case ShipState.Idle:
                transform.Rotate(0, 0, turnRate * Time.deltaTime, Space.Self);
                transform.Translate(transform.right * Time.deltaTime * speed, Space.World);
                break;
            default:
                break;
        }
    }
}
