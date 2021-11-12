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
        [SerializeField] private float _rollRotationSpeed = 5;
        [SerializeField] private float _dynamicToleranceZ = 0.05f;
        [SerializeField] private float _boostMultiplierPosZ = 1.1f;
        [SerializeField] private float _boundsX = 2.7f;
        [SerializeField] private float _lookAtMultiplier = 0.5f;
        [SerializeField] private float _minimumTurnSpeed = 0.25f;
        [SerializeField] private float _maxTurnSpeed = 1.5f;
        [SerializeField] private Transform _backCarBounds;
        [SerializeField] private Transform _forwardTransform;
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
        [SerializeField] private float _maxTurnAngle = 0.5f;
        [SerializeField] private float _maxRollTurn = 15;
        [SerializeField] private float _rollPercentModifier = 0.25f;
        [SerializeField] private float _maxTurnDistance = 5;
        [SerializeField] private Transform _upperLeftRay;
        [SerializeField] private Transform _upperRightRay;
        [SerializeField] private Transform _lowerLeftRay;
        [SerializeField] private Transform _lowerRightRay;

        private Vector3 _targetPoint;
        private float _lockedPosZ = 3.5f;
        private Rigidbody _rb;
        private SpeedManager _speedManager;
        private float _rigidBodyDrag = 0;
        private float _dynamicPosZ;
        private float turnPercentage;
        private bool _overrideTargetPoint = false;

        public float GroundAngle { get { return _groundAngle; } }
        public float AngleToLockControlsX { get { return _angleToLockControlsX; } }
        public bool Grounded { get { return _grounded; } }
        public bool CanMove { get; set; } = true;
        public SpeedManager GetSpeedManager { get { return _speedManager; } }
        public Rigidbody GetRigidbody { get { return _rb; } }

        private void Start() {
            _rb = GetComponent<Rigidbody>();
            _lockedPosZ = transform.position.z;
            _speedManager = FindObjectOfType<SpeedManager>();
            _rigidBodyDrag = _rb.drag;
        }

        private void Update() {
            CheckGrounded();
            UpdateCarBackBounds();

            if (!_grounded || !CanMove) { return; }

            if (InTargetPointRange()) {
                LookAtTarget(transform.position);
                return;
            }

            LookAtTarget();

            //turnPercentage = Mathf.Clamp(Mathf.Abs(_forwardTransform.localRotation.y) / (_maxTurnAngle / 2), _minimumTurnSpeed, _maxTurnSpeed);
        }

        private void FixedUpdate() {
            if (InTargetPointRange() || !_grounded || !CanMove) return;

            Vector3 _targetDirection = (_targetPoint - transform.position).normalized;

            if (_groundAngle < _angleToRideWall) {
                _targetDirection.y = 0;
            }
            turnPercentage = Vector3.Distance(transform.position, _targetDirection) / _maxTurnDistance;

            float force = _speed * turnPercentage;
            float forceZ = _speedReturnZ * turnPercentage;

            _rb.AddForce(new Vector3(_targetDirection.x, _targetDirection.y, 0) * force);
            _rb.AddForce(new Vector3(0, 0, _targetDirection.z) * (_targetDirection.z >= 0 ? force : forceZ));

            if (!_grounded || _groundAngle > _angleToRideWall) {
                force = !_grounded ? _gravityMultiplierBasedOnUp : _gravityMultiplierOnWallsBasedOnUp;

                //Sticks better to slopes
                _rb.AddForce(-transform.up * force);
            }
        }

        private void CheckGrounded() {
            RaycastHit hitUpLeft;
            RaycastHit hitUpRight;
            RaycastHit hitDownLeft;
            RaycastHit hitDownRight;

            bool upLeftHit = Physics.Raycast(_upperLeftRay.position, -transform.up, out hitUpLeft, _raycastLength, _groundLayerMask);
            bool upRightHit = Physics.Raycast(_upperRightRay.position, -transform.up, out hitUpRight, _raycastLength, _groundLayerMask);
            bool downLeftHit = Physics.Raycast(_lowerLeftRay.position, -transform.up, out hitDownLeft, _raycastLength, _groundLayerMask);
            bool downRightHit = Physics.Raycast(_lowerRightRay.position, -transform.up, out hitDownRight, _raycastLength, _groundLayerMask);

            _grounded = upLeftHit || upRightHit || downLeftHit || downRightHit;

            if (_grounded) {
                Vector3 averageNormals = (hitUpLeft.normal + hitUpRight.normal + hitDownLeft.normal + hitDownRight.normal) / 4;
                transform.up -= (transform.up - averageNormals) * _normalSmoothing * Time.deltaTime;

                _groundAngle = Vector3.Angle(Vector3.up, averageNormals);

                float rampDot = Vector3.Dot(Vector3.forward, averageNormals);
                float groundAnglePercentage = Mathf.Clamp01(Mathf.Abs(transform.localRotation.x) / 0.5f) * _normalBasedSpeedMultiplier;
                float normalMultiplier = rampDot > 0 ? (1 + groundAnglePercentage) : Mathf.Abs(1 - groundAnglePercentage);

                _speedManager.NormalBasedSpeed(normalMultiplier);
                _rb.drag = _rigidBodyDrag;
                _rb.useGravity = _groundAngle < _angleToRideWall;
            } else {
                _rb.useGravity = true;
                _rb.drag = 0;
                _groundAngle = 0;

                transform.localRotation = Quaternion.Slerp(transform.localRotation, new Quaternion(0, 0, 0, transform.localRotation.w), _rotInAirSpeed * Time.deltaTime);
            }
        }

        private void UpdateCarBackBounds() {
            _backCarBounds.position = new Vector3(0, _backCarBounds.position.y, Mathf.Clamp(transform.position.z, _lockedPosZ * 1 - _dynamicToleranceZ, _lockedPosZ * 1 + _dynamicToleranceZ) - 1.1f);
        }

        private void LookAtTarget() {
            LookAtTarget(_targetPoint);
        }

        private void LookAtTarget(Vector3 lookat) {
            lookat.z = _dynamicPosZ + _speedManager.CurrentMultiplier;

            Quaternion targetRotation = Quaternion.LookRotation(lookat - transform.position);
            _forwardTransform.rotation = Quaternion.Slerp(_forwardTransform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            transform.localRotation = new Quaternion(transform.localRotation.x, 0, transform.localRotation.z, transform.localRotation.w);
            _forwardTransform.localRotation = new Quaternion(0, Mathf.Clamp(_forwardTransform.localRotation.y, -_maxTurnAngle, _maxTurnAngle), 0, _forwardTransform.localRotation.w);

            lookat.z = transform.position.z;
            float rollPercent = Mathf.Clamp01(turnPercentage - _rollPercentModifier);

            targetRotation = new Quaternion(0, 0, _forwardTransform.localRotation.y > 0 ? -_maxRollTurn * rollPercent : _maxRollTurn * rollPercent, _model.localRotation.w);
            _model.localRotation = Quaternion.Slerp(_model.localRotation, targetRotation, _rollRotationSpeed * Time.deltaTime);
            //_forwardTransform.localRotation = new Quaternion(0, Mathf.Clamp(_forwardTransform.localRotation.y, -_maxTurnAngle, _maxTurnAngle), 0, _forwardTransform.localRotation.w);
        }

        public override void SetOrientation(Vector3 lookAtPosition, bool boosting) {
            float multiplier = boosting ? _boostMultiplierPosZ : Mathf.Clamp(_speedManager.CurrentMultiplier, 1 - _dynamicToleranceZ, 1 + _dynamicToleranceZ);

            _dynamicPosZ = _lockedPosZ * multiplier;

            Vector3 heightCorrectedPoint = new Vector3(_groundAngle < _angleToLockControlsX || _groundAngle > _upsideDownAngleToUnlockControlsX ? Mathf.Clamp(lookAtPosition.x, -_boundsX, _boundsX) : transform.position.x, _groundAngle < _angleToRideWall || _groundAngle > _upsideDownAngleToUnlockControlsX ? transform.position.y : lookAtPosition.y, _dynamicPosZ);
            _targetPoint = heightCorrectedPoint;
        }

        private bool InTargetPointRange() {
            return Vector3.Distance(transform.position, _targetPoint) <= _lookAtThreshold;
        }

        public void SetOverridenTargetPoint(float x) {
            _targetPoint = new Vector3(Mathf.Clamp(x, -_boundsX, _boundsX), transform.position.y, transform.position.z);
            _overrideTargetPoint = true;
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_upperLeftRay.position, -transform.up * _raycastLength);
        }
    }
}
