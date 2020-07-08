using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetAcquisition : MonoBehaviour {
    public event Action<Ship> TargetAcquired;
    public event Action TargetLost;

    private bool loseTargetOnRangeExit;
    private Collider2D[] potentialTargets = new Collider2D[20];
    private ContactFilter2D contactFilter = new ContactFilter2D();
    private float range;
    private List<Ship> possibleTargets;
    private Rigidbody2D r2D;
    private Ship currentTarget;
    private Ship parentShip;

    public void Initialize(float newRange, bool respectRangeForTargetKeeping) {
        GetComponent<CircleCollider2D>().radius = newRange;
        range = newRange;
        loseTargetOnRangeExit = respectRangeForTargetKeeping;
    }

    void Awake() {
        r2D = GetComponent<Rigidbody2D>();
        parentShip = GetComponentInParent<Ship>();

        possibleTargets = new List<Ship>();
    }

    void FindTarget() {
        r2D.GetContacts(potentialTargets);

        Collider2D selectedCollider = potentialTargets
            .Select(collider => collider)
            .Where(collider => collider != null && collider.gameObject.layer != gameObject.layer)
            .FirstOrDefault(collider => collider.name == "Health" && Vector3.Distance(transform.position, collider.transform.position) <= range);


        if (selectedCollider != null) {
            currentTarget = selectedCollider.GetComponentInParent<Ship>();
            TargetAcquired?.Invoke(currentTarget);
            r2D.simulated = false;

            currentTarget.Died += Untarget;
        }
    }

    void OnDestroy() {
        if (currentTarget != null) {
            currentTarget.Died -= Untarget;
        }
    }

    void Start() {
        contactFilter.layerMask = LayerMask.NameToLayer(parentShip.Team.ToString());
        contactFilter.useLayerMask = true;
        contactFilter.useTriggers = true;
        gameObject.layer = LayerMask.NameToLayer(parentShip.Team.ToString());
    }

    void Untarget() {
        TargetLost?.Invoke();

        currentTarget.Died -= Untarget;
        currentTarget = null;
        r2D.simulated = true;
    }

    void Update() {
        if (currentTarget != null && Vector3.Distance(transform.position, currentTarget.transform.position) > range && loseTargetOnRangeExit) {
            Untarget();
        }
    }

    void FixedUpdate() {
        if (currentTarget == null) {
            FindTarget();
        }
    }
}
