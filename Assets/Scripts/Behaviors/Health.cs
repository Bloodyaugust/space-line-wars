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
    private float currentShield;
    private IHealthy healthyInterface;
    private SpriteRenderer healthBackground;
    private SpriteRenderer healthOverlay;

    public void Damage(float amount) {
        float damageRemaining = amount;

        if (!isDead) {
            if (currentShield > 0) {
                if (currentShield <= damageRemaining) {
                    damageRemaining -= currentShield;
                    currentShield = 0;
                } else {
                    currentShield -= damageRemaining;
                    damageRemaining = 0;
                }
            }

            if (damageRemaining > 0) {
                damageRemaining = Mathf.Clamp(damageRemaining - healthyInterface.Armor, 1, damageRemaining);
            }

            if (damageRemaining > 0) {
                Hitpoints -= damageRemaining;
            }

            Debug.Log($"{damageRemaining.ToString()}, {amount}, {healthyInterface.Armor}, {currentShield}");

            if (Hitpoints <= 0) {
                isDead = true;
            }
        }
    }

    public void Initialize(IHealthy HealthyInterface) {
        healthyInterface = HealthyInterface;

        Hitpoints = healthyInterface.Health;
        currentShield = healthyInterface.Shield;

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
            healthOverlay.transform.localScale = new Vector3(Hitpoints / healthyInterface.Health, 1, 1);
        }

        if (currentShield < healthyInterface.Shield) {
            currentShield += healthyInterface.ShieldRegen * Time.deltaTime;
            currentShield = Mathf.Clamp(currentShield, 0, healthyInterface.Shield);
        }
    }
}
