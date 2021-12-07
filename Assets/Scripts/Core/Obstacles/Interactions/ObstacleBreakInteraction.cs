using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRLab.VRoem.Core;

//TODO: Add Namespace, fix curly braces
public class ObstacleBreakInteraction : PlayerObstacleInteraction
{
    [SerializeField] private float _forceMultiplier = 0.5f;
    [SerializeField] private float _upForceMultiplier = 25;
    [SerializeField] private float _policeUpForceMultiplier = 10;
    //[SerializeField] private float _explosionForceMultiplier = 1;
    [SerializeField] private float _waitForGroundCheck = 0.1f;
    [SerializeField] private bool _policeOnly = true;
    [SerializeField] private BoxCollider _boxCollider;
    //[SerializeField] private float _upwardsForce = 1;

    private Rigidbody _rb;
    private SpeedManager _speedManager;
    private MoveObstacle _moveObstacle;
    private float _policeBackForce = 12;

    private void Start()
    {
        _speedManager = FindObjectOfType<SpeedManager>();
        _moveObstacle = GetComponent<MoveObstacle>();

        if (_policeOnly)
        {
            _boxCollider.enabled = false;
        }
    }

    protected override void Interact(Collision collision)
    {
        if (_policeOnly)
        {
            return;
        }

        BlowAway(collision.relativeVelocity.normalized, collision.contacts[0].point, _upForceMultiplier, _speedManager.FinalSpeed);
    }

    protected override void RunOver(bool lookingForward)
    {
        _boxCollider.enabled = true;
        BlowAway(lookingForward ? Vector3.forward : -Vector3.forward, transform.position, _policeUpForceMultiplier, lookingForward ? _speedManager.FinalSpeed : _policeBackForce);
    }

    private void BlowAway(Vector3 dir, Vector3 point, float upForceMultiplier, float carSpeed)
    {
        if (_moveObstacle != null)
        {
            _moveObstacle.enabled = false;
        }
        
        _rb = GetComponent<Rigidbody>();

        if (_rb == null)
        {
            _rb = gameObject.AddComponent<Rigidbody>();
        }
        else
        {
            _rb.isKinematic = false;
        }

        transform.SetParent(null);

        float playerSpeed = carSpeed != 0 ? carSpeed : _speedManager.SpeedBeforeHit;

        float force = playerSpeed * _forceMultiplier;

        _rb.AddForce(Vector3.up * upForceMultiplier, ForceMode.Impulse);
        _rb.AddForceAtPosition(dir * force, point, ForceMode.Impulse);

        Invoke(nameof(AddMoveWithRoadComponent), _waitForGroundCheck);
    }

    private void AddMoveWithRoadComponent()
    {
        gameObject.AddComponent<ObstacleMoveWithGroundPhysics>();
    }
}
