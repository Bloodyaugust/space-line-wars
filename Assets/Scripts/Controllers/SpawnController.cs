using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {
    public int NumberShips;
    public GameObject ShipPrefab;

    void Start() {
        for (int i = 0; i < NumberShips; i++) {
            Instantiate(ShipPrefab, new Vector3(Random.Range(-2f, 2f), Random.Range(-1f, 1f), 0), Quaternion.identity);
        }
    }
}
