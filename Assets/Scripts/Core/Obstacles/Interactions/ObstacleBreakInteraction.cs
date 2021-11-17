using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRLab.VRoem.Core;

public class ObstacleBreakInteraction : PlayerObstacleInteraction
{
    [SerializeField] private float _forceMultiplier = 1;
    [SerializeField] private float _upForceMultiplier = 50;
    [SerializeField] private float _explosionForceMultiplier = 1;
    [SerializeField] private float _waitForGroundCheck = 0.1f;
    //[SerializeField] private float _upwardsForce = 1;
    private MoveObstacle _moveObstacle;
    private Rigidbody _rb;

    private void Start()
    {
        _moveObstacle = GetComponent<MoveObstacle>();
    }

    protected override void Interact(Collision collision)
    {
        _rb = GetComponent<Rigidbody>();

        if (_rb == null) {
            _rb = gameObject.AddComponent<Rigidbody>();
        }
        else {
            _rb.isKinematic = false;
        }
         
        _moveObstacle.enabled = false;

        _rb.AddForce(transform.forward * _moveObstacle.Speed);
        _rb.AddForce(Vector3.up * _upForceMultiplier);
        _rb.AddExplosionForce(_moveObstacle.Speed * _forceMultiplier, collision.contacts[0].point, _moveObstacle.Speed * _explosionForceMultiplier);

        Invoke(nameof(AddMoveWithRoadComponent), _waitForGroundCheck);

        MoneySystem.currentMonney = MoneySystem.currentMonney - MoneySystem.MonneyToLose;
    }

    private void AddMoveWithRoadComponent()
    {
        gameObject.AddComponent<ObstacleMoveWithGroundPhysics>();
    }
}
