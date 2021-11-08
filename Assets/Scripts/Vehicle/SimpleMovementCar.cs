using UnityEngine;

namespace XRLab.VRoem.Vehicle
{
    public class SimpleMovementCar : Car
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _lookAtThreshold = 0.1f;
        [SerializeField] private float _rotationSpeed = 5;
        [SerializeField] private float _dynamicToleranceZ = 0.05f;
        [SerializeField] private float _boostMultiplierPosZ = 1.1f;
        [SerializeField] private float _boundsX = 3.8f;
        [SerializeField] private Transform _backCarBounds;
        [SerializeField] private Transform _model;
        [SerializeField] private LayerMask _groundLayerMask;
        [SerializeField] private float _raycastLength = 2;
        [SerializeField] private bool _grounded = false;
        [SerializeField] private float _groundAngle = 0;
        [SerializeField] private Transform _upperLeftRay;
        [SerializeField] private Transform _upperRightRay;
        [SerializeField] private Transform _lowerLeftRay;
        [SerializeField] private Transform _lowerRightRay;

        private Vector3 _targetPoint;
        private float _lockedPosZ = 3.5f;
        private Rigidbody _rb;
        private SpeedManager _speedManager;       
        private float _rigidBodyDrag = 0;

        public bool Grounded { get { return _grounded; } }

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();           
            _lockedPosZ = transform.position.z;
            _speedManager = FindObjectOfType<SpeedManager>();
            _rigidBodyDrag = _rb.drag;
        }

        private void Update()
        {
            CheckGrounded();
            UpdateCarBackBounds();

            //Doesn't look at target point when not on the ground
            if (!_grounded) { return; }

            LookAtTarget();
        }

        private void FixedUpdate()
        {
            //Remove control when distance to raycast and car is too low or the car is not on the ground
            if (InTargetPointRange() || !_grounded) return;

            Vector3 _targetDirection = (_targetPoint - transform.position).normalized;

            float turnPercentage = Mathf.Clamp(Mathf.Abs(_model.localRotation.y) / 0.25f, 0.25f, 1.5f);

            float force = _speed * turnPercentage;

            _rb.AddForce(_targetDirection * force);
        }

        private void CheckGrounded()
        {
            //4 raycasts at all corners of the car
            RaycastHit hitUpLeft;
            RaycastHit hitUpRight;
            RaycastHit hitDownLeft;
            RaycastHit hitDownRight;

            bool upLeftHit = Physics.Raycast(_upperLeftRay.position, -transform.up, out hitUpLeft, _raycastLength, _groundLayerMask);
            bool upRightHit = Physics.Raycast(_upperRightRay.position, -transform.up, out hitUpRight, _raycastLength, _groundLayerMask);
            bool downLeftHit = Physics.Raycast(_lowerLeftRay.position, -transform.up, out hitDownLeft, _raycastLength, _groundLayerMask);
            bool downRightHit = Physics.Raycast(_lowerRightRay.position, -transform.up, out hitDownRight, _raycastLength, _groundLayerMask);

            _grounded = upLeftHit || upRightHit || downLeftHit || downRightHit;

            if (_grounded)
            {
                //Average all normals to get a ground angle
                Vector3 averageNormals = (hitUpLeft.normal + hitUpRight.normal + hitDownLeft.normal + hitDownRight.normal) / 4;

                _groundAngle = Vector3.Angle(Vector3.up, averageNormals);
                _rb.drag = _rigidBodyDrag;
            }
            else
            {
                _rb.drag = 0;
                _groundAngle = 0;
            }
        }

        private void UpdateCarBackBounds()
        {
            //Car has an invisible collsion box behind him at all times so that he can go up ramps with ease
            _backCarBounds.position = new Vector3(0, _backCarBounds.position.y, Mathf.Clamp(transform.position.z, _lockedPosZ * 1 - _dynamicToleranceZ, _lockedPosZ * 1 + _dynamicToleranceZ) - 1.2f);
        }

        private void LookAtTarget()
        {
            //Look at direction of the controller raycast with the speed of the speed manager added so that the car will look forward
            Vector3 lookat = _targetPoint;
            lookat.z += Mathf.Clamp(_speedManager.ModifiedSpeed, 0, 1.5f);

            Quaternion targetRotation = Quaternion.LookRotation(lookat - transform.position);
            _model.rotation = Quaternion.Slerp(_model.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            //Lock all not wanted local rotation axes
            transform.localRotation = new Quaternion(transform.localRotation.x, 0, 0, transform.localRotation.w);
            _model.localRotation = new Quaternion(0, Mathf.Clamp(_model.localRotation.y, -0.5f, 0.5f), 0, _model.localRotation.w);
        }

        public override void SetOrientation(Vector3 lookAtPosition, bool boosting)
        {
            //Changes target point Z based on speed so that the car will move towards it
            float multiplier = boosting ? _boostMultiplierPosZ : Mathf.Clamp(_speedManager.CurrentMultiplier, 1 - _dynamicToleranceZ, 1 + _dynamicToleranceZ);

            float dynamicPosZ = _lockedPosZ * multiplier;

            //Correct Controller Raycast hit point so that the car will not follow the Y and Z of the hit point
            Vector3 heightCorrectedPoint = new Vector3(Mathf.Clamp(lookAtPosition.x, -_boundsX, _boundsX), transform.position.y, dynamicPosZ);
            _targetPoint = heightCorrectedPoint;
        }

        private bool InTargetPointRange()
        {
            //Check distance between raycast hit point and the car
            return Vector3.Distance(transform.position, _targetPoint) <= _lookAtThreshold;
        }
    }
}
