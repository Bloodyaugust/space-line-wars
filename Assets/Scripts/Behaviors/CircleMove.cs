using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMove : MonoBehaviour {
    public SOShip shipData;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        transform.Rotate(0, 0, -180 * Time.deltaTime, Space.Self);
        transform.Translate(transform.up * Time.deltaTime * shipData.speed, Space.World);
    }
}
