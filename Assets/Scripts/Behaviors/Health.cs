using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    public event Action Died;

    public float Hitpoints;

    private bool isDead = false;
    private float startingHitpoints;

    public void Damage(float amount) {
        if (!isDead) {
            Hitpoints -= amount;

            if (Hitpoints <= 0) {
                isDead = true;
                Died?.Invoke();
            }
        }
    }

    public void Initialize(float hitpoints) {
        Hitpoints = hitpoints;
        startingHitpoints = hitpoints;

        gameObject.layer = LayerMask.NameToLayer(GetComponentInParent<Ship>().Team.ToString());
    }
}
