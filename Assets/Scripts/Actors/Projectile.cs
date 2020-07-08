using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
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
                colliderShip.GetComponentInChildren<Health>().Damage(projectileData.damage);
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
