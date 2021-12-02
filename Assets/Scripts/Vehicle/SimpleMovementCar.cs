using System.Collections.Generic;
using UnityEngine;

//TODO: Fix code conventions. Class is messy. How can we fix this?
namespace XRLab.VRoem.Vehicle {

    public class SimpleMovementCar : Car {
        [Header("Normal Car Controls")]
        [Tooltip("Forward Speed on X axis")]
        [SerializeField] private float _speedOnX;
        [Tooltip("Back to orignal position after effects")]
        [SerializeField] private float _speedReturnZ = 10;
        [Tooltip("Look towards next rotation, and turn")]
        [SerializeField] private float _rotationSpeed = 5;
        [Tooltip("Minimal distance towards next position, works with controller gestures")]
        [SerializeField] private float _lookAtThreshold = 0.1f;
        [Tooltip("Upon arriving at next postition, lower drifting speed")]
        [SerializeField] private float _resetRotationSpeed = 5;
        [Tooltip("Change gravity upon ridign objects like ramps")]
        [SerializeField] private float _gravityMultiplierBasedOnUp = 2;
        [Tooltip("Mulitplier for speed on slopes")]
        [SerializeField] private float _normalBasedSpeedMultiplier = 0.5f;
        [Tooltip("Rotate the object with regard to the rotation of the object where the player is riding on")]
        [SerializeField] private float _normalSmoothing = 1.2f;
        [Tooltip("turning in mid air back to normal rotation")]
        [SerializeField] private float _rotInAirSpeed = 2; 
        [Tooltip("amount of depositioning on acceleration & decelaration")]
        [SerializeField] private float _dynamicToleranceZ = 0.05f;
        [Tooltip("Amount of depostioning on boosting")]
        [SerializeField] private float _boostMultiplierPosZ = 1.1f; 
        [Tooltip("Max distance the player can drive on X")]
        [SerializeField] private float _boundsX = 2.7f; 

        [SerializeField] private Transform _backCarBounds; 
        [SerializeField] private Transform _forwardTransform;
        [SerializeField] private Transform _model;
        [SerializeField] private LayerMask _groundLayerMask;
        [SerializeField] private float _raycastLength = 2;

        [Header("Wall Riding")]
        [Tooltip("gravity for wallriding")]
        [SerializeField] private float _gravityMultiplierOnWallsBasedOnUp = 4;  
        [SerializeField] private bool _grounded = false;
        [SerializeField] private float _groundAngle = 0;
        [SerializeField] private float _angleToRideWall = 45;

       
        [Header("drifting")]
        [Tooltip("'drifting' how fast the car's back turns away")]
        [SerializeField] private float _rollRotationSpeed = 5;
        [Tooltip("max look at for next position, to achieve drifting (Quaternions)")]
        [SerializeField] private float _maxTurnAngle = 0.5f; 
        [Tooltip("max rotation of the car, to simulate drifting")]
        [SerializeField] private float _maxRollTurn = 15;
        [Tooltip("sensitivity for starting the drift, the lower the number the faster to drift")]
        [SerializeField] private float _rollPercentModifier = 0.25f;
        [Tooltip("distance between player and next position, to achieve full rotation")]
        [SerializeField] private float _maxTurnDistance = 5;
        [Tooltip("distance between player and next position, to achieve full rotation")]
        [SerializeField] private float _minTurnDistance = 0.25f;
        private float turnPercentage;

        [Header("Raycasts on the wheels")]
        [SerializeField] private Transform _upperLeftRay;
        [SerializeField] private Transform _upperRightRay;
        [SerializeField] private Transform _lowerLeftRay;
        [SerializeField] private Transform _lowerRightRay;

        private Vector3 _targetPoint;
        private float _lockedPosZ = 3.5f;       //Z Position of the player exc mulitpliers
        private float _dynamicPosZ;             //Z postion of the player inc. multipliers
        private Rigidbody _rb;
        private SpeedManager _speedManager;
        private float _rigidBodyDrag = 0;
        

        private bool _overrideTargetPoint = false;
        public float GroundAngle { get { return _groundAngle; } }
        public bool Grounded { get { return _grounded; } }
        public bool CanMove { get; set; } = true;
        public SpeedManager GetSpeedManager { get { return _speedManager; } }
        public Rigidbody GetRigidbody { get { return _rb; } }

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

            if (!_grounded || !CanMove) { return; }

            if (InTargetPointRange()) {
                LookAtTarget(0);
                return;
            }
            LookAtTarget();
        }

        private void FixedUpdate() 
        {
            if (!_grounded || !CanMove) return;

            Vector3 _targetDirection = (_targetPoint - transform.position).normalized;

            if (_groundAngle < _angleToRideWall) {
                _targetDirection.y = 0;
            }
            turnPercentage = Vector3.Distance(transform.position, _targetDirection) / _maxTurnDistance;

            float force = _speedOnX * turnPercentage;
            float forceZ = _speedReturnZ * turnPercentage;

            if (!InTargetPointRange()) {
                _rb.AddForce(new Vector3(_targetDirection.x, _targetDirection.y, 0) * force);
            }
            
            _rb.AddForce(new Vector3(0, 0, _targetDirection.z) * (_targetDirection.z >= 0 ? force : forceZ));

            if (!_grounded || _groundAngle > _angleToRideWall) {
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
                float normalMultiplier = rampDot > 0 ? (1 + groundAnglePercentage) : Mathf.Abs(1 - groundAnglePercentage);

                _speedManager.NormalBasedSpeed(normalMultiplier);
                _rb.drag = _rigidBodyDrag;
                _rb.useGravity = _groundAngle < _angleToRideWall;
            } 
            else 
            {
                _rb.useGravity = true;
                _rb.drag = 0;
                _groundAngle = 0;

                if (transform.localRotation.eulerAngles.z != 180) 
                {
                    transform.localRotation = Quaternion.Slerp(transform.localRotation, new Quaternion(0, 0, 0, transform.localRotation.w), _rotInAirSpeed * Time.deltaTime);
                } else 
                {
                    transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, 0, 179);
                }
            }
        }

        private void UpdateCarBackBounds() 
        {
            _backCarBounds.position = new Vector3(0, _backCarBounds.position.y, Mathf.Clamp(transform.position.z, _lockedPosZ * 1 - _dynamicToleranceZ, _lockedPosZ * 1 + _dynamicToleranceZ) - 1.1f);
        }

        private void LookAtTarget() 
        {
            LookAtTarget(1);
        }

        private void LookAtTarget(float rotationPercentMultiplier) 
        {
            Vector3 lookat = _targetPoint;

            lookat.z = _dynamicPosZ + _speedManager.CurrentMultiplier;
            Quaternion targetRotation;
            if (rotationPercentMultiplier > 0) 
            {
                targetRotation = Quaternion.LookRotation(lookat - transform.position);
                _forwardTransform.rotation = Quaternion.Slerp(_forwardTransform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            } 
            else 
            {
                _forwardTransform.localRotation = Quaternion.Slerp(_forwardTransform.localRotation, new Quaternion(0, 0, 0, _forwardTransform.localRotation.w), _resetRotationSpeed * Time.deltaTime);
            }

            //Prevents X and Z flipping when Z is 1
            if (transform.localRotation.z > 0.99f || transform.localRotation.x == 1)
            {
                transform.localRotation = new Quaternion(0, 0, 0.99f, transform.localRotation.w);
            }
            else
            {
                transform.localRotation = new Quaternion(transform.localRotation.x, 0, transform.localRotation.z, transform.localRotation.w);
            }

            _forwardTransform.localRotation = new Quaternion(0, Mathf.Clamp(_forwardTransform.localRotation.y, -_maxTurnAngle, _maxTurnAngle), 0, _forwardTransform.localRotation.w);

            lookat.z = transform.position.z;

            float rollPercent = Mathf.Clamp01(turnPercentage - _rollPercentModifier);
            float roll = 0;

            if (rollPercent > _minTurnDistance)
            {
                roll = _forwardTransform.localRotation.y > 0 ? -_maxRollTurn * rollPercent * rotationPercentMultiplier : _maxRollTurn * rollPercent * rotationPercentMultiplier;
            }

            targetRotation = new Quaternion(0, 0, roll, _model.localRotation.w);
            _model.localRotation = Quaternion.Slerp(_model.localRotation, targetRotation, _rollRotationSpeed * Time.deltaTime);
        }

        public override void SetOrientation(Vector3 lookAtPosition, bool boosting) 
        {
            float multiplier = boosting ? _boostMultiplierPosZ : Mathf.Clamp(_speedManager.CurrentMultiplier, 1 - _dynamicToleranceZ, 1 + _dynamicToleranceZ);

            _dynamicPosZ = _lockedPosZ * multiplier;

            Vector3 heightCorrectedPoint = new Vector3(lookAtPosition.x, transform.position.y, _dynamicPosZ);

            if (_groundAngle >= _angleToRideWall)
            {
                heightCorrectedPoint = transform.TransformVector(heightCorrectedPoint);
            }
            else
            {
                heightCorrectedPoint.x = Mathf.Clamp(lookAtPosition.x, -_boundsX, _boundsX);
            }

            _targetPoint = heightCorrectedPoint;
        }

        private bool InTargetPointRange() 
        {
            return Vector3.Distance(transform.position, _targetPoint) <= _lookAtThreshold;
        }

        public void SetOverridenTargetPoint(float x) 
        {
            _targetPoint = new Vector3(Mathf.Clamp(x, -_boundsX, _boundsX), transform.position.y, transform.position.z);
            _overrideTargetPoint = true;
        }

        private void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_upperLeftRay.position, -transform.up * _raycastLength);
        }
    }
}
