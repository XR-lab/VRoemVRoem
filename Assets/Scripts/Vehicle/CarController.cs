using System.Collections;
using UnityEngine;
using UnityEngine.XR;

namespace XRLab.VRoem.Vehicle {
    public class CarController : MonoBehaviour {
        [SerializeField] private Transform _handAnchor;
        [SerializeField] private Transform _leftHandAnchor;
        [SerializeField] private float _speedMultiplierAdded = 0.2f;
        [SerializeField] private float _boostDuration = 1f;
        [SerializeField] private float _boostCooldown = 5f;
        [SerializeField] private float _fillBoostSpeed = 0.25f;
        [SerializeField] private float _tresholdToAccelerate = 0.8f;
        [SerializeField] private float _tresholdToDeccelerate = 0.2f;
        [SerializeField] private Vector3 _positionModifier;
        [SerializeField] private float _boostTimer = 1;
        [SerializeField] private LayerMask _layer;
        [SerializeField] private Color _rayColor = Color.red;
        [SerializeField] private float _maxHandDistance = 5;

        //added by Bouke
        private float _boosttime;

        private SimpleMovementCar _car;
        private LineRenderer _lineRenderer;
        private SpeedManager _speedManager;
        private Transform _vrCam;
        private bool _mouseControl = false;
        private bool _straightRay = true;
        [HideInInspector]public bool _boosting = false;
        private bool _boostInCooldown = false;

        private Vector3 _lastValidHandPosition;        

        private void Start() {
            _car = GetComponent<SimpleMovementCar>();
            _lineRenderer = GetComponent<LineRenderer>();
            _speedManager = FindObjectOfType<SpeedManager>();
            _boostTimer = _boostDuration;
            _vrCam = GameObject.FindGameObjectWithTag("OVR").transform;

            //When releasing boost before the boost meter is empty this variable will fill it up instead of the normal cooldown
            //It does a calculation to make sure it will take as long as the given cooldown
            _fillBoostSpeed = 1 / (1 / _boostDuration * _boostCooldown);
        }


        private void Update() {
            ShootControlRay();
            //BoostCheck();
            deactivateboost();
        }

        private void ShootControlRay() {

            if (Vector3.Distance(_leftHandAnchor.position, _handAnchor.position) < _maxHandDistance)
            {
                _lastValidHandPosition = _handAnchor.position + (_leftHandAnchor.position - _handAnchor.position) / 2;
            }

            //Shoot Ray from right hand or mouse
            Ray ray = new Ray(_lastValidHandPosition, Vector3.forward);
            RaycastHit hit;
            Ray rayAccel = new Ray(_lastValidHandPosition, _handAnchor.forward);
            RaycastHit hitAccel;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layer)) {
                float diffHandCam = _lastValidHandPosition.y - Camera.main.transform.position.y;
                diffHandCam *= _positionModifier.y;

                //Send hit point to car
                float diffY = _car.transform.position.y + diffHandCam;

                _car.SetOrientation(new Vector3(hit.point.x * _positionModifier.x, _car.GroundAngle < _car.AngleToLockControlsX || _car.GroundAngle > _car.UpsideDownAngleToUnlockControlsX ? hit.point.y : diffY, hit.point.z), _boosting);

                //Check position of the hit point so that it can accelerate when you shoot the ray high enough and deccelerate when you shoot the ray low enough
                if (_car.Grounded && Physics.Raycast(rayAccel, out hitAccel, Mathf.Infinity, _layer)) {
                    float clampedY = (Mathf.Clamp(hitAccel.point.y, -1, 6) + 1) / 7;

                    float multiplier = 1;

                    float upperTreshold = _tresholdToAccelerate;
                    float lowerTreshold = _tresholdToDeccelerate;

                    if (clampedY < lowerTreshold) {
                        multiplier -= _speedMultiplierAdded;
                    } else if (clampedY > upperTreshold) {
                        multiplier += _speedMultiplierAdded;
                    }

                    //Give multiplier to the speedmanager
                    _speedManager.CalculateModifedSpeed(multiplier);
                }
            }
        }

        private void BoostCheck() {
            if (_boostTimer > 0) {
                //When pressing boost decrease timer an send speed to speedManager
                if (Input.GetButton("Boost") || OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch)) {
                    _boostTimer -= Time.deltaTime;
                    _boosting = true;
                    _speedManager.CalculateModifedSpeed(2);
                } else {
                    //When releasing boost the meter will fill up as fast as the given cooldown
                    _boosting = false;
                    if (_boostTimer < _boostDuration) {
                        _boostTimer += _fillBoostSpeed * Time.deltaTime;
                    }
                }
            } else if (!_boostInCooldown) {
                //When the boost is done then you get an cooldown so that you can only boost again when the cooldown ends
                _boosting = false;
                _boostInCooldown = true;

                StartCoroutine(nameof(BoostCooldown));
            }
        }

        private IEnumerator BoostCooldown() {
            yield return new WaitForSeconds(_boostCooldown);

            _boostInCooldown = false;
            _boostTimer = _boostDuration;
        }

        public void BoostTime(float time) {
            _boosttime = time;
        }
        private void deactivateboost() {
            if (_boosting) {
                _boosttime -= Time.deltaTime;
                if(_boosttime <= 0) {
                    _boosting = false;
                }
            }
        }
    }
}