using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projectile : MonoBehaviour {
    private bool damageDone;
    private Collider2D[] aoeContacts = new Collider2D[200];
    private ContactFilter2D aoeContactFilter = new ContactFilter2D();
    private float distanceTraveled;
    private float health;
    private float accumulatedSpeed;
    private float initialSpeed;
    private int team;
    private Ship target;
    [SerializeField]
    private SOProjectile projectileData;
    [SerializeField]
    private SOTeamColors TeamColors;

    public void Initialize(int newTeam, float speed, Vector3 right, SOProjectile projectile, Ship newTarget) {
        initialSpeed = speed;
        projectileData = projectile;
        target = newTarget;
        team = newTeam;

        accumulatedSpeed = initialSpeed;
        
        gameObject.layer = LayerMask.NameToLayer(team.ToString());

        GetComponent<SetMaterialProperties>().SetMaterial(0f, TeamColors.Hues[team], projectileData.sprite);
    }

    void Awake() {
        health = projectileData.health;
    }

    void Die() {
        if (Array.Exists(projectileData.flags, flag => flag == "AOE")) {
            Physics2D.OverlapCircleNonAlloc(transform.position, projectileData.aoeRange, aoeContacts, ~(1 <<LayerMask.NameToLayer(team.ToString())));

            aoeContacts
                .Select(collider => collider)
                .Where(collider => collider != null && collider.gameObject.layer != gameObject.layer && collider.name == "Health")
                .ToList()
                .ForEach(collider => {
                    collider.gameObject.GetComponent<Health>().Damage(Mathf.Clamp(Mathf.Lerp(projectileData.damage, 0, Vector2.Distance(collider.transform.position, transform.position) / projectileData.aoeRange), 0, projectileData.damage));
                });
        }

        Instantiate(projectileData.hitParticleSystemPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
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
                Die();
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
            Die();
        }
    }
}
