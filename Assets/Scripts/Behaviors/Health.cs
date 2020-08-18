using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    public event Action Died;

    public bool EnableHealthbar;
    public float Hitpoints;
    public Vector3 HealthbarOffset;

    private bool isDead = false;
    private float startingHitpoints;
    private SpriteRenderer healthBackground;
    private SpriteRenderer healthOverlay;

    public void Damage(float amount) {
        if (!isDead) {
            Hitpoints -= amount;

            if (Hitpoints <= 0) {
                isDead = true;
            }
        }
    }

    public void Initialize(float hitpoints) {
        Hitpoints = hitpoints;
        startingHitpoints = hitpoints;

        gameObject.layer = LayerMask.NameToLayer(GetComponentInParent<ITargetable>().Team.ToString());
    }

    void Awake() {
        healthBackground = transform.Find("HealthBackground").GetComponent<SpriteRenderer>();
        healthOverlay = transform.Find("HealthOverlay").GetComponent<SpriteRenderer>();

        if (!EnableHealthbar) {
            healthBackground.enabled = false;
            healthOverlay.enabled = false;
        } else {
            healthBackground.transform.Translate(HealthbarOffset);
            healthOverlay.transform.Translate(HealthbarOffset);
        }
    }

    void Update() {
        if (isDead) {
            Died.Invoke();
        }

        if (EnableHealthbar) {
            healthOverlay.transform.localScale = new Vector3(Hitpoints / startingHitpoints, 1, 1);
        }
    }
}
