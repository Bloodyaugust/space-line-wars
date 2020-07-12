using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projectile : MonoBehaviour {
    private bool damageDone;
    private float distanceTraveled;
    private float health;
    private float initialSpeed;
    private int team;
    [SerializeField]
    private SOProjectile projectileData;

    public void Initialize(int newTeam, float speed, Vector3 right, SOProjectile projectile) {
        initialSpeed = speed;
        projectileData = projectile;
        team = newTeam;
        
        gameObject.layer = LayerMask.NameToLayer(team.ToString());
    }

    void Awake() {
        health = projectileData.health;
    }
    
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.name == "Health") {
            Ship colliderShip = collider.gameObject.GetComponentInParent<Ship>();

            if (colliderShip.Team != team) {
                if (Array.Exists(projectileData.flags, flag => flag == "OneShot")) {
                    if (!damageDone) {
                        colliderShip.GetComponentInChildren<Health>().Damage(projectileData.damage);
                    }
                } else {
                    colliderShip.GetComponentInChildren<Health>().Damage(projectileData.damage);
                }

                damageDone = true;
                Destroy(gameObject);
            }
        }
    }

    void Update() {
        switch (projectileData.moveType) {
            case "ballistic":
                Vector2 movementVector = transform.right * (projectileData.speed + initialSpeed) * Time.deltaTime;

                transform.Translate(movementVector, Space.World);
                distanceTraveled += movementVector.magnitude;
                break;
            default:
                break;
        }

        if (distanceTraveled >= projectileData.range || health <= 0) {
            Destroy(gameObject);
        }
    }
}
