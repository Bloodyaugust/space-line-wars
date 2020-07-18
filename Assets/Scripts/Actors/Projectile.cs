using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projectile : MonoBehaviour {
    private bool damageDone;
    private float distanceTraveled;
    private float health;
    private float accumulatedSpeed;
    private float initialSpeed;
    private int team;
    private Ship target;
    [SerializeField]
    private SOProjectile projectileData;

    public void Initialize(int newTeam, float speed, Vector3 right, SOProjectile projectile, Ship newTarget) {
        initialSpeed = speed;
        projectileData = projectile;
        target = newTarget;
        team = newTeam;

        accumulatedSpeed = initialSpeed;
        
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
        Vector2 movementVector;

        switch (projectileData.moveType) {
            case "ballistic":
                movementVector = transform.right * (projectileData.speed + initialSpeed) * Time.deltaTime;

                transform.Translate(movementVector, Space.World);
                distanceTraveled += movementVector.magnitude;
                break;
            case "missile":
                if (target != null) {
                    Vector3 targetDirection = (target.transform.position - transform.position).normalized;
                    Vector3 rotatedTargetDirection = Quaternion.Euler(0, 0, 90) * targetDirection;
                    Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, rotatedTargetDirection);

                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * projectileData.rotationSpeed);
                }

                accumulatedSpeed += Time.deltaTime * projectileData.speed;
                movementVector = transform.right * accumulatedSpeed * Time.deltaTime;

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
