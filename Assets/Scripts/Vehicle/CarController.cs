using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.VFX;
using XRLab.VRoem.Core;

//TODO: Fix code conventions, refactor into data classes
namespace XRLab.VRoem.Vehicle {
    public class CarController : MonoBehaviour {
        [Header("The Controllers")]
        [SerializeField] private Transform _rightHandAnchor;

        [Header("Misc")]
        [SerializeField] private float _speedMultiplierAdded = 0.2f;
        [SerializeField] private float _fillBoostSpeed = 0.25f;
        [SerializeField] private float _tresholdToAccelerate = 0.8f;
        [SerializeField] private float _tresholdToDeccelerate = 0.2f;
        [SerializeField] private Vector3 _positionModifier;
        [SerializeField] private LayerMask _layer;
        [SerializeField] private Color _rayColor = Color.red;

        [Header("Audio")]
        public AudioSource _carBoostAudio;

        //added by Bouke
        [Header("Boosting")]
        private float _boosttime;
        [SerializeField] private GameObject boostEffects1, boostEffects2;
        [SerializeField] private float _boostDuration = 1f;
        [SerializeField] private float _boostCooldown = 5f;
        [SerializeField] private float _boostTimer = 1;
        [HideInInspector] public bool _boosting = false;
        private bool _boostInCooldown = false;

        private SimpleMovementCar _car;
        private LineRenderer _lineRenderer;
        private SpeedManager _speedManager;
        private Transform _vrCam;

        private bool _mouseControl = false;

        private void Start() {
            _car = GetComponent<SimpleMovementCar>();
            _lineRenderer = GetComponent<LineRenderer>();
            _speedManager = FindObjectOfType<SpeedManager>();
            _boostTimer = _boostDuration;
            _vrCam = GameObject.FindGameObjectWithTag(Tags.OVR).transform;

            //When releasing boost before the boost meter is empty this variable will fill it up instead of the normal cooldown
            //It does a calculation to make sure it will take as long as the given cooldown
            _fillBoostSpeed = 1 / (1 / _boostDuration * _boostCooldown);

        }


        private void Update() {
            ShootControlRay();
            deactivateboost();
        }

        #region Conroller Position Moves Car
        private void ShootControlRay() {

            if (Input.GetKeyDown(KeyCode.C))
            {
                _mouseControl = !_mouseControl;
            }

            //Shoot Ray from right hand or mouse
            Ray ray = _mouseControl ? Camera.main.ScreenPointToRay(Input.mousePosition) : new Ray(_rightHandAnchor.position, Vector3.forward);
            RaycastHit hit;
            Ray rayAccel = _mouseControl ? ray : new Ray(_rightHandAnchor.position, _rightHandAnchor.forward);
            RaycastHit hitAccel;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layer)) {

                _car.SetOrientation(new Vector3(hit.point.x * (_mouseControl ? 1 : _positionModifier.x), hit.point.y, hit.point.z), _boosting);

                //Check position of the hit point so that it can accelerate when you shoot the ray high enough and deccelerate when you shoot the ray low enough
                if (!_boosting && _car.Grounded && Physics.Raycast(rayAccel, out hitAccel, Mathf.Infinity, _layer)) {
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
        #endregion
        #region Boosting
        public void BoostTime(float time) {
            _boosttime = time;
        }
        private void deactivateboost() {
            if (_boosting) {
                if (_speedManager.FinalSpeed == 0)
                {
                    _boosting = false;
                }

                _boosttime -= Time.deltaTime;
                boostEffects1.SetActive(true);
                boostEffects2.SetActive(true);
                if (_boosttime <= 0) {
                    _boosting = false;
                    boostEffects1.SetActive(false);
                    boostEffects2.SetActive(false);
                }
            }
        }

        #endregion
    }
}