using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Capturable : MonoBehaviour {
    public event Action<int> Captured;

    private bool captured = false;
    private CircleCollider2D circleCollider2D;
    private Collider2D[] colliderContacts = new Collider2D[20];
    private Dictionary<int, int> captureNumbers;
    private int currentTeam;
    private float captureInterval = 1;
    private float timeToTestCapture;

    public void Disable() {
        circleCollider2D.enabled = false;
        enabled = false;
    }

    public void ForceCapture(int newTeam) {
        captured = true;
        currentTeam = newTeam;

        Captured?.Invoke(currentTeam);
    }

    void Awake() {
        captureNumbers = new Dictionary<int, int>() { {0, 0}, {1, 0} };
        circleCollider2D = GetComponent<CircleCollider2D>();
        timeToTestCapture = captureInterval;
    }

    void Update() {
        timeToTestCapture -= Time.deltaTime;

        if (timeToTestCapture <= 0) {
            circleCollider2D.GetContacts(colliderContacts);

            Collider2D[] capturingShips = colliderContacts
                .Select(collider => collider)
                .Where(collider => collider != null && collider.name == "Health")
                .ToArray();

            captureNumbers[0] = 0;
            captureNumbers[1] = 0;
            for (int i = 0; i < capturingShips.Length; i++) {
                Ship capturingShip = capturingShips[i].GetComponentInParent<Ship>();

                captureNumbers[capturingShip.Team] += 1;
            }

            if (captureNumbers[0] > captureNumbers[1] && (currentTeam == 1 || !captured)) {
                captured = true;
                currentTeam = 0;
                Captured?.Invoke(currentTeam);
            }
            if (captureNumbers[1] > captureNumbers[0] && (currentTeam == 0 || !captured)) {
                captured = true;
                currentTeam = 1;
                Captured?.Invoke(currentTeam);
            }

            timeToTestCapture = captureInterval;
        }
    }
}
