using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRLab.VRoem.Core;

//TODO: Add Namespace, fix curly braces
public class ObstacleBreakInteraction : PlayerObstacleInteraction
{
    [SerializeField] private float _forceMultiplier = 1;
    [SerializeField] private float _upForceMultiplier = 50;
    //[SerializeField] private float _explosionForceMultiplier = 1;
    [SerializeField] private float _waitForGroundCheck = 0.1f;
    //[SerializeField] private float _upwardsForce = 1;
    private Rigidbody _rb;
    private SpeedManager _speedManager;

    private void Start()
    {
        _speedManager = FindObjectOfType<SpeedManager>();
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

        transform.SetParent(null);

        float playerSpeed = _speedManager.FinalSpeed != 0 ? _speedManager.FinalSpeed : _speedManager.SpeedBeforeHit;

        float force = playerSpeed * _forceMultiplier;

        _rb.AddForce(Vector3.up * _upForceMultiplier, ForceMode.Impulse);
        _rb.AddForceAtPosition(collision.relativeVelocity.normalized * force, collision.contacts[0].point, ForceMode.Impulse);

        Invoke(nameof(AddMoveWithRoadComponent), _waitForGroundCheck);

        MoneySystem.currentMonney = MoneySystem.currentMonney - MoneySystem.MonneyToLose;
    }

    private void AddMoveWithRoadComponent()
    {
        gameObject.AddComponent<ObstacleMoveWithGroundPhysics>();
    }
}
