using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {
    public float timeScale = 1;

    void Update() {
        if (Time.timeScale != timeScale) {
            Time.timeScale = timeScale;
        }
    }
}
