using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {
    public int NumberShips;
    public GameObject ShipPrefab;

    void Start() {
        for (int i = 0; i < NumberShips; i++) {
            GameObject newShip = Instantiate(ShipPrefab, new Vector3(Random.Range(-20f, 20f), Random.Range(-10f, 10f), 0), Quaternion.identity);

            newShip.GetComponent<Ship>().Team = i % 2;
        }
    }
}
