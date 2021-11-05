using System.Collections;
using UnityEngine;
using XRLab.VRoem.Core;

namespace XRLab.VRoem.Vehicle
{
    public class CarControllerV2 : MonoBehaviour
    {
        [SerializeField] private Transform _handAnchor;
        [SerializeField] private float _speedMultiplierAdded = 0.2f;
        [SerializeField] private float _boostDuration = 1f;
        [SerializeField] private float _boostCooldown = 5f;
        [SerializeField] private float _fillBoostSpeed = 0.25f;
        [SerializeField] private float _boostTimer = 1;
        [SerializeField] private LayerMask _layer;
        [SerializeField] private Color _rayColor = Color.red;

        private SimpleMovementCar _car;
        private LineRenderer _lineRenderer;
        private SpeedManager _speedManager;
        private Transform _vrCam;
        private bool _mouseControl = false;
        private bool _boosting = false;
        private bool _boostInCooldown = false;
        private bool _canBoost = true;

        private void Start()
        {
            _car = GetComponent<SimpleMovementCar>();
            _lineRenderer = GetComponent<LineRenderer>();
            _speedManager = FindObjectOfType<SpeedManager>();
            _boostTimer = _boostDuration;
            _vrCam = GameObject.FindGameObjectWithTag(Tags.OVR).transform;
            _fillBoostSpeed = 1 / (1 / _boostDuration * _boostCooldown);
        }


        private void ShootControlRay()
        {

        }

        private void BoostCheck()
        {

        }

    }
}
