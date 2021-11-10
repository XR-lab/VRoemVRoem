using UnityEngine;

namespace XRLab.VRoem.Vehicle
{
    public class SimpleMovementCar : Car
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _speedReturnZ = 10;
        [SerializeField] private float _gravityMultiplierBasedOnUp = 2;
        [SerializeField] private float _gravityMultiplierOnWallsBasedOnUp = 4;
        [SerializeField] private float _lookAtThreshold = 0.1f;
        [SerializeField] private float _rotationSpeed = 5;
        [SerializeField] private float _dynamicToleranceZ = 0.05f;
        [SerializeField] private float _boostMultiplierPosZ = 1.1f;
        [SerializeField] private float _boundsX = 2.7f;
        [SerializeField] private float _lookAtExtraSpeed = 1.5f;
        [SerializeField] private float _minimumTurnSpeed = 0.25f;
        [SerializeField] private float _maxTurnSpeed = 1.5f;
        [SerializeField] private Transform _backCarBounds;
        [SerializeField] private Transform _model;
        [SerializeField] private LayerMask _groundLayerMask;
        [SerializeField] private float _raycastLength = 2;
        [SerializeField] private bool _grounded = false;
        [SerializeField] private float _groundAngle = 0;
        [SerializeField] private float _angleToRideWall = 45;
        [SerializeField] private float _rotInAirSpeed = 2;
        [SerializeField] private float _normalSmoothing = 1.2f;
        [SerializeField] private float _angleToLockControlsX = 80;
        [SerializeField] private float _upsideDownAngleToUnlockControlsX = 120;
        [SerializeField] private float _normalBasedSpeedMultiplier = 0.5f;
        [SerializeField] private Transform _upperLeftRay;
        [SerializeField] private Transform _upperRightRay;
        [SerializeField] private Transform _lowerLeftRay;
        [SerializeField] private Transform _lowerRightRay;

        private Vector3 _targetPoint;
        private float _lockedPosZ = 3.5f;
        private Rigidbody _rb;
        private SpeedManager _speedManager;       
        private float _rigidBodyDrag = 0;

        public float GroundAngle { get { return _groundAngle; } }
        public float AngleToLockControlsX { get { return _angleToLockControlsX; } }

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

            if (!_grounded) { return; }

            LookAtTarget();
        }

        private void FixedUpdate()
        {
            if (InTargetPointRange() || !_grounded) return;

            Vector3 _targetDirection = (_targetPoint - transform.position).normalized;

            if (_groundAngle < _angleToRideWall)
            {
                _targetDirection.y = 0;
            }           

            float turnPercentage = Mathf.Clamp(Mathf.Abs(_model.localRotation.y) / 0.25f, _minimumTurnSpeed, _maxTurnSpeed);

            float force = _speed * turnPercentage;
            float forceZ = _speedReturnZ * turnPercentage;

            _rb.AddForce(new Vector3(_targetDirection.x, _targetDirection.y, 0) * force);
            _rb.AddForce(new Vector3(0, 0, _targetDirection.z) * (_targetDirection.z >= 0 ? force :  forceZ));

            if (!_grounded || _groundAngle > _angleToRideWall)
            {
                force = !_grounded ? _gravityMultiplierBasedOnUp : _gravityMultiplierOnWallsBasedOnUp;

                //Sticks better to slopes
                _rb.AddForce(-transform.up * force);
            }
        }

        private void CheckGrounded()
        {
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
                Vector3 averageNormals = (hitUpLeft.normal + hitUpRight.normal + hitDownLeft.normal + hitDownRight.normal) / 4;
                transform.up -= (transform.up - averageNormals) * _normalSmoothing * Time.deltaTime;

                _groundAngle = Vector3.Angle(Vector3.up, averageNormals);

                float rampDot = Vector3.Dot(Vector3.forward, averageNormals);
                float groundAnglePercentage = Mathf.Clamp01(Mathf.Abs(transform.localRotation.x) / 0.5f) * _normalBasedSpeedMultiplier;
                float normalMultiplier = rampDot > 0 ? (1  + groundAnglePercentage) : Mathf.Abs(1 - groundAnglePercentage);

                _speedManager.NormalBasedSpeed(normalMultiplier);
                _rb.drag = _rigidBodyDrag;
                _rb.useGravity = _groundAngle < _angleToRideWall;
            }
            else
            {
                _rb.useGravity = true;
                _rb.drag = 0;
                _groundAngle = 0;

                transform.localRotation = Quaternion.Slerp(transform.localRotation, new Quaternion(0, 0, 0, transform.localRotation.w), _rotInAirSpeed * Time.deltaTime);
            }
        }

        private void UpdateCarBackBounds()
        {
            _backCarBounds.position = new Vector3(0, _backCarBounds.position.y, Mathf.Clamp(transform.position.z, _lockedPosZ * 1 - _dynamicToleranceZ, _lockedPosZ * 1 + _dynamicToleranceZ) - 1.1f);
        }

        private void LookAtTarget()
        {
            Vector3 lookat = _targetPoint;
            lookat.z += Mathf.Clamp(_speedManager.FinalSpeed, 0, _lookAtExtraSpeed);

            Quaternion targetRotation = Quaternion.LookRotation(lookat - transform.position);
            _model.rotation = Quaternion.Slerp(_model.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            transform.localRotation = new Quaternion(transform.localRotation.x, 0, transform.localRotation.z, transform.localRotation.w);
            _model.localRotation = new Quaternion(0, Mathf.Clamp(_model.localRotation.y, -0.5f, 0.5f), 0, _model.localRotation.w);
        }

        public override void SetOrientation(Vector3 lookAtPosition, bool boosting)
        {
            float multiplier = boosting ? _boostMultiplierPosZ : Mathf.Clamp(_speedManager.CurrentMultiplier, 1 - _dynamicToleranceZ, 1 + _dynamicToleranceZ);

            float dynamicPosZ = _lockedPosZ * multiplier;

            Vector3 heightCorrectedPoint = new Vector3(_groundAngle < _angleToLockControlsX || _groundAngle > _upsideDownAngleToUnlockControlsX ? Mathf.Clamp(lookAtPosition.x, -_boundsX, _boundsX) : transform.position.x, _groundAngle < _angleToRideWall || _groundAngle > _upsideDownAngleToUnlockControlsX ? transform.position.y : lookAtPosition.y, dynamicPosZ);
            _targetPoint = heightCorrectedPoint;
        }

        private bool InTargetPointRange()
        {
            return Vector3.Distance(transform.position, _targetPoint) <= _lookAtThreshold;
        }
    }
}
