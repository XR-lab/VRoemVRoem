using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRLab.VRoem.Core;

public class ObstacleBreakInteraction : PlayerObstacleInteraction
{
    [SerializeField] private float _forceMultiplier = 1;
    [SerializeField] private float _explosionForceMultiplier = 1;
    //[SerializeField] private float _upwardsForce = 1;
    private MoveObstacle _moveObstacle;

    private void Start()
    {
        _moveObstacle = GetComponent<MoveObstacle>();
    }

    protected override void Interact(Collision collision)
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb == null) {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        else {
            rb.isKinematic = false;
        }
         
        _moveObstacle.enabled = false;

        rb.AddForce(transform.forward * _moveObstacle.Speed);
        rb.AddExplosionForce(_moveObstacle.Speed * _forceMultiplier, collision.contacts[0].point, _moveObstacle.Speed * _explosionForceMultiplier);

        Invoke(nameof(AddMoveWithRoadComponent), 0.2f);

        MoneySystem.currentMonney = MoneySystem.currentMonney - MoneySystem.MonneyToLose;
    }

    private void AddMoveWithRoadComponent()
    {
        gameObject.AddComponent<ObstacleMoveWithGroundPhysics>();
    }
}
